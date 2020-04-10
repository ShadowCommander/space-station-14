using System;
using Content.Server.GameObjects.Components.HUD.Hotbar;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Players;

namespace Content.Server.GameObjects.Components.Mobs.Abilities
{
    public abstract class AbilityComponent : Component
    {
        public float Cooldown;
        public TimeSpan CooldownStart;
        public TimeSpan CooldownEnd;

        public HotbarComponent.Ability Ability;

        public override void Initialize()
        {
            base.Initialize();

            Cooldown = 0.0f;
            CooldownStart = TimeSpan.Zero;
            CooldownEnd = TimeSpan.Zero;
        }

        public override void OnRemove()
        {
            base.OnRemove();

            if (Owner.TryGetComponent(out HotbarComponent hotbarComponent))
            {
                hotbarComponent.RemoveAbility(Ability);
            }
        }

        public abstract void TriggerAbility(ICommonSession session, GridCoordinates coords, EntityUid uid, TimeSpan curTime);
/*        {
            if (curTime < CooldownEnd)
            {
                return;
            }

            if (Cooldown > 0.0f)
            {
                CooldownStart = _gameTiming.CurTime;
                CooldownEnd = CooldownStart + TimeSpan.FromSeconds(Cooldown);
            }
        }
*/    }
}
