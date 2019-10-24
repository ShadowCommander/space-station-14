using System.Collections.Generic;
using Content.Server.GameObjects.Components.Cargo;
using Content.Shared.Prototypes.Cargo;

namespace Content.Server.Cargo
{
    public interface ICargoOrderDataManager
    {
        void AddComponent(CargoOrderDatabaseComponent component);
        void AddOrder(int id, CargoOrderData order);
        void AddOrder(int id, string requester, string reason, string productId, int amount, int payingAccountId, bool approved);
        CargoOrderDatabase GetAccount(int id);
        IEnumerable<CargoOrderDatabase> GetAllAccounts();
        bool TryGetAccount(int id, out CargoOrderDatabase account);
        List<CargoOrderData> GetOrdersFromAccount(int accountId);
    }
}
