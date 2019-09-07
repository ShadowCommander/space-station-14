using Content.Client.GameObjects.Components.Cargo;
using Content.Shared.Prototypes.Cargo;
using Robust.Client.Graphics.Drawing;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.Utility;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Maths;
using System.Collections.Generic;

namespace Content.Client.UserInterface.Cargo
{
    public class CargoConsoleMenu : SS14Window
    {
#pragma warning disable 649
        [Dependency] private readonly ILocalizationManager _loc;
#pragma warning restore 649

        protected override Vector2? CustomSize => (400, 600);

        public CargoConsoleBoundUserInterface Owner { get; set; }

        private List<CargoProductPrototype> _productPrototypes = new List<CargoProductPrototype>();
        private List<CargoOrderData> _requestData = new List<CargoOrderData>();
        private List<CargoOrderData> _orderData = new List<CargoOrderData>();
        private List<string> _categoryStrings = new List<string>();

        private Label _accountNameLabel { get; set; }
        private Label _pointsLabel { get; set; }
        private Label _shuttleStatusLabel { get; set; }
        private ItemList _requests { get; set; }
        private ItemList _orders { get; set; }
        private OptionButton _categories { get; set; }
        private LineEdit _searchBar { get; set; }

        public ItemList Products { get; set; }
        public Button CallShuttleButton { get; set; }
        public Button PermissionsButton { get; set; }

        public CargoConsoleMenu(CargoConsoleBoundUserInterface owner)
        {
            IoCManager.InjectDependencies(this);
            Owner = owner;

            Title = _loc.GetString("Cargo Console");

            var rows = new VBoxContainer
            {
                MarginTop = 0
            };

            var accountName = new HBoxContainer()
            {
                MarginTop = 0
            };
            var accountNameLabel = new Label {
                Text = _loc.GetString("Account Name: "),
                StyleClasses = { NanoStyle.StyleClassLabelKeyText }
            };
            _accountNameLabel = new Label {
                Text = _loc.GetString("None")
            };
            accountName.AddChild(accountNameLabel);
            accountName.AddChild(_accountNameLabel);
            rows.AddChild(accountName);

            var points = new HBoxContainer();
            var pointsLabel = new Label
            {
                Text = _loc.GetString("Points: "),
                StyleClasses = { NanoStyle.StyleClassLabelKeyText }
            };
            _pointsLabel = new Label
            {
                Text = "0"
            };
            points.AddChild(pointsLabel);
            points.AddChild(_pointsLabel);
            rows.AddChild(points);

            var shuttleStatus = new HBoxContainer();
            var shuttleStatusLabel = new Label
            {
                Text = _loc.GetString("Shuttle Status: "),
                StyleClasses = { NanoStyle.StyleClassLabelKeyText }
            };
            _shuttleStatusLabel = new Label
            {
                Text = _loc.GetString("Away")
            };
            shuttleStatus.AddChild(shuttleStatusLabel);
            shuttleStatus.AddChild(_shuttleStatusLabel);
            rows.AddChild(shuttleStatus);

            var buttons = new HBoxContainer();
            CallShuttleButton = new Button()
            {
                Text = _loc.GetString("Call Shuttle"),
                TextAlign = Button.AlignMode.Center,
                SizeFlagsHorizontal = SizeFlags.FillExpand
            };
            PermissionsButton = new Button()
            {
                Text = _loc.GetString("Permissions"),
                TextAlign = Button.AlignMode.Center
            };
            buttons.AddChild(CallShuttleButton);
            buttons.AddChild(PermissionsButton);
            rows.AddChild(buttons);

            var category = new HBoxContainer();
            _categories = new OptionButton
            {
                SizeFlagsHorizontal = SizeFlags.FillExpand,
                SizeFlagsStretchRatio = 1
            };
            _searchBar = new LineEdit
            {
                PlaceHolder = _loc.GetString("Search"),
                SizeFlagsHorizontal = SizeFlags.FillExpand,
                SizeFlagsStretchRatio = 1
            };
            category.AddChild(_categories);
            category.AddChild(_searchBar);
            rows.AddChild(category);

            Products = new ItemList()
            {
                SizeFlagsVertical = SizeFlags.FillExpand,
                SizeFlagsStretchRatio = 6
            };
            rows.AddChild(Products);

            var requestsAndOrders = new PanelContainer
            {
                SizeFlagsVertical = SizeFlags.FillExpand,
                SizeFlagsStretchRatio = 6,
                PanelOverride = new StyleBoxFlat { BackgroundColor = Color.Black }
            };
            var rAndOVBox = new VBoxContainer();
            var requestsLabel = new Label { Text = _loc.GetString("Requests") };
            _requests = new ItemList
            {
                StyleClasses = { "transparentItemList" },
                SizeFlagsVertical = SizeFlags.FillExpand,
                SizeFlagsStretchRatio = 1,
            };
            var ordersLabel = new Label { Text = _loc.GetString("Orders") };
            _orders = new ItemList
            {
                StyleClasses = { "transparentItemList" },
                SizeFlagsVertical = SizeFlags.FillExpand,
                SizeFlagsStretchRatio = 1,
            };
            rAndOVBox.AddChild(requestsLabel);
            rAndOVBox.AddChild(_requests);
            rAndOVBox.AddChild(ordersLabel);
            rAndOVBox.AddChild(_orders);
            requestsAndOrders.AddChild(rAndOVBox);
            rows.AddChild(requestsAndOrders);

            Contents.AddChild(rows);

            /*categories.AddItem("Honk");
            categories.AddItem("Foo");
            categories.AddItem("Bar");
            categories.AddItem("Baz");

            for (var i = 0; i < categoriesList.Length; i++)
            {
                categories.AddItem(categoriesList[i], i);
            }*/
            CallShuttleButton.OnPressed += OnCallShuttleButtonPressed;
            _searchBar.OnTextChanged += OnSearchBarTextChanged;
            _categories.OnItemSelected += OnOverrideMenuItemSelected;
            Products.OnItemSelected += OnItemsItemSelected;
            Populate();
        }

