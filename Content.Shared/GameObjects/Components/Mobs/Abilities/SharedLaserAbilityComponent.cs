using System;
using System.Collections.Generic;
using System.Text;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.GameObjects.Components.Mobs.Abilities
{
    public class SharedLaserAbilityComponent : Component
    {
        public override string Name => "LaserAbility";
        public override uint? NetID => ContentNetIDs.LASER_ABILITY;

        [Serializable, NetSerializable]
        public class TriggerAbilityMsg : ComponentMessage
        {
            public readonly GridCoordinates Pos;

            public TriggerAbilityMsg(GridCoordinates pos)
            {
                Pos = pos;
            }
        }
    }
}
