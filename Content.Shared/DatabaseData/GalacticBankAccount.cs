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
        public int Id { get;  }
        public string Name { get; }
        public int Balance { get; set; }

        public GalacticBankAccount(int id, string name)
        {
            Id = id;
            Name = name;
            Balance = 0;
        }
    }
}
