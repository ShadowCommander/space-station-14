﻿#nullable enable
using System;
using System.Threading.Tasks;
using Content.Server.Mind;
using Content.Server.Mind.Components;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.FixedPoint;
using NUnit.Framework;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Minds;

[TestFixture]
public sealed class MindTests
{
    private const string Prototypes = @"
- type: entity
  id: MindTestEntity
  components:
  - type: Mind

- type: entity
  parent: MindTestEntity
  id: MindTestEntityDamageable
  components:
  - type: Damageable
  - type: MobState
    thresholds:
      0: Alive
      100: Critical
      200: Dead
  - type: Destructible
    thresholds:
    - trigger:
        !type:DamageTypeTrigger
        damageType: Blunt
        damage: 400
        behaviors:
        - !type:GibBehavior { }
";

    // Exception handling for PlayerData and NetUserId invalid due to testing.
    // Can be removed when Players can be mocked.
    private Mind CreateMind(NetUserId userId, MindSystem mindSystem)
    {
        Mind? mind = null;

        CatchPlayerDataException(() =>
            mindSystem.TryCreateMind(userId, out mind));

        Assert.NotNull(mind);
        return mind!;
    }

    /// <summary>
    ///     Exception handling for PlayerData and NetUserId invalid due to testing.
    ///     Can be removed when Players can be mocked.
    /// </summary>
    /// <param name="func"></param>
    private void CatchPlayerDataException(Action func)
    {
        try
        {
            func();
        }
        catch (ArgumentException e)
        {
            // Prevent exiting due to PlayerData not being initialized.
            if (e.Message == "New owner must have previously logged into the server. (Parameter 'newOwner')")
                return;
            throw;
        }
    }

    [Test]
    public async Task TestCreateAndTransferMind()
    {
        await using var pairTracker = await PoolManager.GetServerClient(new PoolSettings{ NoClient = true });
        var server = pairTracker.Pair.Server;

        var entMan = server.ResolveDependency<IServerEntityManager>();

        await server.WaitAssertion(() =>
        {
            var mindSystem = entMan.EntitySysManager.GetEntitySystem<MindSystem>();

            var entity = entMan.SpawnEntity(null, new MapCoordinates());
            var mindComp = entMan.EnsureComponent<MindComponent>(entity);
            var userId = new NetUserId(Guid.NewGuid());

            var mind = CreateMind(userId, mindSystem);

            Assert.That(mind.UserId, Is.EqualTo(userId));

            mindSystem.TransferTo(mind, entity);
            Assert.That(mindSystem.GetMind(entity, mindComp), Is.EqualTo(mind));
        });

        await pairTracker.CleanReturnAsync();
    }

    [Test]
    public async Task TestEntityDeadWhenGibbed()
    {
        await using var pairTracker = await PoolManager.GetServerClient(new PoolSettings{ NoClient = true, ExtraPrototypes = Prototypes });
        var server = pairTracker.Pair.Server;

        var entMan = server.ResolveDependency<IServerEntityManager>();
        var protoMan = server.ResolveDependency<IPrototypeManager>();

        EntityUid entity = default!;
        MindComponent mindComp = default!;
        Mind mind = default!;
        var mindSystem = entMan.EntitySysManager.GetEntitySystem<MindSystem>();
        var damageableSystem = entMan.EntitySysManager.GetEntitySystem<DamageableSystem>();

        await server.WaitAssertion(() =>
        {
            entity = entMan.SpawnEntity("MindTestEntityDamageable", new MapCoordinates());
            mindComp = entMan.EnsureComponent<MindComponent>(entity);
            var userId = new NetUserId(Guid.NewGuid());

            mind = CreateMind(userId, mindSystem);

            Assert.That(mind.UserId, Is.EqualTo(userId));

            mindSystem.TransferTo(mind, entity);
            Assert.That(mindSystem.GetMind(entity, mindComp), Is.EqualTo(mind));

        });

        await PoolManager.RunTicksSync(pairTracker.Pair, 5);

        await server.WaitAssertion(() =>
        {
            var damageable = entMan.GetComponent<DamageableComponent>(entity);
            if (!protoMan.TryIndex<DamageTypePrototype>("Blunt", out var prototype))
            {
                return;
            }

            damageableSystem.SetDamage(damageable, new DamageSpecifier(prototype, FixedPoint2.New(401)));
            Assert.That(mindSystem.GetMind(entity, mindComp), Is.EqualTo(mind));
        });

        await PoolManager.RunTicksSync(pairTracker.Pair, 5);

        await pairTracker.CleanReturnAsync();
    }

