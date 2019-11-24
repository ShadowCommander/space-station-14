using Content.Server.GameTicking.GameRules;
using Content.Server.Interfaces.GameTicking;
using Robust.Shared.IoC;
using Robust.Shared.Localization;

namespace Content.Server.GameTicking.GamePresets
{
    public sealed class PresetTroubleInTerroristTown : GamePreset
    {
#pragma warning disable 649
        [Dependency] private readonly IGameTicker _gameTicker;
        [Dependency] private readonly ILocalizationManager _loc;
#pragma warning restore 649

        public override void Start()
        {
            _gameTicker.AddGameRule<RuleTroubleInTerroristTown>();
        }

        public override string Description => _loc.GetString("Trouble in Terrorist Town. Kill or survive to win!");
    }
}
