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
        public override string Name => "GalacticBank";
        public override uint? NetID => ContentNetIDs.GALACTIC_BANK;
        public override Type StateType => typeof(GalacticBankState);

        protected GalacticBankAccount _account;

        public GalacticBankAccount Account => _account;

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            if (serializer.Reading)
            {
                _account = serializer.ReadDataField<GalacticBankAccount>("account");
            }
            else if (serializer.Writing)
            {
                serializer.DataField(ref _account, "account", null);
            }
        }
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
