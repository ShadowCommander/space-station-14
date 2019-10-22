using System;

namespace Content.Server.Cargo
{
    public class CargoBankAccount : ICargoBankAccount
    {
        public int ID { get; }

        public string Name { get; }

        public int Balance { get; private set; }

        public CargoBankAccount(int iD, string name, int balance)
        {
            ID = iD;
            Name = name;
            Balance = balance;
        }

        internal void ChangeBalance(int points)
        {
            Balance += points;
        }
    }
}
