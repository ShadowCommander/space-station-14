using Robust.Client.UserInterface.Controls;
using Robust.Shared.Localization;

namespace Content.Client.UserInterface
{
    public class AntagPreferencesGui : VBoxContainer
    {
        public VBoxContainer Preferences;

        public AntagPreferencesGui(ILocalizationManager localization)
        {

            AddChild(new VBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.FillExpand,
                Children =
                {
                    new NanoHeading
                    {
                        Text = localization.GetString("Antag Preferences"),
                    },
                    (Preferences = new VBoxContainer
                    {
                        SizeFlagsVertical = SizeFlags.FillExpand,
                    })
                }
            });
        }
    }
}
