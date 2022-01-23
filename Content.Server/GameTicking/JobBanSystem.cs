using Robust.Shared.GameObjects;
using Robust.Shared.Network;

namespace Content.Server.GameTicking;

public class JobBanSystem : EntitySystem
{
    public bool IsBanned(NetUserId playerUserId, string jobId)
    {
        return true;
    }
}
