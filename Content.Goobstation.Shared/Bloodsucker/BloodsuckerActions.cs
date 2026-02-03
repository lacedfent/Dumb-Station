// Bloodsucker Action Events
using Content.Shared.Actions;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Bloodsucker;

public sealed partial class BloodsuckerGlareActionEvent : EntityTargetActionEvent
{
}

public sealed partial class BloodsuckerCloakActionEvent : InstantActionEvent
{
}

public sealed partial class BloodsuckerBatFormActionEvent : InstantActionEvent
{
}

public sealed partial class BloodsuckerMistFormActionEvent : InstantActionEvent
{
}

public sealed partial class BloodsuckerDrainActionEvent : EntityTargetActionEvent
{
}

public sealed partial class BloodsuckerThrallActionEvent : EntityTargetActionEvent
{
}
