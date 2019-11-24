using System;
using System.Threading;
using Content.Server.GameObjects;
using Content.Server.Interfaces.Chat;
using Content.Server.Interfaces.GameTicking;
using Content.Server.Mobs.Roles;
using Content.Server.Players;
using Robust.Server.Interfaces.Player;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Timer = Robust.Shared.Timers.Timer;

namespace Content.Server.GameTicking.GameRules
{
    /// <summary>
    ///     Simple GameRule that will do a free-for-all death match.
    ///     Kill everybody else to win.
    /// </summary>
    public sealed class RuleTroubleInTerroristTown : GameRule, IEntityEventSubscriber
    {
        private static readonly TimeSpan DeadCheckDelay = TimeSpan.FromSeconds(5);

#pragma warning disable 649
        [Dependency] private readonly IPlayerManager _playerManager;
        [Dependency] private readonly IEntityManager _entityManager;
        [Dependency] private readonly IChatManager _chatManager;
        [Dependency] private readonly IGameTicker _gameTicker;
        [Dependency] private readonly ILocalizationManager _loc;
#pragma warning restore 649

        private CancellationTokenSource _checkTimerCancel;
        private int _traitorsAlive;
        private int _crewAlive;

        public override void Added()
        {
            _chatManager.DispatchServerAnnouncement(_loc.GetString("The game is now Trouble in Terrorist Town. Kill or survive to win!"));

            _entityManager.SubscribeEvent<MobDamageStateChangedMessage>(_onMobDamageStateChanged, this);
            _playerManager.PlayerStatusChanged += PlayerManagerOnPlayerStatusChanged;
            _runDelayedCheck();
        }

        public override void Removed()
        {
            base.Removed();

            _entityManager.UnsubscribeEvent<MobDamageStateChangedMessage>(this);
            _playerManager.PlayerStatusChanged -= PlayerManagerOnPlayerStatusChanged;
        }

        private void _onMobDamageStateChanged(object sender, MobDamageStateChangedMessage message)
        {
            _runDelayedCheck();
        }

        private void _checkForWinner()
        {
            _checkTimerCancel = null;
            _traitorsAlive = 0;
            _crewAlive = 0;

            foreach (var playerSession in _playerManager.GetAllPlayers())
            {
                if (playerSession.AttachedEntity == null
                    || !playerSession.AttachedEntity.TryGetComponent(out SpeciesComponent species))
                {
                    continue;
                }

                if (!species.CurrentDamageState.IsConscious)
                {
                    continue;
                }

                if (!_playerManager.TryGetPlayerData(playerSession.SessionId, out var playerData) ||
                    !(playerData.ContentDataUncast is PlayerData data))
                {
                    continue;
                }

                if (data.Mind.HasRoleType(typeof(Traitor)))
                {
                    _traitorsAlive += 1;
                }
                else
                {
                    _crewAlive += 1;
                }
            }

            if (_crewAlive == 0)
            {
                if (_traitorsAlive == 0)
                {
                    _chatManager.DispatchServerAnnouncement(_loc.GetString("Everybody is dead! The traitors have accomplished their goal!"));
                }
                else
                {
                    _chatManager.DispatchServerAnnouncement(_loc.GetString("The crew is dead! The traitors have accomplished their goal!"));
                }
            }
            else if (_traitorsAlive == 0)
            {
                _chatManager.DispatchServerAnnouncement(_loc.GetString("The traitors are dead! The crew have survived!"));
            }
            else
            {
                // Crew and traitors are still alive. Keep playing.
                return;
            }
/*            
            _chatManager.DispatchServerAnnouncement(_loc.GetString("The traitors were:"));
            foreach (var playerSession in _playerManager.GetAllPlayers())
            {
                if (playerSession.AttachedEntity == null
                    || !playerSession.AttachedEntity.TryGetComponent(out SpeciesComponent species))
                {
                    continue;
                }

                
            }
            _chatManager.DispatchServerAnnouncement(_loc.GetString("The crew members were:"));
*/
            _chatManager.DispatchServerAnnouncement(_loc.GetString("Restarting in 10 seconds."));
            Timer.Spawn(TimeSpan.FromSeconds(10), () => _gameTicker.RestartRound());
        }

        private void PlayerManagerOnPlayerStatusChanged(object sender, SessionStatusEventArgs e)
        {
            if (e.NewStatus == SessionStatus.Disconnected)
            {
                _runDelayedCheck();
            }
        }

        private void _runDelayedCheck()
        {
            _checkTimerCancel?.Cancel();
            _checkTimerCancel = new CancellationTokenSource();

            Timer.Spawn(DeadCheckDelay, _checkForWinner, _checkTimerCancel.Token);
        }
    }
}
