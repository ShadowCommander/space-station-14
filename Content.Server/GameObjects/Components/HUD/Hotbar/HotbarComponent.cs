using System;
using System.Collections.Generic;
using System.Linq;
using Content.Shared.GameObjects.Components.HUD.Hotbar;
using Robust.Server.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Network;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Players;
using Robust.Shared.ViewVariables;

namespace Content.Server.GameObjects.Components.HUD.Hotbar
{
    [RegisterComponent]
    public class HotbarComponent : SharedHotbarComponent
    {
#pragma warning disable 649
        [Dependency] private readonly IGameTiming _gameTiming;
#pragma warning restore 649

        [ViewVariables]
        private List<Ability> _abilities;
        private List<Ability> _hotbar;

        public Ability SelectedAbility;

        public override void Initialize()
        {
            base.Initialize();

            _hotbar = new List<Ability>(10);
            SelectedAbility = null;
            //CollectAbilities();

            Dirty();
        }

        public override void HandleMessage(ComponentMessage message, IComponent component)
        {
            base.HandleMessage(message, component);

            switch (message)
            {
                case TriggerAbilityMsg msg:
                {
                    if (msg.Index < 0 || msg.Index >= _hotbar.Count)
                    {
                        return;
                    }

                    var ability = _hotbar.ElementAtOrDefault(msg.Index);
                    if (ability == null)
                    {
                        break;
                    }

                    SelectAbility(ability);
                    break;
                }
            }
        }

        /// <summary>
        ///     Send the start and end time of each ability. This is for the cooldown graphic.
        ///     Send whether the ability is active. For highlighting the edge of the ability.
        ///     Send Texture of ability
        /// </summary>
        /// <returns></returns>
        public override ComponentState GetComponentState()
        {
            var hotbar = new List<AbilityData>();
            foreach (var ability in _hotbar)
            {
                hotbar.Add(new AbilityData(ability.TexturePath));
            }
            return new HotbarComponentState(hotbar);
        }

        /// <summary>
        ///     Sends an entity message to refresh the ability list
        /// </summary>
        public void CollectAbilities()
        {
            _abilities.Clear();
            SendMessage(new CollectAbilitiesMessage());
        }

        /// <summary>
        ///     For adding abilities to the ability list
        /// </summary>
        /// <param name="ability"></param>
        public void AddAbility(Ability ability)
        {
            _abilities.Add(ability);
        }

        public void RemoveAbility(Ability ability)
        {
            _abilities.Remove(ability);
        }

        /// <summary>
        ///     For setting the current active ability that will be triggered by MiddleClick
        /// </summary>
        /// <param name="ability"></param>
        public void SelectAbility(Ability ability)
        {
            SelectedAbility.DeselectAbility();
            SelectedAbility = ability;
            SelectedAbility.SelectAbility();
        }

        public class Ability
        {
#pragma warning disable 649
        [Dependency] private readonly IGameTiming _gameTiming;
#pragma warning restore 649

            public Action<ICommonSession, GridCoordinates, EntityUid, TimeSpan> Action { get; }
            public string TexturePath;
            public bool Selected;

            public Ability(Action<ICommonSession, GridCoordinates, EntityUid, TimeSpan> action, string texturePath)
            {
                Action = action;
                TexturePath = texturePath;
                Selected = false;
            }

            public void Trigger(ICommonSession session, GridCoordinates coords, EntityUid uid, TimeSpan curTime)
            {
//                if (curTime < CooldownEnd)
//                {
//                    return;
//                }
                Action?.Invoke(session, coords, uid, curTime);
//                if (Cooldown > 0)
//                {
//                    CooldownStart = _gameTiming.CurTime;
//                    CooldownEnd = CooldownStart + TimeSpan.FromSeconds(Cooldown);
//                }
            }

            public void SelectAbility()
            {
                Selected = true;
            }

            public void DeselectAbility()
            {
                Selected = false;
            }
        }
    }

    /// <summary>
    ///     Broadcasts to other components to add their abilites to the ability list.
    /// </summary>
    public class CollectAbilitiesMessage : ComponentMessage
    {
    }
}
