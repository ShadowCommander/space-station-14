using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Content.Server.Database;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;

namespace Content.Server.Administration;

public class RoleBanSystem : EntitySystem
{
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IPlayerLocator _playerLocator = default!;

    private readonly Dictionary<LocatedPlayerData, Dictionary<string, ServerJobBanDef>> _cachedJobBans = new();

    public override void Initialize()
    {
        base.Initialize();

        _playerManager.PlayerStatusChanged += OnPlayerStatusChanged;
    }

    public override void Shutdown()
    {
        base.Shutdown();

        _cachedJobBans.Clear();
    }

    private async void OnPlayerStatusChanged(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus != SessionStatus.Connected)
            return;

        var playerData = await _playerLocator.LookupIdAsync(e.Session.UserId);
        if (playerData == null || _cachedJobBans.ContainsKey(playerData))
            return;

        var jobBans = await _db.GetServerJobBansAsync(playerData.LastAddress, playerData.UserId, playerData.LastHWId);
        if (jobBans.Count == 0)
            return;

        var userJobBans = new Dictionary<string, ServerJobBanDef>();
        foreach (var ban in jobBans)
        {
            if (userJobBans.ContainsKey(ban.Role))
                continue;
            userJobBans.Add(ban.Role, ban);
        }
        _cachedJobBans.Add(playerData, userJobBans);
    }

    public async Task AddJobBan(NetUserId userId, string jobId)
    {
        var banDef = new ServerJobBanDef(
            null,
            userId,
            null,
            null,
            DateTimeOffset.Now,
            null,
            "",
            userId,
            null,
            jobId);

        await AddJobBan(banDef);
    }

    public async Task<bool> AddJobBan(ServerJobBanDef banDef)
    {
        if (banDef.UserId != null)
        {
            var playerData = await _playerLocator.LookupIdAsync(banDef.UserId.Value);
            if (playerData == null)
                return false;
            if (!_cachedJobBans.TryGetValue(playerData, out var jobBans))
            {
                jobBans = new Dictionary<string, ServerJobBanDef>();
                _cachedJobBans.Add(playerData, jobBans);
            }

            if (jobBans.ContainsKey(banDef.Role))
                return false;
            jobBans.Add(banDef.Role, banDef);
        }

        await _db.AddServerJobBanAsync(banDef);
        return true;
    }

    public bool IsBanned(NetUserId userId, string jobId)
    {
        return _cachedJobBans.TryGetValue(playerData, out var jobBans) && jobBans.ContainsKey(jobId);
    }
}
