// Blood Sucker Antag Component
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Bloodsucker.Components;

/// <summary>
/// Marks an entity as a bloodsucker vampire
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class BloodsuckerComponent : Component
{
    /// <summary>
    /// Current blood points
    /// </summary>
    [DataField, AutoNetworkedField]
    public float BloodPoints = 0f;

    /// <summary>
    /// Maximum blood points
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MaxBloodPoints = 1000f;

    /// <summary>
    /// Blood points required to use abilities
    /// </summary>
    [DataField]
    public float GlareCost = 50f;

    [DataField]
    public float CloakCost = 30f;

    [DataField]
    public float BatFormCost = 100f;

    [DataField]
    public float MistFormCost = 150f;

    [DataField]
    public float ThrallCost = 200f;

    /// <summary>
    /// Is the bloodsucker currently cloaked?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IsCloaked = false;

    /// <summary>
    /// Coffin entity UID
    /// </summary>
    [DataField]
    public EntityUid? Coffin;

    /// <summary>
    /// Is currently in bat form?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool InBatForm = false;

    /// <summary>
    /// Is currently in mist form?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool InMistForm = false;

    /// <summary>
    /// List of thralls
    /// </summary>
    [DataField]
    public List<EntityUid> Thralls = new();

    /// <summary>
    /// Sunlight damage per second
    /// </summary>
    [DataField]
    public float SunlightDamage = 5f;

    /// <summary>
    /// Blood drain rate per second
    /// </summary>
    [DataField]
    public float DrainRate = 20f;

    /// <summary>
    /// Is currently draining blood?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IsDraining = false;

    /// <summary>
    /// Target being drained
    /// </summary>
    [DataField]
    public EntityUid? DrainTarget;
}
