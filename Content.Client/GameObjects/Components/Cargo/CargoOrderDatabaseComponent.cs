using Content.Shared.GameObjects.Components.Cargo;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.GameObjects.Components.Cargo
{
    [RegisterComponent]
    public class CargoOrderDatabaseComponent : SharedCargoOrderDatabaseComponent
    {
        /// <summary>
        ///     Event called when the database is updated.
        /// </summary>
        public event Action OnDatabaseUpdated;

        public override void HandleComponentState(ComponentState curState, ComponentState nextState)
        {
            base.HandleComponentState(curState, nextState);
            if (!(curState is CargoOrderDatabaseState state))
                return;
            _orders.Clear();
            foreach (var order in state.Orders)
            {
                _orders.Add(order);
            }

            OnDatabaseUpdated?.Invoke();
        }
    }
}
