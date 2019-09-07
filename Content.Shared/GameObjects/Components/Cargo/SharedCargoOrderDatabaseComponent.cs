using Content.Shared.Prototypes.Cargo;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.ViewVariables;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Content.Shared.GameObjects.Components.Cargo
{
    public class SharedCargoOrderDatabaseComponent : Component, IEnumerable<CargoOrderData>
    {

        public override string Name => "CargoOrderDatabase";
        public sealed override uint? NetID => ContentNetIDs.CARGO_ORDER_DATABASE;
        public sealed override Type StateType => typeof(CargoOrderDatabaseState);

        [ViewVariables]
        public List<CargoOrderData> _orders { get; } = new List<CargoOrderData>();

        private int _orderNumber;

        public IEnumerator<CargoOrderData> GetEnumerator()
        {
            return _orders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Removes all orders from the database.
        /// </summary>
        public virtual void Clear()
        {
            _orders.Clear();
        }

        /// <summary>
        ///     Adds a order to the database.
        /// </summary>
        /// <param name="order">The order to be added.</param>
        public virtual void AddOrder(CargoOrderData order)
        {
            if (!Contains(order))
                _orders.Add(order);
        }

        /// <summary>
        ///     Adds a order to the database.
        /// </summary>
        /// <param name="order">The order to be added.</param>
        public virtual void AddOrder(string requester, string reason, CargoProductPrototype product, int amount, string payingAccount, int payingAccountId, bool approved)
        {
            var order = new CargoOrderData(_orderNumber, requester, reason, product, amount, payingAccount, payingAccountId, approved);
            _orderNumber += 1;
            if (!Contains(order))
                _orders.Add(order);
        }

        /// <summary>
        ///     Removes a order from the database.
        /// </summary>
        /// <param name="order">The order to be removed.</param>
        /// <returns>Whether it could be removed or not</returns>
        public virtual bool RemoveOrder(CargoOrderData order)
        {
            return _orders.Remove(order);
        }

        /// <summary>
        ///     Returns whether the database contains the order or not.
        /// </summary>
        /// <param name="order">The order to check</param>
        /// <returns>Whether the database contained the order or not.</returns>
        public virtual bool Contains(CargoOrderData order)
        {
            return _orders.Contains(order);
        }

        /// <summary>
        ///     Returns a list with the IDs of all order.
        /// </summary>
        /// <returns>A list of order IDs</returns>
        public List<CargoOrderData> GetOrderIdList()
        {
            var list = new List<CargoOrderData>();

            foreach (var order in _orders)
            {
                list.Add(order);
            }

            return list;
        }

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            if (serializer.Reading)
            {
                var orders = serializer.ReadDataField("orders", new List<CargoOrderData>());
                foreach (var order in orders)
                {
                    _orders.Add(order);
                }
            }
            else if (serializer.Writing)
            {
                var orders = GetOrderIdList();
                serializer.DataField(ref orders, "orders", new List<CargoOrderData>());
            }
        }
    }

    [NetSerializable, Serializable]
    public class CargoOrderDatabaseState : ComponentState
    {
        public readonly List<CargoOrderData> Orders;
        public CargoOrderDatabaseState(List<CargoOrderData> orders) : base(ContentNetIDs.CARGO_ORDER_DATABASE)
        {
            Orders = orders;
        }
    }
}
