using Content.Shared.DatabaseData;
using Content.Shared.GameObjects.Components.Cargo;
using Robust.Shared.GameObjects;

namespace Content.Client.GameObjects.Components.Cargo
{
    public class GalacticBankComponent : SharedGalacticBankComponent
    {
        public GalacticBankAccount Account { get; set; }

        public override void HandleComponentState(ComponentState curState, ComponentState nextState)
        {
            base.HandleComponentState(curState, nextState);
            if (!(curState is GalacticBankState state))
                return;
            Account = state.Account;
        }
    }
}
