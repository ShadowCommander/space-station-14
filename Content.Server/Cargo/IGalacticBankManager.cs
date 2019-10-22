using Robust.Shared.Timing;
using System.Collections.Generic;

namespace Content.Server.Cargo
{
    public interface IGalacticBankManager
    {
        IEnumerable<CargoBankAccount> GetAllBankAccounts();

        void Shutdown();
        void Update(FrameEventArgs frameEventArgs);

        CargoBankAccount CreateBankAccount(int id, string name, int balance);
    }
}
