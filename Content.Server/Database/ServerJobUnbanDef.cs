using System;
using Robust.Shared.Network;

namespace Content.Server.Database;

public class ServerJobUnbanDef
{
    public int BanId { get; }

    public NetUserId? UnbanningAdmin { get; }

    public DateTimeOffset UnbanTime { get; }

    public ServerJobUnbanDef(int banId, NetUserId? unbanningAdmin, DateTimeOffset unbanTime)
    {
        BanId = banId;
        UnbanningAdmin = unbanningAdmin;
        UnbanTime = unbanTime;
    }
}
