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
    public class HotbarGui : PanelContainer
    {
        private List<HotbarSlot> _slots = new List<HotbarSlot>();

        private VBoxContainer _vBox;
        private TextureButton _hideButton;

        public Action<BaseButton.ButtonEventArgs, int> OnPressed;

        public HotbarGui()
        {
            _vBox = new VBoxContainer();
            AddChild(_vBox);

            HotbarSlot CreateSlot(int index)
            {
                var button = new HotbarSlot(null, index);
                button.OnPressed += args =>
                {
                    OnPressed?.Invoke(args, index);
                };
                _slots.Add(button);
                return button;
            }

            var zero = CreateSlot(0);
            _vBox.AddChild(CreateSlot(1));
            _vBox.AddChild(CreateSlot(2));
            _vBox.AddChild(CreateSlot(3));
            _vBox.AddChild(CreateSlot(4));
            _vBox.AddChild(CreateSlot(5));
            _vBox.AddChild(CreateSlot(6));
            _vBox.AddChild(CreateSlot(7));
            _vBox.AddChild(CreateSlot(8));
            _vBox.AddChild(CreateSlot(9));
            _vBox.AddChild(zero);
        }

        public void SetSlot(int index, Texture texture)
        {
            if (index >= _slots.Count)
            {
                return;
            }
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
