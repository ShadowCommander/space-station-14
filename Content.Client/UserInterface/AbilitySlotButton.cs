using System;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;

namespace Content.Client.UserInterface
{
    public class AbilitySlotButton : ContainerButton
    {
        public SpriteView SpriteView { get; }
        public TextureRect Texture { get; }
        public TextureRect CooldownCircle { get; internal set; }

        public AbilitySlotButton(Texture texture)
        {
            CustomMinimumSize = (64, 64);

            AddChild(Texture = new TextureRect
            {
                Texture = texture,
                Scale = (2, 2),
            });

            AddChild(SpriteView = new SpriteView
            {
                Scale = (2, 2),
                OverrideDirection = Direction.South
            });

            AddChild(CooldownCircle = new TextureRect
            {
                SizeFlagsHorizontal = SizeFlags.ShrinkCenter,
                SizeFlagsVertical = SizeFlags.ShrinkCenter,
                Stretch = TextureRect.StretchMode.KeepCentered,
                Scale = (2, 2),
                Visible = false,
            });
        }
    }
}
