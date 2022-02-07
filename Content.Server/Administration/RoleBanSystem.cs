using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Content.Server.Database;
using Content.Server.Roles;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Network;

namespace Content.Server.Administration;

public class RoleBanSystem : EntitySystem
{
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IPlayerLocator _playerLocator = default!;

    private readonly Dictionary<NetUserId, HashSet<string>> _cachedRoleBans = new();

    public override void Initialize()
    {
        base.Initialize();

        _playerManager.PlayerStatusChanged += OnPlayerStatusChanged;
    }

    public override void Shutdown()
    {
        base.Shutdown();

        _cachedRoleBans.Clear();
    }

    private async void OnPlayerStatusChanged(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus != SessionStatus.Connected
            || _cachedRoleBans.ContainsKey(e.Session.UserId))
            return;

        var playerData = await _playerLocator.LookupIdAsync(e.Session.UserId);
        if (playerData == null)
        {
            Logger.Error("PlayerLocate", $"Role bans couldn't be found PlayerData for player {e.Session.Name} {e.Session.UserId.ToString()} couldn't be found.");
            return;
        }

        if (_cachedRoleBans.ContainsKey(e.Session.UserId))
            return;

        var jobBans = await _db.GetServerJobBansAsync(playerData.LastAddress, playerData.UserId, playerData.LastHWId);
        if (jobBans.Count == 0)
            return;

        var userRoleBans = new HashSet<string>();
        foreach (var ban in jobBans)
        {
            if (userRoleBans.Contains(ban.Role))
                continue;
            userRoleBans.Add(ban.Role);
        }
        _cachedRoleBans.Add(playerData.UserId, userRoleBans);
    }

    public async Task<bool> AddRoleBan(ServerJobBanDef banDef, LocatedPlayerData? playerData = null)
    {
        if (banDef.UserId != null)
        {
            if (!_cachedRoleBans.TryGetValue(banDef.UserId.Value, out var jobBans))
            {
                jobBans = new HashSet<string>();
                _cachedRoleBans.Add(banDef.UserId.Value, jobBans);
            }
            if (!jobBans.Contains(banDef.Role))
                jobBans.Add(banDef.Role);
        }

        await _db.AddServerJobBanAsync(banDef);
        return true;
    }

    public bool IsBanned(NetUserId userId, string jobId)
    {
        return _cachedRoleBans.TryGetValue(userId, out var jobBans) && jobBans.Contains(jobId);
    }

    public HashSet<string>? GetRoleBans(NetUserId playerUserId)
    {
        return _cachedRoleBans.TryGetValue(playerUserId, out var roleBans) ? roleBans : null;
    }
}
