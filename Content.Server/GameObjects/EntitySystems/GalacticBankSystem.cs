using Content.Server.GameObjects.Components.Cargo;
using Content.Shared.DatabaseData;
using Robust.Shared.GameObjects;
using Robust.Shared.GameObjects.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Server.GameObjects.EntitySystems
{
    public class GalacticBankSystem : EntitySystem
    {
        private float _timer = 0f;
        private int _idIndex = 0;
        private readonly List<GalacticBankAccount> _accounts = new List<GalacticBankAccount>();

        public IReadOnlyList<GalacticBankAccount> Accounts => _accounts;

        public override void Initialize()
        {
            EntityQuery = new TypeEntityQuery(typeof(GalacticBankComponent));
        }

        public override void Update(float frameTime)
        {
            _timer += frameTime;
            if (_timer > 1f)
                return;
            _timer -= 1f;
            foreach (var account in _accounts)
            {
                account.Balance += 1;
            }
        }

        public bool CreateAccount(string name)
        {
            var account = new GalacticBankAccount(_idIndex, name);
            if (_accounts.Contains(account))
                return false;
            _accounts.Add(account);
            _idIndex += 1;
            return true;
        }

        public bool RegisterAccount(GalacticBankAccount account)
        {
            if (_accounts.Contains(account))
                return false;
            _accounts.Add(account);
            return true;
        }

        public void UnregisterAccount(GalacticBankAccount account)
        {
            _accounts.Remove(account);
        }

        public GalacticBankAccount GetAccountById(int id)
        {
            foreach (var account in _accounts)
            {
                if (account.Id == id)
                {
                    return account;
                }
            }

            return null;
        }

        public string[] GetServerNames()
        {
            var list = new string[_accounts.Count];

            for (var i = 0; i < _accounts.Count; i++)
            {
                list[i] = _accounts[i].Name;
            }

            return list;
        }

        public int[] GetServerIds()
        {
            var list = new int[_accounts.Count];

            for (var i = 0; i < _accounts.Count; i++)
            {
                list[i] = _accounts[i].Id;
            }

            return list;
        }
    }
}
