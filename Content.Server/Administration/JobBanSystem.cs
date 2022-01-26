using System.Collections.Generic;
using Content.Server.Database;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;

namespace Content.Server.Administration;

public class JobBanSystem : EntitySystem
{
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    private Dictionary<NetUserId, List<string>?> _cachedJobBans = new();

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

        var userId = e.Session.UserId;

        if (_cachedJobBans.ContainsKey(userId))
            return;

        var jobBans = await _db.GetPlayerJobBansAsync(userId);
        if (jobBans.Count == 0)
            return;

        var userJobBans = new List<string>();
        foreach (var jobBan in jobBans)
        {
            userJobBans.Add(jobBan.Job);
        }
        _cachedJobBans.Add(userId, userJobBans);
    }

    public void AddJobBan(NetUserId userId, string jobId)
    {
        if (!_cachedJobBans.TryGetValue(userId, out var jobBans) || jobBans == null)
        {
            jobBans = new List<string>();
            _cachedJobBans.Add(userId, jobBans);
        }

        if (jobBans.Contains(jobId))
            return;
        jobBans.Add(jobId);

        _db.AddPlayerJobBanAsync(userId, jobId);
    }

    public bool IsBanned(NetUserId userId, string jobId)
    {
        return _cachedJobBans.TryGetValue(userId, out var jobBans) && jobBans != null && jobBans.Contains(jobId);
    }
}
