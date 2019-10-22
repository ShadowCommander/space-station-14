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
        public sealed override string Name => "CargoConsole";

#pragma warning disable CS0649
        [Dependency]
        protected IPrototypeManager _prototypeManager;
#pragma warning restore

        /// <summary>
        ///     Sent to the server to request a sync of market and order lists.
        /// </summary>
        [Serializable, NetSerializable]
        public class CargoConsoleSyncRequestMessage : BoundUserInterfaceMessage
        {
            public CargoConsoleSyncRequestMessage()
            {
            }
        }

        /// <summary>
        ///     Sent to the client to let it know about the market and orders.
        /// </summary>
        [Serializable, NetSerializable]
        public class CargoConsoleOrderDataMessage : BoundUserInterfaceMessage
        {
            public readonly List<CargoOrderData> Orders;

            public CargoConsoleOrderDataMessage(List<CargoOrderData> orders)
            {
                Orders = orders;
            }
        }

        /// <summary>
        ///     Request that the server updates the client.
        /// </summary>
        [Serializable, NetSerializable]
        public class CargoConsoleAddOrderMessage : BoundUserInterfaceMessage
        {
            public string Requester;
            public string Reason;
            public string ProductId;
            public int Amount;

            public CargoConsoleAddOrderMessage(string requester, string reason, string productId, int amount)
            {
                Requester = requester;
                Reason = reason;
                ProductId = productId;
                Amount = amount;
            }
        }

        [NetSerializable, Serializable]
        public enum CargoConsoleUiKey
        {
            Key
        }
    }

    [NetSerializable, Serializable]
    public class CargoConsoleInterfaceState : BoundUserInterfaceState
    {
        public readonly int Id;
        public readonly string Name;
        public readonly int Balance;

        public CargoConsoleInterfaceState(int id, string name, int balance)
        {
            Id = id;
            Name = name;
            Balance = balance;
        }
    }
}
