// Shared Bloodsucker System
using Content.Goobstation.Shared.Bloodsucker.Components;
using Content.Shared.Actions;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Bloodsucker;

public abstract class SharedBloodsuckerSystem : EntitySystem
{
    [Dependency] protected readonly SharedActionsSystem Actions = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BloodsuckerComponent, ComponentInit>(OnComponentInit);
    }

    private void OnComponentInit(Entity<BloodsuckerComponent> ent, ref ComponentInit args)
    {
        // Initialize bloodsucker
    }
}

[Serializable, NetSerializable]
public sealed class BloodDrainDoAfterEvent : SimpleDoAfterEvent
{
}

[Serializable, NetSerializable]
public sealed class ThrallConvertDoAfterEvent : SimpleDoAfterEvent
{
}
