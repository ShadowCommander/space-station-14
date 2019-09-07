using Content.Client.UserInterface.Cargo;
using Content.Shared.Prototypes.Cargo;
using Robust.Client.GameObjects.Components.UserInterface;
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
        private CargoConsoleMenu menu;
        [ViewVariables]
        private TextPrompt confirmationMenu;
        [ViewVariables]
        private CargoConsoleMenu amountMenu;

        [ViewVariables]
        public GalacticMarketComponent Market;

        [ViewVariables]
        public CargoOrderDatabaseComponent Orders;

        [ViewVariables]
        private bool _requestOnly;

        public CargoConsoleBoundUserInterface(ClientUserInterfaceComponent owner, object uiKey) : base(owner, uiKey)
        {
            
        }

        protected override void Open()
        {
            base.Open();

            if (!Owner.Owner.TryGetComponent(out Market)
            ||  !Owner.Owner.TryGetComponent(out Orders)) return;


            menu = new CargoConsoleMenu(this);
            confirmationMenu = new TextPrompt { Owner = this };
            //amountMenu = new CargoConsoleConfirmationMenu { Owner = this };

            menu.OnClose += Close;
            Orders.OnDatabaseUpdated += menu.Populate;

            menu.Products.OnItemSelected += (args) => { confirmationMenu.OpenCentered(); };

            menu.OpenCentered();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing) return;
            menu?.Dispose();
            confirmationMenu?.Dispose();
            amountMenu?.Dispose();
        }

        internal void AddOrder(CargoProductPrototype cargoProductPrototype)
        {
            Orders.AddOrder("Requester", "Reason", cargoProductPrototype, 1, "Test", 1, true);
        }
    }
}
