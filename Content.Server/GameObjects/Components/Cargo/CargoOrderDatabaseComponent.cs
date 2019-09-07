using Content.Shared.GameObjects.Components.Cargo;
using Robust.Shared.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Server.GameObjects.Components.Cargo
{
    [RegisterComponent]
    public class CargoOrderDatabaseComponent : SharedCargoOrderDatabaseComponent
    {
        public override ComponentState GetComponentState()
        {
            return new CargoOrderDatabaseState(_orders);
        }
    }
}
