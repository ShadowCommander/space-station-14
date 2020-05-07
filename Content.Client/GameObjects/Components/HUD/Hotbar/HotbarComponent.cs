using System;
using System.Collections.Generic;
using Content.Client.UserInterface;
using Content.Client.Utility;
using Content.Shared.GameObjects.Components.HUD.Hotbar;
using Robust.Client.Console;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Interfaces.ResourceManagement;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Network;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.IoC;
using Robust.Shared.Map;

namespace Content.Client.GameObjects.Components.HUD.Hotbar
{
    /*
     * Hotbar holds 10 abilites
     * Abilities
     *   Toggle on/off (Highlight edge)
     *   Cooldown graphic (Like ItemSlot cooldown)
     *   Texture
     */
    [RegisterComponent]
    public class HotbarComponent : SharedHotbarComponent
    {
#pragma warning disable 649
        [Dependency] private readonly IResourceCache _resourceCache;
        [Dependency] private readonly IGameHud _gameHud;
        [Dependency] private readonly IClientConsole _console;
        [Dependency] private readonly IGameTiming _timing;
#pragma warning restore 649

        private List<Ability> _hotbar;
        private List<Ability> _abilities;
        private List<Func<bool>> _actions;
        private HotbarGui _gui;

        public override void Initialize()
        {
            base.Initialize();

            _actions = new List<Func<bool>>();
            _gui = new HotbarGui();
            _gui.OnPressed = OnPressed;

            // TODO Testing
            //_gui.SetSlot(1, _resourceCache.GetTexture("/Textures/UserInterface/Inventory/back.png"));
            //if (_actions.Count > 2)
            //    _actions[1] += () => { return ExecuteConsoleCommand("say \"Hello, world!\""); };
        }

        // TODO Begin testing
        private bool ExecuteConsoleCommand(string str)
        {
            _console.ProcessCommand(str);
            return true;
        }
        // End testing

        public override void OnRemove()
        {
            base.OnRemove();

            _gui?.Dispose();
        }

        public override void HandleMessage(ComponentMessage message, IComponent component = null)
        {
            base.HandleMessage(message, component);

            switch (message)
            {
                case PlayerAttachedMsg _:
                {
                    if (_gui == null)
                    {
                        _gui = new HotbarGui();
                    }
                    else
                    {
                        _gui.Parent?.RemoveChild(_gui);
                    }

                    _gameHud.HotbarContainer.AddChild(_gui);
                    break;
                }
                case PlayerDetachedMsg _:
                {
                    _gui.Parent?.RemoveChild(_gui);
                    break;
                }
                case AbilityTriggeredMsg msg:
                {
                    _abilities[msg.Index].CooldownStart = msg.Start;
                    _abilities[msg.Index].CooldownEnd = msg.End;
                    break;
                }
                case AbilityStateMsg msg:
                {
                    _abilities[msg.Index].Active = msg.Active;
                    break;
                }
            }
        }
/*
        public override void HandleComponentState(ComponentState curState, ComponentState nextState)
        {
            base.HandleComponentState(curState, nextState);

            var cast = (HotbarComponentState) curState;

            if (cast.Size > _actions.Count)
            {
                for (var i = _actions.Count; i < cast.Size; i++)
                {
                    _actions.Add(null);
                }
            }
            else if (cast.Size > _actions.Count)
            {
                for (var i = _actions.Count; i > cast.Size; i--)
                {
                    _actions.RemoveAt(i - 1);
                }
            }
        }*/

        /// <summary>
        /// Trigger on client, then if not handled, trigger on server.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index">Ability slot index on hotbar</param>
        private void OnPressed(BaseButton.ButtonEventArgs args, int index)
        {
            if (index < 0 || index > _actions.Count)
            {
                return;
            }

            if (_actions[index] == null || _actions[index].Invoke())
            {
                return;
            }

            SendActivateSlotMessage(index);
        }

        private void SendActivateSlotMessage(int index)
        {
            SendMessage(new TriggerAbilityMsg(index));
        }

        public void ActivateAbility(int index, GridCoordinates pos)
        {
            _hotbar[index].Action?.Invoke(pos);
        }
    }

    public class AbilityButton : ContainerButton
    {
        public SpriteView SpriteView;
        public TextureRect CooldownTexture;
    }

    public class Ability
    {
        public Texture Texture;
        public TimeSpan? CooldownStart;
        public TimeSpan? CooldownEnd;
        public bool Active;
        public Action<GridCoordinates> Action;

        public Ability(Texture texture, Action<GridCoordinates> action)
        {
            Texture = texture;
            CooldownStart = null;
            CooldownEnd = null;
            Active = false;
            Action = action;
        }
    }
}
