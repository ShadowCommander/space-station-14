using Content.Client.UserInterface.Cargo;
using Content.Shared.GameObjects.Components.Cargo;
using Content.Shared.Prototypes.Cargo;
using Robust.Client.GameObjects.Components.UserInterface;
using Robust.Shared.GameObjects.Components.UserInterface;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.ViewVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.GameObjects.Components.Cargo
{
    public class CargoConsoleBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private CargoConsoleMenu _menu;
        [ViewVariables]
        private CargoConsoleOrderMenu _orderMenu;
        [ViewVariables]
        private CargoConsoleMenu _amountMenu;

        [ViewVariables]
        public GalacticMarketComponent Market { get; private set; }
        [ViewVariables]
        public CargoOrderDatabaseComponent Orders { get; private set; }
        // TODO add GalacticBank account

        private CargoProductPrototype _product;

        public CargoConsoleBoundUserInterface(ClientUserInterfaceComponent owner, object uiKey) : base(owner, uiKey)
        {
            SendMessage(new SharedCargoConsoleComponent.CargoConsoleSyncMessage());
        }

        protected override void Open()
        {
            base.Open();

            if (!Owner.Owner.TryGetComponent(out GalacticMarketComponent market)
            ||  !Owner.Owner.TryGetComponent(out CargoOrderDatabaseComponent orders)) return;

            Market = market;
            Orders = orders;

            _menu = new CargoConsoleMenu(this);
            _orderMenu = new CargoConsoleOrderMenu();
            //amountMenu = new CargoConsoleConfirmationMenu { Owner = this };

            _menu.OnClose += Close;

            _menu.Populate();

            Market.OnDatabaseUpdated += _menu.PopulateMarket;
            Orders.OnDatabaseUpdated += _menu.PopulateOrders;

            _menu.Products.OnItemSelected += (args) => {
                _product = _menu.ProductPrototypes[args.ItemIndex];
                _orderMenu.OpenCenteredMinSize();
            };
            _orderMenu.SubmitButton.OnPressed += (args) =>
            {
                AddOrder();
                _orderMenu.Close();
            };

            _menu.OpenCentered();

        }

        protected override void ReceiveMessage(BoundUserInterfaceMessage message)
        {
            switch (message)
            {
                case SharedCargoConsoleComponent.CargoConsoleOrderDataMessage msg:
                {
                    Orders.Clear();
                    foreach(var order in msg.Orders)
                    {
                        Orders.AddOrder(order);
                    }
                    _menu.PopulateOrders();
                    break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing) return;
            _menu?.Dispose();
            _orderMenu?.Dispose();
            _amountMenu?.Dispose();
        }

        internal void AddOrder()
        {
            SendMessage(new SharedCargoConsoleComponent.CargoConsoleAddOrderMessage(_orderMenu.Requester.Text, _orderMenu.Reason.Text, _product.ID, _orderMenu.Amount.Value));
        }
    }
}
