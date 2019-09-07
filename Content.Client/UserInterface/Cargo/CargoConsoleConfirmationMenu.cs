using Content.Client.GameObjects.Components.Cargo;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.UserInterface.Cargo
{
    public class CargoConsoleConfirmationMenu : SS14Window
    {
#pragma warning disable 649
        [Dependency] private readonly ILocalizationManager _loc;
#pragma warning restore 649

        protected override Vector2? CustomSize => (400, 600);

        public CargoConsoleBoundUserInterface Owner { get; set; }

        public LineEdit Reason { get; set; }
        public Button Confirm { get; set; }

        public CargoConsoleConfirmationMenu()
        {
        }
    }
}
