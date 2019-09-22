using Content.Server.GameObjects.EntitySystems;
using Content.Shared.GameObjects.Components.Cargo;
using Content.Shared.Prototypes.Cargo;
using Robust.Server.GameObjects.Components.UserInterface;
using Robust.Server.Interfaces.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.ViewVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Server.GameObjects.Components.Cargo
{
    [RegisterComponent]
    [ComponentReference(typeof(IActivate))]
    public class CargoConsoleComponent : SharedCargoConsoleComponent, IActivate
    {
        [ViewVariables]
        public int Points = 1000;

        private BoundUserInterface _userInterface;

        [ViewVariables]
        public GalacticMarketComponent Market { get; private set; }
        [ViewVariables]
        public CargoOrderDatabaseComponent Orders { get; private set; }
        [ViewVariables]
        public string BankName { get; private set; }
        [ViewVariables]
        public int BankId { get; private set; }

        private bool _requestOnly = false;

        public override void Initialize()
        {
            base.Initialize();
            Market = Owner.GetComponent<GalacticMarketComponent>();
            Orders = Owner.GetComponent<CargoOrderDatabaseComponent>();
            _userInterface = Owner.GetComponent<ServerUserInterfaceComponent>().GetBoundUserInterface(CargoConsoleUiKey.Key);
            _userInterface.OnReceiveMessage += UserInterfaceOnOnReceiveMessage;
            BankId = 0;
        }

        private void UserInterfaceOnOnReceiveMessage(ServerBoundUserInterfaceMessage serverMsg)
        {
            var message = serverMsg.Message;
            switch (message)
            {
                case CargoConsoleAddOrderMessage msg:
                {
                    _prototypeManager.TryIndex(msg.ProductId, out CargoProductPrototype product);
                    if (product == null)
                    {
                        break;
                    }

                    Orders.AddOrder(msg.Requester, msg.Reason, msg.ProductId, msg.Amount, BankId, !_requestOnly);
                    _userInterface.SendMessage(new CargoConsoleOrderDataMessage(Orders.GetOrderList()));
                    break;
                }
                case CargoConsoleSyncMessage msg:
                {
                    //_userInterface.SendMessage(new CargoConsoleFullDataMessage(Orders.GetOrderList(), Market.GetProductIdList()));
                    break;
                }
            }
        }

        void IActivate.Activate(ActivateEventArgs eventArgs)
        {
            if (!eventArgs.User.TryGetComponent(out IActorComponent actor))
            {
                return;
            }

            _userInterface.Open(actor.playerSession);
        }
    }
}
