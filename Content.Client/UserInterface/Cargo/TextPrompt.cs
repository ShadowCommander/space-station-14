using Robust.Client.GameObjects.Components.UserInterface;
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
    class TextPrompt : SS14Window
    {
#pragma warning disable 649
        [Dependency] private readonly ILocalizationManager _loc;
#pragma warning restore 649

        protected override Vector2? CustomSize => (400, 600);

        public BoundUserInterface Owner { get; set; }

        public LineEdit TextInput { get; set; }
        public Button Confirm { get; set; }

        public TextPrompt()
        {
            IoCManager.InjectDependencies(this);

            Title = _loc.GetString("Input Text");

            TextInput = new LineEdit();
            Confirm = new Button()
            {
                Text = _loc.GetString("OK"),
                TextAlign = Button.AlignMode.Center
            };

            Contents.AddChild(TextInput);
            Contents.AddChild(Confirm);
        }
    }
}
