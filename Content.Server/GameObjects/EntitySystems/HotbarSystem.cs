using System;
using Content.Server.GameObjects.Components.HUD.Hotbar;
using Content.Shared.Input;
using Robust.Server.GameObjects.EntitySystems;
using Robust.Server.Interfaces.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.GameObjects.Systems;
using Robust.Shared.Input;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Players;

namespace Content.Server.GameObjects.EntitySystems
{
    internal sealed class HotbarSystem : EntitySystem
    {
#pragma warning disable 649
        [Dependency] private readonly IGameTiming _gameTiming;
#pragma warning restore 649

        public override void Initialize()
        {
            base.Initialize();

            EntityQuery = new TypeEntityQuery(typeof(HotbarComponent));

            if (!EntitySystemManager.TryGetEntitySystem<InputSystem>(out var input))
            {
                return;
            }

            input.BindMap.BindFunction(ContentKeyFunctions.MouseMiddle, new PointerInputCmdHandler(ActivateAbility));
        }

        public override void Shutdown()
        {
            base.Shutdown();

            if (!EntitySystemManager.TryGetEntitySystem<InputSystem>(out var input))
            {
                return;
            }

            input.BindMap.UnbindFunction(ContentKeyFunctions.MouseMiddle);
        }

        private bool ActivateAbility(ICommonSession session, GridCoordinates coords, EntityUid uid)
        {
            var plyEnt = ((IPlayerSession)session).AttachedEntity;

            if (plyEnt == null || !plyEnt.IsValid())
                return false;

            if (!plyEnt.TryGetComponent(out HotbarComponent hotbarComp))
                return false;

            if (hotbarComp.SelectedAbility == null)
                return false;

            hotbarComp.SelectedAbility?.Trigger(session, coords, uid, _gameTiming.CurTime);
            return true;
        }
    }
}
