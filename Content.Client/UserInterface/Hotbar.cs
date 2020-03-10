using System;
using System.Collections.Generic;
using Content.Client.Utility;
using Robust.Client.Console;
using Robust.Client.Graphics;
using Robust.Client.Graphics.Drawing;
using Robust.Client.Interfaces.ResourceManagement;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.IoC;
using Robust.Shared.Maths;

namespace Content.Client.UserInterface
{
    public class Hotbar : PanelContainer
    {
        private readonly IResourceCache _resourceCache;
        private readonly IClientConsole _console;

        private const int _slotCount = 10;

        private List<string> _list = new List<string>();
        private List<HotbarSlot> _slots = new List<HotbarSlot>();

        private TextureButton _hideButton;

        public Hotbar()
        {
            _resourceCache = IoCManager.Resolve<IResourceCache>();
            _console = IoCManager.Resolve<IClientConsole>();

            var vBox = new VBoxContainer();
            AddChild(vBox);

            HotbarSlot CreateSlot(int index)
            {
                var button = new HotbarSlot(null, index);
                button.OnPressed += OnPressed;
                _list.Add("");
                _slots.Add(button);
                return button;
            }

            var zero = CreateSlot(0);
            vBox.AddChild(CreateSlot(1));
            vBox.AddChild(CreateSlot(2));
            vBox.AddChild(CreateSlot(3));
            vBox.AddChild(CreateSlot(4));
            vBox.AddChild(CreateSlot(5));
            vBox.AddChild(CreateSlot(6));
            vBox.AddChild(CreateSlot(7));
            vBox.AddChild(CreateSlot(8));
            vBox.AddChild(CreateSlot(9));
            vBox.AddChild(zero);

            SetSlot(1, "say \"Hello, world!\"", _resourceCache.GetTexture("/Textures/UserInterface/Inventory/back.png"));
        }

        private void OnPressed(BaseButton.ButtonEventArgs args)
        {
            if (!(args.Button is HotbarSlot hotbarSlot))
            {
                return;
            }
            _console.ProcessCommand(_list[hotbarSlot.Index]);
        }

        private void SetSlot(int index, string command, Texture texture)
        {
            if (index >= _list.Count)
            {
                return;
            }
            _list[index] = command;
            _slots[index].TextureNormal = texture;
        }

        public class HotbarSlot : TextureButton
        {
            public const string StyleClassButtonRect = "buttonRect";


            public int Index;

            public HotbarSlot(Texture background, int index)
            {
                AddStyleClass(StyleClassButtonRect);
                CustomMinimumSize = (64, 64);

                Index = index;

                AddChild(new Label
                {
                    Text = index.ToString(),
                    SizeFlagsVertical = SizeFlags.None
                });
            }
        }
    }
}
