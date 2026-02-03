// Bloodsucker Rule System
using Content.Goobstation.Shared.Bloodsucker.Components;
using Content.Server.Antag;
using Content.Server.GameTicking.Rules;
using Content.Shared.Mind;

namespace Content.Goobstation.Server.Bloodsucker.GameTicking.Rules;

public sealed class BloodsuckerRuleSystem : GameRuleSystem<BloodsuckerRuleComponent>
{
    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BloodsuckerRuleComponent, AfterAntagEntitySelectedEvent>(OnAfterEntitySelected);
    }

    private void OnAfterEntitySelected(Entity<BloodsuckerRuleComponent> ent, ref AfterAntagEntitySelectedEvent args)
    {
        MakeBloodsucker(args.EntityUid, ent);
    }

    public void MakeBloodsucker(EntityUid target, Entity<BloodsuckerRuleComponent> rule)
    {
        // Add bloodsucker component
        EnsureComp<BloodsuckerComponent>(target);

        // Send briefing
        var briefing = Loc.GetString("bloodsucker-role-greeting");
        _antag.SendBriefing(target, briefing, Color.DarkRed, rule.Comp.BriefingSound);
    }
}
