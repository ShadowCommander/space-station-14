using Robust.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.DatabaseData
{
    [NetSerializable, Serializable]
    public class GalacticBankAccount
    {
        private string _id;
        private string _name;
        private int _balance;

        public GalacticBankAccount(string id, string name, int balance)
        {
            _id = id;
            _name = name;
            _balance = balance;
        }
    }
}
