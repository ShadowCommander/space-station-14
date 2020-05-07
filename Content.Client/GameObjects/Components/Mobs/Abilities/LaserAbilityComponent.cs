using Content.Client.GameObjects.Components.HUD.Hotbar;
using Content.Shared.GameObjects.Components.Mobs.Abilities;
using Robust.Shared.Map;

namespace Content.Client.GameObjects.Components.Mobs.Abilities
{
    public class LaserAbilityComponent : SharedLaserAbilityComponent
    {
        public Ability Ability;

        public override void Initialize()
        {
            base.Initialize();

            Ability = new Ability(null, TriggerAbility);
        }

        public void TriggerAbility(GridCoordinates pos)
        {
            SendNetworkMessage(new TriggerAbilityMsg(pos));
        }
    }
}
