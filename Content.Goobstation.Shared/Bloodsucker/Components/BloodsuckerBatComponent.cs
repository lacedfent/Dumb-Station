// Bloodsucker Bat Component
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Bloodsucker.Components;

/// <summary>
/// Component for bloodsucker bat form that drains blood from nearby entities
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class BloodsuckerBatComponent : Component
{
    /// <summary>
    /// Blood drain rate per second when hovering over someone
    /// </summary>
    [DataField]
    public float DrainRate = 1.0f;

    /// <summary>
    /// Maximum blood that can be drained from one person
    /// </summary>
    [DataField]
    public float MaxDrain = 5.0f;

    /// <summary>
    /// Track how much blood has been drained from each entity
    /// </summary>
    [DataField]
    public Dictionary<EntityUid, float> DrainedAmounts = new();

    /// <summary>
    /// Range to drain blood from
    /// </summary>
    [DataField]
    public float DrainRange = 0.5f;
}
