using System;
using System.Collections.Generic;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Shared.GameObjects.Components.HUD.Hotbar
{
    public class SharedHotbarComponent : Component
    {
        public override string Name => "Hotbar";
        public sealed override uint? NetID => ContentNetIDs.HOTBAR;
    }

    [Serializable, NetSerializable]
    public class HotbarComponentState : ComponentState
    {
        public List<AbilityData> Abilities;

        public HotbarComponentState(List<AbilityData> abilities) : base(ContentNetIDs.HOTBAR)
        {
            Abilities = abilities;
        }
    }

    public class AbilityData
    {
        public readonly string TexturePath;

        public AbilityData(string texturePath)
        {
            TexturePath = texturePath;
        }
    }

    [Serializable, NetSerializable]
    public class TriggerAbilityMsg : ComponentMessage
    {
        public readonly int Index;

        public TriggerAbilityMsg(int index)
        {
            Index = index;
        }
    }

    [Serializable, NetSerializable]
    public class AbilityTriggeredMsg : ComponentMessage
    {
        public readonly int Index;
        public readonly TimeSpan Start;
        public readonly TimeSpan End;

        public AbilityTriggeredMsg(int index, TimeSpan start, TimeSpan end)
        {
            Index = index;
            Start = start;
            End = end;
        }
    }

    public class AbilityStateMsg : ComponentMessage
    {
        public readonly int Index;
        public readonly bool Active;

        public AbilityStateMsg(int index, bool active)
        {
            Index = index;
            Active = active;
        }
    }
}
