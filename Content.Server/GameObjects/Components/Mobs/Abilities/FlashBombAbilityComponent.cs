using System;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Players;
using static Content.Server.GameObjects.Components.HUD.Hotbar.HotbarComponent;

namespace Content.Server.GameObjects.Components.Mobs.Abilities
{
    public class FlashBombAbilityComponent : AbilityComponent
    {
#pragma warning disable 649
        [Dependency] private readonly IGameTiming _gameTiming;
#pragma warning restore 649

        public override string Name => "FlashBombAbility";

        public Ability Ability;

        public override void Initialize()
        {
            base.Initialize();

            //Ability = new Ability(TriggerAbility, "", 10);
        }

        public override void TriggerAbility(ICommonSession session, GridCoordinates coords, EntityUid uid, TimeSpan curTime)
        {
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
    }
}
