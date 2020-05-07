using System;
using System.Collections.Generic;
using Robust.Client.Graphics;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.Serialization;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Client.GameObjects.Components.Mobs.Abilities
{
    public class AbilityComponent : Component
    {
        public override string Name => "Ability";

        private Dictionary<string, AbilityPrototype> _abilities;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            serializer.DataField(ref _abilities, "abilities", new Dictionary<string, AbilityPrototype>());
        }
    }

    public class AbilityPrototype : IExposeData
    {
        public Texture Texture;
        public TimeSpan? Cooldown;
        public bool Disabled;
        public bool Activated;
        public TimeSpan? CooldownStart;
        public TimeSpan? CooldownEnd;
        public Action<GridCoordinates> Action;

        public AbilityPrototype(Texture texture, Action<GridCoordinates> action)
        {
            Texture = texture;
            CooldownStart = null;
            CooldownEnd = null;
            Activated = false;
            Action = action;
        }

        public void ExposeData(ObjectSerializer serializer)
        {
            serializer.DataField(ref Cooldown, "cooldown", null);
        }
    }
}
