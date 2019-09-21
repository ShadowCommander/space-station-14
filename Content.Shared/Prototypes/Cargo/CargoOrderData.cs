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
        public string ProductId;
        public int Amount;
        public int PayingAccountId;
        public bool Approved;

        public CargoOrderData(int orderNumber, string requester, string reason, string productId, int amount, int payingAccountId, bool approved)
        {
            OrderNumber = orderNumber;
            Requester = requester;
            Reason = reason;
            ProductId = productId;
            Amount = amount;
            PayingAccountId = payingAccountId;
            Approved = approved;
        }
    }
}
