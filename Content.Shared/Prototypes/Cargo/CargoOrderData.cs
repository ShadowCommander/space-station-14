using Robust.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.Prototypes.Cargo
{
    [NetSerializable, Serializable]
    public class CargoOrderData
    {
        public int OrderNumber;
        public string Requester;
        // public String RequesterRank;
        // public int RequesterId;
        public string Reason;
        public CargoProductPrototype Product;
        public int Amount;
        public string PayingAccount;
        public int PayingAccountId;
        public bool Approved;

        public CargoOrderData(int orderNumber, string requester, string reason, CargoProductPrototype product, int amount, string payingAccount, int payingAccountId, bool approved)
        {
            OrderNumber = orderNumber;
            Requester = requester;
            Reason = reason;
            Product = product;
            Amount = amount;
            PayingAccount = payingAccount;
            PayingAccountId = payingAccountId;
            Approved = approved;
        }
    }
}
