// Bloodsucker Rule Component
using Content.Server.Antag;
using Content.Server.GameTicking.Rules.Components;
using Robust.Shared.Audio;

namespace Content.Goobstation.Server.Bloodsucker.GameTicking.Rules;

[RegisterComponent, Access(typeof(BloodsuckerRuleSystem))]
public sealed partial class BloodsuckerRuleComponent : Component
{
    public readonly SoundSpecifier BriefingSound = new SoundPathSpecifier("/Audio/_Goobstation/Bloodsucker/bloodsucker_greeting.ogg");
}
