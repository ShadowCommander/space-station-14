﻿using System;
using Content.Client.Animations;
using Content.Client.UserInterface;
using Content.Client.Utility;
using Content.Shared.GameObjects;
using Content.Shared.GameObjects.Components.Weapons.Ranged;
using Robust.Client.Animations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Animations;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Network;
using Robust.Shared.Maths;
using Robust.Shared.Serialization;
using Robust.Shared.ViewVariables;
using static Content.Client.StaticIoC;

namespace Content.Client.GameObjects.Components.Weapons.Ranged
{
    [RegisterComponent]
    public class BallisticMagazineWeaponComponent : Component, IItemStatus
    {
        private static readonly Animation AlarmAnimationSmg = new Animation
        {
            Length = TimeSpan.FromSeconds(1.4),
            AnimationTracks =
            {
                new AnimationTrackControlProperty
                {
                    // These timings match the SMG audio file.
                    Property = nameof(Label.FontColorOverride),
                    InterpolationMode = AnimationInterpolationMode.Previous,
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(Color.Red, 0.1f),
                        new AnimationTrackProperty.KeyFrame(null, 0.3f),
                        new AnimationTrackProperty.KeyFrame(Color.Red, 0.2f),
                        new AnimationTrackProperty.KeyFrame(null, 0.3f),
                        new AnimationTrackProperty.KeyFrame(Color.Red, 0.2f),
                        new AnimationTrackProperty.KeyFrame(null, 0.3f),
                    }
                }
            }
        };

