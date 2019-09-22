using Content.Shared.DatabaseData;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Content.Shared.GameObjects.Components.Cargo
{
    public class SharedGalacticBankComponent : Component, IEnumerable<GalacticBankAccount>
    {
        public override string Name => "GalacticBank";
        public override uint? NetID => ContentNetIDs.GALACTIC_BANK;
        public override Type StateType => typeof(GalacticBankState);

        protected List<GalacticBankAccount> _accounts = new List<GalacticBankAccount>();

        public IEnumerator<GalacticBankAccount> GetEnumerator()
        {
            return _accounts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Removes all bank accounts from the database.
        /// </summary>
        public virtual void Clear()
        {
            _accounts.Clear();
        }

        /// <summary>
        ///     Adds a bank account to the database.
        /// </summary>
        /// <param name="account">The bank account to be added.</param>
        public virtual void AddAccount(GalacticBankAccount account)
        {
            if (!Contains(account))
                _accounts.Add(account);
        }

        /// <summary>
        ///     Adds a bank account to the database.
        /// </summary>
        /// <param name="id">The ID of the new account.</param>
        /// <param name="name">The name of the new account.</param>
        /// <param name="balance">The balance the new account will start with.</param>
        public virtual void AddAccount(string id, string name, int balance)
        {
            var account = new GalacticBankAccount(id, name, balance);
            if (!Contains(account))
                _accounts.Add(account);
        }

        /// <summary>
        ///     Removes a bank account from the database.
        /// </summary>
        /// <param name="account">The bank account to be removed.</param>
        /// <returns>Whether the bank account could be removed or not</returns>
        public virtual bool RemoveAccount(GalacticBankAccount account)
        {
            return _accounts.Remove(account);
        }

        /// <summary>
        ///     Returns whether the database contains the bank account or not.
        /// </summary>
        /// <param name="account">The bank account to check</param>
        /// <returns>Whether the database contains the bank account or not.</returns>
        public virtual bool Contains(GalacticBankAccount account)
        {
            return _accounts.Contains(account);
        }

        /// <summary>
        ///     Returns a list with the IDs of all bank accounts.
        /// </summary>
        /// <returns>A list of bank account IDs</returns>
        public List<GalacticBankAccount> GetAccountList()
        {
            var list = new List<GalacticBankAccount>();

            foreach (var account in _accounts)
            {
                list.Add(account);
            }

            return list;
        }

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            if (serializer.Reading)
            {
                var accounts = serializer.ReadDataField("accounts", new List<GalacticBankAccount>());
                foreach (var account in accounts)
                {
                    _accounts.Add(account);
                }
            }
            else if (serializer.Writing)
            {
                var accounts = GetAccountList();
                serializer.DataField(ref accounts, "accounts", new List<GalacticBankAccount>());
            }
        }
    }

    [NetSerializable, Serializable]
    public class GalacticBankState : ComponentState
    {
        public readonly List<GalacticBankAccount> Accounts;
        public GalacticBankState(List<GalacticBankAccount> accounts) : base(ContentNetIDs.GALACTIC_BANK)
        {
            Accounts = accounts;
        }
    }

}
