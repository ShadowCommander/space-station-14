using System;
using System.Collections.Generic;
using Content.Server.GameObjects.Components.HUD.Hotbar;
using Content.Server.GameObjects.Components.Sound;
using Content.Shared.GameObjects;
using Content.Shared.GameObjects.Components.HUD.Hotbar;
using Content.Shared.GameObjects.Components.Mobs.Abilities;
using Content.Shared.Input;
using Content.Shared.Physics;
using Robust.Server.GameObjects.EntitySystems;
using Robust.Server.Interfaces.Player;
using Robust.Shared.Audio;
using Robust.Shared.GameObjects;
using Robust.Shared.GameObjects.EntitySystemMessages;
using Robust.Shared.Input;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Network;
using Robust.Shared.Interfaces.Physics;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Physics;
using Robust.Shared.Players;
using Robust.Shared.Serialization;

namespace Content.Server.GameObjects.Components.Mobs.Abilities
{
    public class LaserAbilityComponent : SharedLaserAbilityComponent
    {
#pragma warning disable 649
        [Dependency] private readonly IEntitySystemManager _entitySystemManager;
#pragma warning restore 649

        private const float MaxLength = 20;

        string _spritename;
        private int _damage;
        private int _baseFireCost;
        private float _lowerChargeLimit;
        private string _fireSound;

        public HotbarComponent.Ability Ability;

        public override void Initialize()
        {
            base.Initialize();

            //Cooldown = 10;

            //Ability = new HotbarComponent.Ability(TriggerAbility, "Textures/Objects/Guns/Laser/laser_cannon.rsi/laser_cannon.png");

            if (Owner.TryGetComponent(out HotbarComponent hotbarComponent))
            {
                hotbarComponent.AddAbility(Ability);
            }
        }

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            serializer.DataField(ref _spritename, "fireSprite", "Objects/laser.png");
            serializer.DataField(ref _damage, "damage", 10);
            serializer.DataField(ref _baseFireCost, "baseFireCost", 300);
            serializer.DataField(ref _lowerChargeLimit, "lowerChargeLimit", 10);
            serializer.DataField(ref _fireSound, "fireSound", "/Audio/laser.ogg");
        }

        public override void HandleMessage(ComponentMessage message, IComponent component = null)
        {
            base.HandleMessage(message, component);

            switch (message)
            {
                case CollectAbilitiesMessage _:
                {
                    if (Owner.TryGetComponent(out HotbarComponent hotbarComponent))
                    {
                        hotbarComponent.AddAbility(Ability);
                    }
                    break;
                }
            }
        }

        public override void HandleNetworkMessage(ComponentMessage message, INetChannel netChannel, ICommonSession session = null)
        {
            base.HandleNetworkMessage(message, netChannel, session);

            switch (message)
            {
                case TriggerAbilityMsg msg:
                {
                    TriggerAbility(session, msg.Pos);
                    break;
                }
            }
        }

        public void TriggerAbility(ICommonSession session, GridCoordinates coords)
        {
            //if (curTime < CooldownEnd)
            //{
            //    return;
            //}

            //CooldownStart = curTime;
            //CooldownEnd = CooldownStart + TimeSpan.FromSeconds(Cooldown);

            var player = ((IPlayerSession) session).AttachedEntity;
            Fire(player, coords);
            return;
        }

        private void Fire(IEntity user, GridCoordinates clickLocation)
        {
            var userPosition = user.Transform.WorldPosition; //Remember world positions are ephemeral and can only be used instantaneously
            var angle = new Angle(clickLocation.Position - userPosition);

            var ray = new CollisionRay(userPosition, angle.ToVec(), (int) (CollisionGroup.Impassable | CollisionGroup.MobImpassable));
            var rayCastResults = IoCManager.Resolve<IPhysicsManager>().IntersectRay(user.Transform.MapID, ray, MaxLength, user, ignoreNonHardCollidables: true);

            Hit(rayCastResults, user);
            AfterEffects(user, rayCastResults, angle);
        }

        protected virtual void Hit(RayCastResults ray, IEntity user = null)
        {
            if (ray.HitEntity != null && ray.HitEntity.TryGetComponent(out DamageableComponent damage))
            {
                damage.TakeDamage(DamageType.Heat, _damage, Owner, user);
                //I used Math.Round over Convert.toInt32, as toInt32 always rounds to
                //even numbers if halfway between two numbers, rather than rounding to nearest
            }
        }

        protected virtual void AfterEffects(IEntity user, RayCastResults ray, Angle angle)
        {
            var time = IoCManager.Resolve<IGameTiming>().CurTime;
            var dist = ray.DidHitObject ? ray.Distance : MaxLength;
            var offset = angle.ToVec() * dist / 2;
            var message = new EffectSystemMessage
            {
                EffectSprite = _spritename,
                Born = time,
                DeathTime = time + TimeSpan.FromSeconds(1),
                Size = new Vector2(dist, 1f),
                Coordinates = user.Transform.GridPosition.Translated(offset),
                //Rotated from east facing
                Rotation = (float) angle.Theta,
                ColorDelta = new Vector4(0, 0, 0, -1500f),
                Color = new Vector4(255, 255, 255, 750),

                Shaded = false
            };
            var mgr = IoCManager.Resolve<IEntitySystemManager>();
            mgr.GetEntitySystem<EffectSystem>().CreateParticle(message);
            Owner.GetComponent<SoundComponent>().Play(_fireSound, AudioParams.Default.WithVolume(-5));
        }
    }
}
