// Thrall Component
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Bloodsucker.Components;

/// <summary>
/// Marks an entity as a bloodsucker thrall
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ThrallComponent : Component
{
    /// <summary>
    /// The bloodsucker master
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Master;
}
