using System;

namespace Content.Server.Cargo
{
    public interface ICargoBankAccount
    {
        int ID { get; }
        string Name { get; }
        int Balance { get; }
    }
}
