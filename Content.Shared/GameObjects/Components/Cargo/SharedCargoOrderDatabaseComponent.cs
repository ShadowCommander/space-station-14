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

        public List<CargoOrderData> _orders { get; } = new List<CargoOrderData>();

        private int _orderNumber = 0;

        /// <summary>
        ///     A read-only list of cargo orders.
        /// </summary>
        public IReadOnlyList<CargoOrderData> Orders => _orders;

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
        ///     Adds an order to the database.
        /// </summary>
        /// <param name="order">The order to be added.</param>
        public virtual void AddOrder(CargoOrderData order)
        {
            if (!Contains(order))
                _orders.Add(order);
        }

        /// <summary>
        ///     Adds an order to the database.
        /// </summary>
        /// <param name="requester">The person who requested the item.</param>
        /// <param name="reason">The reason the product was requested.</param>
        /// <param name="productId">The ID of the product requested.</param>
        /// <param name="amount">The amount of the products requested.</param>
        /// <param name="payingAccount">The name of the bank account paying for the order.</param>
        /// <param name="payingAccountId">The ID of the bank account paying for the order.</param>
        /// <param name="approved">Whether the order will be bought when the orders are processed.</param>
        public virtual void AddOrder(string requester, string reason, string productId, int amount, int payingAccountId, bool approved)
        {
            var order = new CargoOrderData(_orderNumber, requester, reason, productId, amount, payingAccountId, approved);
            _orderNumber += 1;
            if (!Contains(order))
                _orders.Add(order);
        }

        /// <summary>
        ///     Removes an order from the database.
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
        ///     Returns a list with the IDs of all orders.
        /// </summary>
        /// <returns>A list of order IDs</returns>
        public List<CargoOrderData> GetOrderList()
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
                var orders = GetOrderList();
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
