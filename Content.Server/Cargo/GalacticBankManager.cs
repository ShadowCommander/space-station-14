using Robust.Shared.Timing;
using System.Collections.Generic;

namespace Content.Server.Cargo
{
    public class GalacticBankManager : IGalacticBankManager
    {
        private readonly float DELAY = 10f;
        private readonly int POINT_INCREASE = 10;

        private int _index = 0;
        private float _timer = 0;
        private readonly Dictionary<int, CargoBankAccount> _accounts = new Dictionary<int, CargoBankAccount>();

        public IEnumerable<CargoBankAccount> GetAllBankAccounts()
        {
            return _accounts.Values;
        }

        public void Shutdown()
        {
            throw new System.NotImplementedException();
        }

        public CargoBankAccount CreateBankAccount(int id, string name, int balance = 0)
        {
            var account = new CargoBankAccount(_index, name, balance);
            _index += 1;
            return (account);
        }

        public void Update(FrameEventArgs frameEventArgs)
        {
            _timer += frameEventArgs.DeltaSeconds;
            if (_timer < DELAY)
                return;
            _timer -= DELAY;
            foreach (var account in GetAllBankAccounts())
            {
                account.ChangeBalance(POINT_INCREASE);
            }
            
        }
    }
}
