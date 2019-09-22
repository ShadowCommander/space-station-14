using Content.Shared.GameObjects.Components.Cargo;
using Robust.Shared.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.GameObjects.Components.Cargo
{
    class GalacticBankComponent : SharedGalacticBankComponent
    {
        /// <summary>
        ///     Event called when the database is updated.
        /// </summary>
        public event Action OnDatabaseUpdated;

        public override void HandleComponentState(ComponentState curState, ComponentState nextState)
        {
            base.HandleComponentState(curState, nextState);
            if (!(curState is GalacticBankState state))
                return;
            _accounts.Clear();
            foreach (var order in state.Accounts)
            {
                _accounts.Add(order);
            }

            OnDatabaseUpdated?.Invoke();
        }
    }
}
