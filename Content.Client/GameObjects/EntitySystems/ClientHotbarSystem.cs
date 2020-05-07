using System;
using System.Collections.Generic;
using System.Text;
using Content.Client.GameObjects.Components.HUD.Hotbar;
using Content.Shared.Input;
using Robust.Client.GameObjects.EntitySystems;
using Robust.Shared.GameObjects;
using Robust.Shared.GameObjects.Systems;
using Robust.Shared.Input;
using Robust.Shared.Map;
using Robust.Shared.Players;

namespace Content.Client.GameObjects.EntitySystems
{
    public class ClientHotbarSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();

            EntityQuery = new TypeEntityQuery(typeof(HotbarComponent));

            if (!EntitySystemManager.TryGetEntitySystem<InputSystem>(out var input))
            {
                return;
            }

            input.BindMap.BindFunction(ContentKeyFunctions.MouseMiddle, new PointerInputCmdHandler(ActivateAbility));
            input.BindMap.BindFunction(ContentKeyFunctions.OpenAbilitiesMenu, new PointerInputCmdHandler(ActivateAbility));
            input.BindMap.BindFunction(ContentKeyFunctions.Hotbar0, new PointerInputCmdHandler((s, pos, uid) => HotkeyToggle(s, pos, uid, 0)));
        }

        public override void Shutdown()
        {
            base.Shutdown();

            if (!EntitySystemManager.TryGetEntitySystem<InputSystem>(out var input))
            {
                return;
            }

            input.BindMap.UnbindFunction(ContentKeyFunctions.MouseMiddle);
            input.BindMap.UnbindFunction(ContentKeyFunctions.Hotbar0);
        }

        private bool ActivateAbility(ICommonSession session, GridCoordinates coords, EntityUid uid)
        {
            var plyEnt = session.AttachedEntity;

            if (plyEnt == null || !plyEnt.IsValid())
                return false;

            if (!plyEnt.TryGetComponent(out HotbarComponent hotbarComp))
                return false;

            //if (hotbarComp.SelectedAbility == null)
            //    return false;

            //hotbarComp.SelectedAbility?.Trigger(session, coords, uid, _gameTiming.CurTime);
            return true;
        }

        private bool HotkeyToggle(ICommonSession session, GridCoordinates pos, EntityUid uid, int index)
        {
            var plyEnt = session.AttachedEntity;

            if (plyEnt == null || !plyEnt.IsValid())
            {
                return false;
            }

            if (!plyEnt.TryGetComponent(out HotbarComponent hotbarComp))
            {
                return false;
            }

            hotbarComp.ActivateAbility(index, pos);
            return true;
        }

    }
}
