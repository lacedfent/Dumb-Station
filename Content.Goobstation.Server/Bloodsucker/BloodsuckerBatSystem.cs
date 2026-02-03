// Bloodsucker Bat System
using Content.Goobstation.Shared.Bloodsucker.Components;
using Content.Server.Body.Systems;
using Content.Server.Popups;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Polymorph;
using Robust.Shared.Timing;
using System.Numerics;

namespace Content.Goobstation.Server.Bloodsucker;

public sealed class BloodsuckerBatSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly BloodstreamSystem _bloodstream = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BloodsuckerBatComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<BloodsuckerBatComponent, PolymorphedEvent>(OnPolymorphed);
    }

    private void OnStartup(Entity<BloodsuckerBatComponent> ent, ref ComponentStartup args)
    {
        ent.Comp.DrainedAmounts.Clear();
    }

    private void OnPolymorphed(Entity<BloodsuckerBatComponent> ent, ref PolymorphedEvent args)
    {
        // Reset drained amounts when transforming back
        ent.Comp.DrainedAmounts.Clear();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<BloodsuckerBatComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            // Find nearby entities to drain
            var pos = _transform.GetWorldPosition(uid);
            var nearbyEnts = _lookup.GetEntitiesInRange(pos, comp.DrainRange);

            foreach (var target in nearbyEnts)
            {
                if (target == uid)
                    continue;

                // Check if target has blood
                if (!TryComp<BloodstreamComponent>(target, out var bloodstream))
                    continue;

                // Check if already drained max from this target
                if (!comp.DrainedAmounts.TryGetValue(target, out var drained))
                    drained = 0f;

                if (drained >= comp.MaxDrain)
                    continue;

                // Drain less from dead entities
                var drainAmount = comp.DrainRate * frameTime;
                if (!_mobState.IsAlive(target))
                    drainAmount *= 0.2f; // 20% drain rate from dead

                // Don't drain more than max
                var remainingDrain = comp.MaxDrain - drained;
                drainAmount = Math.Min(drainAmount, remainingDrain);

                // Try to drain blood
                if (_bloodstream.TryModifyBloodLevel(target, -drainAmount))
                {
                    comp.DrainedAmounts[target] = drained + drainAmount;

                    // Give blood to the bat's master (if they have bloodsucker component)
                    if (TryComp<BloodsuckerComponent>(uid, out var bloodsuckerComp))
                    {
                        bloodsuckerComp.BloodPoints += drainAmount * 10; // Convert to blood points
                        if (bloodsuckerComp.BloodPoints > bloodsuckerComp.MaxBloodPoints)
                            bloodsuckerComp.BloodPoints = bloodsuckerComp.MaxBloodPoints;
                    }

                    // Show popup occasionally
                    if (drained == 0f || (drained % 2f < drainAmount))
                    {
                        _popup.PopupEntity("You drain blood as you fly!", uid, uid);
                    }
                }
            }
        }
    }
}
