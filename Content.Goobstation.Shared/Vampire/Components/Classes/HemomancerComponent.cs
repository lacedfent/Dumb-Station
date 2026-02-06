using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Vampire.Components.Classes;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class HemomancerComponent : VampireClassComponent
{
    [AutoNetworkedField]
    public bool InSanguinePool = false;
    [AutoNetworkedField]
    public bool BloodBringersRiteActive = false;
    public int BloodBringersRiteLoopId = 0;
}