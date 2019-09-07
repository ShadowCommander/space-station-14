using Content.Shared.Prototypes.Cargo;
using Robust.Shared.GameObjects;
using Robust.Shared.GameObjects.Components.UserInterface;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.GameObjects.Components.Cargo
{
    public class SharedCargoConsoleComponent : Component
    {
        public override string Name => "CargoConsole";
        public override uint? NetID => ContentNetIDs.CARGO_CONSOLE;

        /// <summary>
        ///     Request that the server updates the client.
        /// </summary>
        [Serializable, NetSerializable]
        public class CargoConsoleAddOrder : BoundUserInterfaceMessage
        {
            public CargoOrderData Order;

            public CargoConsoleAddOrder(CargoOrderData order)
            {
                Order = order;
            }
        }

        [NetSerializable, Serializable]
        public enum CargoConsoleUiKey
        {
            Key
        }
    }
}
