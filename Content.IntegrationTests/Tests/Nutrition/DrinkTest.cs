using System.Threading.Tasks;
using Content.IntegrationTests.Tests.Chemistry;
using Content.Server.Chemistry.Components.SolutionManager;
using Content.Server.Chemistry.EntitySystems;
using Content.Server.Hands.Components;
using Content.Server.Interaction;
using Content.Server.Nutrition.Components;
using Content.Server.Nutrition.EntitySystems;
using Content.Shared.Body.Components;
using Content.Shared.FixedPoint;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Item;
using NUnit.Framework;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.Nutrition;

[TestFixture]
public sealed class DrinkTest
{
    private const string Prototypes = @"
- type: entity
  parent: BaseItem
  id: DummyDrink
  components:
  - type: SolutionContainerManager
    solutions:
      drink:
        maxVol: 30
        reagents:
        - ReagentId: Water
          Quantity: 30
  - type: SolutionTransfer
    transferAmount: 30
  - type: Drink
    transferAmount: 30
    delay: 0
    forceFeedDelay: 0
  - type: DrainableSolution
    solution: drink

- type: entity
  id: DummyDrinker
  components:
  - type: Hands
  - type: Body
  - type: DoAfter
";
    [Test]
    public async Task TestDrink()
    {
        await using var pairTracker = await PoolManager.GetServerClient(new PoolSettings{ NoClient = true, ExtraPrototypes = Prototypes });
        var server = pairTracker.Pair.Server;

        await server.WaitRunTicks(5);

        var entityManager = server.ResolveDependency<IEntityManager>();
        var systemManager = server.ResolveDependency<IEntitySystemManager>();
        var handsSystem = systemManager.GetEntitySystem<SharedHandsSystem>();

        EntityUid user = default;
        EntityUid drink = default;
        DrinkComponent drinkComp = default;
        await server.WaitAssertion(() =>
        {
            user = entityManager.SpawnEntity("DummyDrinker", MapCoordinates.Nullspace);
            handsSystem.AddHand(user, "hand", HandLocation.Left);

            drink = entityManager.SpawnEntity("DummyDrink", MapCoordinates.Nullspace);
            Assert.That(entityManager.TryGetComponent(drink, out drinkComp));
        });

        await server.WaitRunTicks(5);

        Assert.That(systemManager.TryGetEntitySystem<InteractionSystem>(out var interactionSystem));

        await server.WaitAssertion(() =>
        {
            interactionSystem.UseInHandInteraction(user, drink);
            interactionSystem.UseInHandInteraction(user, drink);
        });

        await server.WaitRunTicks(60);
        await server.WaitIdleAsync();

        await server.WaitAssertion(() =>
        {
            Assert.That(entityManager.TryGetComponent<SolutionContainerManagerComponent>(drink, out var solutionContainerComp));
            Assert.That(solutionContainerComp.Solutions.TryGetValue(drinkComp.SolutionName, out var solution));

            Assert.That(solution.Volume, Is.EqualTo(FixedPoint2.New(0)));
        });
    }
}
