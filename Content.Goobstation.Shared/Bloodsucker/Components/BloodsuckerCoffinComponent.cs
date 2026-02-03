// Bloodsucker Coffin Component
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Bloodsucker.Components;

/// <summary>
/// Marks an entity as a bloodsucker coffin/lair
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class BloodsuckerCoffinComponent : Component
{
    /// <summary>
    /// The bloodsucker owner
    /// </summary>
    [DataField]
    public EntityUid? Owner;

    /// <summary>
    /// Blood regeneration rate while resting
    /// </summary>
    [DataField]
    public float RegenRate = 10f;
}
