using Content.Shared.DatabaseData;
using Content.Shared.GameObjects.Components.Cargo;
using Robust.Shared.GameObjects;

namespace Content.Server.GameObjects.Components.Cargo
{
    [RegisterComponent]
    public class GalacticBankComponent : SharedGalacticBankComponent
    {
        protected GalacticBankAccount _account;

        public GalacticBankAccount Account => _account;
        public override ComponentState GetComponentState()
        {
            return new GalacticBankState(_account);
        }
    }
}
