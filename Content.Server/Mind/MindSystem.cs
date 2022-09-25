using Content.Server.Database;
using Content.Server.GameTicking;
using Content.Server.Ghost;
using Content.Server.Ghost.Components;
using Content.Server.Mind.Components;
using Content.Server.MobState;
using Content.Shared.Examine;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Server.Mind;

public sealed class MindSystem : EntitySystem
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;
    [Dependency] private readonly GhostSystem _ghostSystem = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MindComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<MindComponent, ExaminedEvent>(OnExamined);
    }

    public void SetGhostOnShutdown(EntityUid uid, bool value, MindComponent? mind = null)
    {
        if (!Resolve(uid, ref mind))
            return;

        mind.GhostOnShutdown = value;
    }

    /// <summary>
    ///     Don't call this unless you know what the hell you're doing.
    ///     Use <see cref="Mind.TransferTo(System.Nullable{Robust.Shared.GameObjects.EntityUid},bool)"/> instead.
    ///     If that doesn't cover it, make something to cover it.
    /// </summary>
    public void InternalAssignMind(EntityUid uid, Mind value, MindComponent? mind = null)
    {
        if (!Resolve(uid, ref mind))
            return;

        mind.Mind = value;
        RaiseLocalEvent(uid, new MindAddedMessage(), true);
    }

    /// <summary>
    ///     Don't call this unless you know what the hell you're doing.
    ///     Use <see cref="Mind.TransferTo(System.Nullable{Robust.Shared.GameObjects.EntityUid},bool)"/> instead.
    ///     If that doesn't cover it, make something to cover it.
    /// </summary>
    public void InternalEjectMind(EntityUid uid, MindComponent? mind = null)
    {
        if (!Resolve(uid, ref mind))
            return;

        if (!Deleted(uid))
            RaiseLocalEvent(uid, new MindRemovedMessage(), true);

        mind.Mind = null;
    }

    private void OnShutdown(EntityUid uid, MindComponent mind, ComponentShutdown args)
    {
        // Let's not create ghosts if not in the middle of the round.
        if (_gameTicker.RunLevel != GameRunLevel.InRound)
            return;

        if (mind.HasMind)
        {
            if (mind.Mind?.VisitingEntity is {Valid: true} visiting)
            {
                if (TryComp(visiting, out GhostComponent? ghost))
                {
                    _ghostSystem.SetCanReturnToBody(ghost, false);
                }

                mind.Mind!.TransferTo(visiting);
            }
            else if (mind.GhostOnShutdown)
            {
                // Changing an entities parents while deleting is VERY sus. This WILL throw exceptions.
                // TODO: just find the applicable spawn position directly without actually updating the transform's parent.
                Transform(uid).AttachToGridOrMap();
                var spawnPosition = Transform(uid).Coordinates;

                // Use a regular timer here because the entity has probably been deleted.
                Timer.Spawn(0, () =>
                {
                    // Make extra sure the round didn't end between spawning the timer and it being executed.
                    if (_gameTicker.RunLevel != GameRunLevel.InRound)
                        return;

                    // Async this so that we don't throw if the grid we're on is being deleted.
                    var gridId = spawnPosition.GetGridUid(EntityManager);
                    if (!spawnPosition.IsValid(EntityManager) || gridId == EntityUid.Invalid || !_mapManager.GridExists(gridId))
                    {
                        spawnPosition = _gameTicker.GetObserverSpawnPoint();
                    }

                    // TODO refactor observer spawning.
                    if (!spawnPosition.IsValid(EntityManager))
                    {
                        Logger.ErrorS("mind", $"Entity \"{ToPrettyString(uid)}\" for {mind.Mind?.CharacterName} was deleted, and no applicable spawn location is available.");
                        return;
                    }

                    var ghost = Spawn("MobObserver", spawnPosition);
                    var ghostComponent = Comp<GhostComponent>(ghost);
                    _ghostSystem.SetCanReturnToBody(ghostComponent, false);

                    // Log these to make sure they're not causing the GameTicker round restart bugs...
                    Logger.DebugS("mind", $"Entity \"{ToPrettyString(uid)}\" for {mind.Mind?.CharacterName} was deleted, spawned \"{ToPrettyString(ghost)}\".");

                    if (mind.Mind == null)
                        return;

                    var val = mind.Mind.CharacterName ?? string.Empty;
                    MetaData(ghost).EntityName = val;
                    mind.Mind.TransferTo(ghost);
                });
            }
        }
    }

    private void OnExamined(EntityUid uid, MindComponent mind, ExaminedEvent args)
    {
        if (!mind.ShowExamineInfo || !args.IsInDetailsRange)
        {
            return;
        }

        var dead = _mobStateSystem.IsDead(uid);

        if (dead)
        {
            if (mind.Mind?.Session == null)
            {
                // Player has no session attached and dead
                args.PushMarkup($"[color=yellow]{Loc.GetString("mind-component-no-mind-and-dead-text", ("ent", uid))}[/color]");
            }
            else
            {
                // Player is dead with session
                args.PushMarkup($"[color=red]{Loc.GetString("comp-mind-examined-dead", ("ent", uid))}[/color]");
            }
        }
        else if (!mind.HasMind)
        {
            args.PushMarkup($"[color=mediumpurple]{Loc.GetString("comp-mind-examined-catatonic", ("ent", uid))}[/color]");
        }
        else if (mind.Mind?.Session == null)
        {
            args.PushMarkup($"[color=yellow]{Loc.GetString("comp-mind-examined-ssd", ("ent", uid))}[/color]");
        }
    }

    public Mind? GetMind(EntityUid uid, MindComponent? mind = null)
    {
        if (!Resolve(uid, ref mind))
            return null;

        if (mind.HasMind)
            return mind.Mind;
        return null;
    }

    public void TransferTo(EntityUid uid, EntityUid? target, MindComponent? mind = null)
    {
        if (!Resolve(uid, ref mind))
            return;

        if (!mind.HasMind)
            return;

        mind.Mind!.TransferTo(target);
    }

    public void TransferTo(Mind mind, EntityUid? target)
    {
        mind.TransferTo(target);
    }

    public Mind CreateMind(NetUserId userId)
    {
        var mind = new Mind(userId);
        ChangeOwningPlayer(mind, userId);
        return mind;
    }

    public void ChangeOwningPlayer(Mind mind, NetUserId? netUserId)
    {
        mind.ChangeOwningPlayer(netUserId);
    }
}