        private static readonly Animation AlarmAnimationLmg = new Animation
        {
            Length = TimeSpan.FromSeconds(0.75),
            AnimationTracks =
            {
                new AnimationTrackControlProperty
                {
                    // These timings match the SMG audio file.
                    Property = nameof(Label.FontColorOverride),
                    InterpolationMode = AnimationInterpolationMode.Previous,
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(Color.Red, 0.0f),
                        new AnimationTrackProperty.KeyFrame(null, 0.15f),
                        new AnimationTrackProperty.KeyFrame(Color.Red, 0.15f),
                        new AnimationTrackProperty.KeyFrame(null, 0.15f),
                        new AnimationTrackProperty.KeyFrame(Color.Red, 0.15f),
                        new AnimationTrackProperty.KeyFrame(null, 0.15f),
                    }
                }
            }
        };

        private static readonly Texture BulletAtlas = ResC.GetTexture("/Textures/UserInterface/status/bullets/bullets.png");

        public override string Name => "BallisticMagazineWeapon";
        public override uint? NetID => ContentNetIDs.BALLISTIC_MAGAZINE_WEAPON;
        public override Type StateType => typeof(BallisticMagazineWeaponComponentState);

        private StatusControl _statusControl;

        /// <summary>
        ///     True if a bullet is chambered.
        /// </summary>
        [ViewVariables]
        public bool Chambered { get; private set; }

        /// <summary>
        ///     Count of bullets in the magazine.
        /// </summary>
        /// <remarks>
        ///     Null if no magazine is inserted.
        /// </remarks>
        [ViewVariables]
        public (int count, int max)? MagazineCount { get; private set; }

        [ViewVariables] private BallisticCaliber _caliber;

        [ViewVariables(VVAccess.ReadWrite)] private bool _isLmgAlarmAnimation;

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            serializer.DataField(ref _isLmgAlarmAnimation, "lmg_alarm_animation", false);
        }

        public override void HandleComponentState(ComponentState curState, ComponentState nextState)
        {
            var cast = (BallisticMagazineWeaponComponentState) curState;

            Chambered = cast.Chambered;
            MagazineCount = cast.MagazineCount;
            _caliber = cast.Caliber;
            _statusControl?.Update();
        }

        public override void HandleMessage(ComponentMessage message, INetChannel netChannel = null,
            IComponent component = null)
        {
            switch (message)
            {
                case BmwComponentAutoEjectedMessage _:
                    _statusControl?.PlayAlarmAnimation();
                    return;
            }

            base.HandleMessage(message, netChannel, component);
        }

        public Control MakeControl()
        {
            _statusControl = new StatusControl(this);
            _statusControl.Update();
            return _statusControl;
        }

        public void DestroyControl(Control control)
        {
            if (_statusControl == control)
            {
                _statusControl = null;
            }
        }

        private sealed class StatusControl : Control
        {
            private readonly BallisticMagazineWeaponComponent _parent;
            private readonly HBoxContainer _bulletsListTop;
            private readonly HBoxContainer _bulletsListBottom;
            private readonly TextureRect _chamberedBullet;
            private readonly Label _noMagazineLabel;

            public StatusControl(BallisticMagazineWeaponComponent parent)
            {
                _parent = parent;
                SizeFlagsHorizontal = SizeFlags.FillExpand;
                SizeFlagsVertical = SizeFlags.ShrinkCenter;
                AddChild(new VBoxContainer
                {
                    SizeFlagsHorizontal = SizeFlags.FillExpand,
                    SizeFlagsVertical = SizeFlags.ShrinkCenter,
                    SeparationOverride = 0,
                    Children =
                    {
                        (_bulletsListTop = new HBoxContainer {
                            SeparationOverride = 0
                        }),
                        new HBoxContainer
                        {
                            SizeFlagsHorizontal = SizeFlags.FillExpand,
                            Children =
                            {
                                (_bulletsListBottom = new HBoxContainer
                                {
                                    SizeFlagsVertical = SizeFlags.ShrinkCenter,
                                    SeparationOverride = 0
                                }),
                                (_chamberedBullet = new TextureRect
                                {
                                    Texture = ResC.GetTexture("/Textures/UserInterface/status/bullets/chambered.png"),
                                    SizeFlagsVertical = SizeFlags.ShrinkCenter,
                                    SizeFlagsHorizontal = SizeFlags.ShrinkEnd | SizeFlags.Fill,
                                })
                            }
                        }
                    }
                });
                AddChild(_noMagazineLabel = new Label
                {
                    Text = "No Magazine!",
                    StyleClasses = { NanoStyle.StyleClassItemStatus }
                });
            }

            public void Update()
            {
                _chamberedBullet.ModulateSelfOverride =
                    _parent.Chambered ? Color.FromHex("#d7df60") : Color.Black;

                _bulletsListTop.RemoveAllChildren();
                _bulletsListBottom.RemoveAllChildren();

                if (_parent.MagazineCount == null)
                {
                    _noMagazineLabel.Visible = true;
                    return;
                }

                var (count, capacity) = _parent.MagazineCount.Value;

                _noMagazineLabel.Visible = false;

                AtlasTexture texture;
                if (_parent._caliber == BallisticCaliber.Unspecified)
                {
                    texture = new AtlasTexture(BulletAtlas, new UIBox2(90, 30, 94, 40));
                }
                else
                {
                    if (_parent._caliber == BallisticCaliber.A762mm)
                    {
                        texture = new AtlasTexture(BulletAtlas, new UIBox2(38, 9, 44, 40));
                    }
                    else if (_parent._caliber == BallisticCaliber.A44)
                    {
                        texture = new AtlasTexture(BulletAtlas, new UIBox2(70, 21, 74, 40));
                    }
                    else
                    {
                        texture = new AtlasTexture(BulletAtlas, new UIBox2(90, 30, 94, 40));
                    }
                }


                var maxRow = 120 / texture.Width;

                if (capacity > maxRow)
                {
                    FillBulletRow(_bulletsListBottom, Math.Min(maxRow, count), maxRow, texture);
                    FillBulletRow(_bulletsListTop, Math.Max(0, count - maxRow), capacity - maxRow, texture);
                }
                else
                {
                    FillBulletRow(_bulletsListBottom, count, capacity, texture);
                }
            }

            private static void FillBulletRow(Control container, int count, int capacity, Texture texture)
            {
                var colorA = Color.FromHex("#b68f0e");
                var colorB = Color.FromHex("#d7df60");
                var colorGoneA = Color.FromHex("#000000");
                var colorGoneB = Color.FromHex("#222222");

                var altColor = false;

                for (var i = count; i < capacity; i++)
                {
                    container.AddChild(new TextureRect
                    {
                        Texture = texture,
                        ModulateSelfOverride = altColor ? colorGoneA : colorGoneB
                    });

                    altColor ^= true;
                }

                for (var i = 0; i < count; i++)
                {
                    container.AddChild(new TextureRect
                    {
                        Texture = texture,
                        ModulateSelfOverride = altColor ? colorA : colorB
                    });

                    altColor ^= true;
                }
            }

            protected override Vector2 CalculateMinimumSize()
            {
                return Vector2.ComponentMax((0, 15), base.CalculateMinimumSize());
            }

            public void PlayAlarmAnimation()
            {
                var animation = _parent._isLmgAlarmAnimation ? AlarmAnimationLmg : AlarmAnimationSmg;
                _noMagazineLabel.PlayAnimation(animation, "alarm");
            }
        }
    }
}