    public async Task TestGetPlayerFromEntity()
    {
        await using var pairTracker = await PoolManager.GetServerClient(new PoolSettings{ NoClient = true });
        var server = pairTracker.Pair.Server;

        var entMan = server.ResolveDependency<IServerEntityManager>();
        var playerMan = server.ResolveDependency<IPlayerManager>();

        await server.WaitAssertion(() =>
        {
            // var playerSession = new PlayerSession();

            var entity = entMan.SpawnEntity(null, new MapCoordinates());

            var mindSys = entMan.EntitySysManager.GetEntitySystem<MindSystem>();

            var mindComp = entMan.GetComponent<MindComponent>(entity);
            // mindComp.Mind?.Session;
        });

        await PoolManager.RunTicksSync(pairTracker.Pair, 5);

        await pairTracker.CleanReturnAsync();
    }

    [Test]
    public async Task TestMindTransfersToOtherEntity()
    {
        await using var pairTracker = await PoolManager.GetServerClient(new PoolSettings{ NoClient = true });
        var server = pairTracker.Pair.Server;

        var entMan = server.ResolveDependency<IServerEntityManager>();

        await server.WaitAssertion(() =>
        {
            var mindSystem = entMan.EntitySysManager.GetEntitySystem<MindSystem>();

            var entity = entMan.SpawnEntity(null, new MapCoordinates());
            var targetEntity = entMan.SpawnEntity(null, new MapCoordinates());
            var mindComp = entMan.EnsureComponent<MindComponent>(entity);
            entMan.EnsureComponent<MindComponent>(targetEntity);

            var mind = CreateMind(new NetUserId(Guid.NewGuid()), mindSystem);

            mindSystem.TransferTo(mind, entity);

            Assert.That(mindSystem.GetMind(entity, mindComp), Is.EqualTo(mind));

            mindSystem.TransferTo(mind, targetEntity);
            Assert.That(mindSystem.GetMind(entity, mindComp), Is.EqualTo(null));
            Assert.That(mindSystem.GetMind(targetEntity), Is.EqualTo(mind));
        });

        await pairTracker.CleanReturnAsync();
    }

    [Test]
    public async Task TestOwningPlayerCanBeChanged()
    {
        await using var pairTracker = await PoolManager.GetServerClient(new PoolSettings{ NoClient = true });
        var server = pairTracker.Pair.Server;

        var entMan = server.ResolveDependency<IServerEntityManager>();

        await server.WaitAssertion(() =>
        {
            var mindSystem = entMan.EntitySysManager.GetEntitySystem<MindSystem>();

            var entity = entMan.SpawnEntity(null, new MapCoordinates());
            var mindComp = entMan.EnsureComponent<MindComponent>(entity);

            var userId = new NetUserId(Guid.NewGuid());
            var mind = CreateMind(userId, mindSystem);

            mindSystem.TransferTo(mind, entity);

            Assert.That(mindSystem.GetMind(entity, mindComp), Is.EqualTo(mind));
            Assert.That(mind.UserId, Is.EqualTo(userId));

            var newUserId = new NetUserId(Guid.NewGuid());
            CatchPlayerDataException(() =>
                mindSystem.ChangeOwningPlayer(entity, newUserId, mindComp));

            Assert.That(mind.UserId, Is.EqualTo(newUserId));
        });

        await pairTracker.CleanReturnAsync();
    }
}