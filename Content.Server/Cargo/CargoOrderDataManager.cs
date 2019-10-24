﻿using Content.Server.GameObjects.Components.Cargo;
using Content.Shared.Prototypes.Cargo;
using System.Collections.Generic;

namespace Content.Server.Cargo
{
    public class CargoOrderDataManager : ICargoOrderDataManager
    {
        private readonly Dictionary<int, CargoOrderDatabase> _accounts = new Dictionary<int, CargoOrderDatabase>();
        private readonly List<CargoOrderDatabaseComponent> _components = new List<CargoOrderDatabaseComponent>();

        public CargoOrderDataManager()
        {
        }

        public IEnumerable<CargoOrderDatabase> GetAllAccounts()
        {
            return _accounts.Values;
        }

        public CargoOrderDatabase GetAccount(int id)
        {
            return _accounts[id];
        }

        public bool TryGetAccount(int id, out CargoOrderDatabase account)
        {
            if (_accounts.TryGetValue(id, out var _account))
            {
                account = _account;
                return true;
            }
            account = null;
            return false;
        }

        /// <summary>
        ///     Adds an order to the database.
        /// </summary>
        /// <param name="order">The order to be added.</param>
        public virtual void AddOrder(int id, CargoOrderData order)
        {
            if (!TryGetAccount(id, out var account))
                return;
            account.AddOrder(order);
        }

        /// <summary>
        ///     Adds an order to the database.
        /// </summary>
        /// <param name="requester">The person who requested the item.</param>
        /// <param name="reason">The reason the product was requested.</param>
        /// <param name="productId">The ID of the product requested.</param>
        /// <param name="amount">The amount of the products requested.</param>
        /// <param name="payingAccountId">The ID of the bank account paying for the order.</param>
        /// <param name="approved">Whether the order will be bought when the orders are processed.</param>
        public virtual void AddOrder(int id, string requester, string reason, string productId, int amount, int payingAccountId, bool approved)
        {
            if (!TryGetAccount(id, out var account))
                return;
            account.AddOrder(requester, reason, productId, amount, payingAccountId, approved);
        }

        public void AddComponent(CargoOrderDatabaseComponent component)
        {
            if (_components.Contains(component))
                return;
            _components.Add(component);
        }

        public List<CargoOrderData> GetOrdersFromAccount(int accountId)
        {
            if (!TryGetAccount(accountId, out var account))
                return null;
            return account.GetOrderList();
        }
    }
}
