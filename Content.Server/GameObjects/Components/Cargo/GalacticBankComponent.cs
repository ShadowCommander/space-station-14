using Content.Shared.GameObjects.Components.Cargo;
using Robust.Shared.GameObjects;

namespace Content.Server.GameObjects.Components.Cargo
{
    [RegisterComponent]
    public class GalacticBankComponent : SharedGalacticBankComponent
    {
        public override ComponentState GetComponentState()
        {
            return new GalacticBankState(_account);
        }
    }
}
