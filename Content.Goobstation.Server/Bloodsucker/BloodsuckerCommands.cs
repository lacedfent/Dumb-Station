// Bloodsucker Admin Commands
using Content.Goobstation.Shared.Bloodsucker.Components;
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Goobstation.Server.Bloodsucker;

[AdminCommand(AdminFlags.Fun)]
public sealed class MakeBloodsuckerCommand : IConsoleCommand
{
    public string Command => "makebloodsucker";
    public string Description => "Makes an entity a bloodsucker.";
    public string Help => "makebloodsucker <entity uid>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteLine("Usage: makebloodsucker <entity uid>");
            return;
        }

        if (!NetEntity.TryParse(args[0], out var netEntity))
        {
            shell.WriteLine("Invalid entity uid.");
            return;
        }

        var entityManager = IoCManager.Resolve<IEntityManager>();
        var entity = entityManager.GetEntity(netEntity);

        if (!entityManager.EntityExists(entity))
        {
            shell.WriteLine("Entity does not exist.");
            return;
        }

        entityManager.EnsureComponent<BloodsuckerComponent>(entity);
        shell.WriteLine($"Made {entityManager.ToPrettyString(entity)} a bloodsucker.");
    }
}

[AdminCommand(AdminFlags.Fun)]
public sealed class MakeBloodsuckerSelfCommand : LocalizedCommands
{
    [Dependency] private readonly IEntityManager _entities = default!;

    public override string Command => "makebloodsuckerself";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player?.AttachedEntity is not { } entity)
        {
            shell.WriteError("You must be attached to an entity.");
            return;
        }

        _entities.EnsureComponent<BloodsuckerComponent>(entity);
        shell.WriteLine("You are now a bloodsucker!");
    }
}
