using Content.Shared.DatabaseData;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Content.Shared.GameObjects.Components.Cargo
{
    public class SharedGalacticBankComponent : Component
    {
        public sealed override string Name => "GalacticBank";
        public sealed override uint? NetID => ContentNetIDs.GALACTIC_BANK;
        public sealed override Type StateType => typeof(GalacticBankState);
    }

    [NetSerializable, Serializable]
    public class GalacticBankState : ComponentState
    {
        public GalacticBankAccount Account;

        public GalacticBankState(GalacticBankAccount account) : base(ContentNetIDs.GALACTIC_BANK)
        {
            Account = account;
        }
    }
}
