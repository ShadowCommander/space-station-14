using Content.Server.Database;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.IoC;

namespace Content.Server.Administration.Commands;


[AdminCommand(AdminFlags.Ban)]
public sealed class RoleBanCommand : IConsoleCommand
{
    public string Command => "roleban";
    public string Description => "Bans a player from a job";
    public string Help => $"Usage: {Command} <name or user ID> <role> <reason>";

    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 3)
        {
            shell.WriteLine(Help);
            return;
        }

        // var player = shell.Player as IPlayerSession;
        // var plyMgr = IoCManager.Resolve<IPlayerManager>();
        var locator = IoCManager.Resolve<IPlayerLocator>();
        var dbMan = IoCManager.Resolve<IServerDbManager>();

        var target = args[0];
        var role = args[1];
        // var reason = args[2];


        var located = await locator.LookupIdByNameOrIdAsync(target);
        if (located == null)
        {
            shell.WriteError("Unable to find a player with that name.");
            return;
        }

        await dbMan.AddPlayerJobBanAsync(located.UserId, role);
    }
}