        private void OnCallShuttleButtonPressed(BaseButton.ButtonEventArgs args)
        {
        }

        private void OnOverrideMenuItemSelected(OptionButton.ItemSelectedEventArgs args)
        {
        }

        private void OnSearchBarTextChanged(LineEdit.LineEditEventArgs args)
        {
            PopulateMarket();
        }

        private void OnItemsItemSelected(ItemList.ItemListSelectedEventArgs args)
        {
            
            Owner.AddOrder(_productPrototypes[args.ItemIndex]);
        }

        /// <summary>
        ///     Populates the list of products that will actually be shown, using the current filters.
        /// </summary>
        public void PopulateMarket()
        {
            _productPrototypes.Clear();
            _categoryStrings.Clear();

            Products.Clear();
            _categories.Clear();

            var search = _searchBar.Text.Trim().ToLowerInvariant();
            foreach (var prototype in Owner.Market)
            {
                if (search.Length == 0 || prototype.Name.ToLowerInvariant().Contains(search))
                {
                    _productPrototypes.Add(prototype);
                    Products.AddItem(prototype.Name, prototype.Icon.Frame0());
                    if (!_categoryStrings.Contains(prototype.Category))
                    {
                        _categoryStrings.Add(prototype.Category);
                    }
                }
            }
            _categoryStrings.Sort();
            foreach (var str in _categoryStrings)
            {
                _categories.AddItem(str);
            }
        }

        /// <summary>
        ///     Populates the list of orders and requests.
        /// </summary>
        public void PopulateOrders()
        {
            _requestData.Clear();
            _orderData.Clear();

            _requests.Clear();
            _orders.Clear();

            foreach (var order in Owner.Orders)
            {
                if (order.Approved)
                {
                    _orderData.Add(order);
                    _orders.AddItem($"{order.Product.Name} (x{order.Amount}) by {order.Requester}\nReason: {order.Reason}", order.Product.Icon.Frame0());
                }
                else
                {
                    _requestData.Add(order);
                    _requests.AddItem($"{order.Product.Name} (x{order.Amount}) by {order.Requester}\nReason: {order.Reason}", order.Product.Icon.Frame0());
                }
            }
        }

        public void Populate()
        {
            PopulateMarket();
            PopulateOrders();
        }
    }
}
