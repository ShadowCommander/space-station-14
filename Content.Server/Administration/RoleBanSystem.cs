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

        var roleBans = await _db.GetServerRoleBansAsync(playerData.LastAddress, playerData.UserId, playerData.LastHWId);
        if (roleBans.Count == 0)
            return;

        var userRoleBans = new HashSet<string>();
        foreach (var ban in roleBans)
        {
            if (userRoleBans.Contains(ban.Role))
                continue;
            userRoleBans.Add(ban.Role);
        }
        _cachedRoleBans.Add(playerData.UserId, userRoleBans);
    }

    public async Task<bool> AddRoleBan(ServerRoleBanDef banDef, LocatedPlayerData? playerData = null)
    {
        if (banDef.UserId != null)
        {
            if (!_cachedRoleBans.TryGetValue(banDef.UserId.Value, out var roleBans))
            {
                roleBans = new HashSet<string>();
                _cachedRoleBans.Add(banDef.UserId.Value, roleBans);
            }
            if (!roleBans.Contains(banDef.Role))
                roleBans.Add(banDef.Role);
        }

        await _db.AddServerRoleBanAsync(banDef);
        return true;
    }

    public bool IsBanned(NetUserId userId, string roleId)
    {
        return _cachedRoleBans.TryGetValue(userId, out var roleBans) && roleBans.Contains(roleId);
    }

    public HashSet<string>? GetRoleBans(NetUserId playerUserId)
    {
        return _cachedRoleBans.TryGetValue(playerUserId, out var roleBans) ? roleBans : null;
    }
}
