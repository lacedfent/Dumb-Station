// Bloodsucker System - Main Logic
using Content.Goobstation.Shared.Bloodsucker;
using Content.Goobstation.Shared.Bloodsucker.Components;
using Content.Server.Actions;
using Content.Server.Antag;
using Content.Server.Body.Systems;
using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Server.Polymorph.Systems;
using Content.Server.Stunnable;
using Content.Shared.Actions;
using Content.Shared.Alert;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Mind;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Systems;
using Content.Shared.Polymorph;
using Content.Shared.Stealth;
using Content.Shared.Stealth.Components;
using Robust.Server.Audio;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using System.Numerics;

namespace Content.Goobstation.Server.Bloodsucker;

public sealed partial class BloodsuckerSystem : SharedBloodsuckerSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly DamageableSystem _damage = default!;
    [Dependency] private readonly BloodstreamSystem _bloodstream = default!;
    [Dependency] private readonly StunSystem _stun = default!;
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _movement = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly SharedStealthSystem _stealth = default!;
    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly AudioSystem _audio = default!;
    [Dependency] private readonly PolymorphableSystem _polymorph = default!;

    private const string BloodReagent = "Blood";
    
    private readonly SoundSpecifier _greetingSound = new SoundPathSpecifier("/Audio/_Goobstation/Bloodsucker/bloodsucker_greeting.ogg");

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BloodsuckerComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<BloodsuckerComponent, BloodDrainDoAfterEvent>(OnBloodDrain);
        SubscribeLocalEvent<BloodsuckerComponent, ThrallConvertDoAfterEvent>(OnThrallConvert);
        SubscribeLocalEvent<BloodsuckerComponent, EntGotInsertedIntoContainerMessage>(OnEnteredContainer);
        SubscribeLocalEvent<BloodsuckerComponent, EntGotRemovedFromContainerMessage>(OnExitedContainer);
        
        SubscribeActions();
    }

    private void OnEnteredContainer(Entity<BloodsuckerComponent> ent, ref EntGotInsertedIntoContainerMessage args)
    {
        // Check if entered a coffin (EntityStorage)
        if (HasComp<EntityStorageComponent>(args.Container.Owner))
        {
            ent.Comp.Coffin = args.Container.Owner;
            _popup.PopupEntity("You rest in the coffin, feeling your power slowly return...", ent, ent);
        }
    }

    private void OnExitedContainer(Entity<BloodsuckerComponent> ent, ref EntGotRemovedFromContainerMessage args)
    {
        if (ent.Comp.Coffin == args.Container.Owner)
        {
            ent.Comp.Coffin = null;
        }
    }

    private void OnStartup(Entity<BloodsuckerComponent> ent, ref ComponentStartup args)
    {
        // Grant initial abilities
        _actions.AddAction(ent, "ActionBloodsuckerGlare");
        _actions.AddAction(ent, "ActionBloodsuckerCloak");
        _actions.AddAction(ent, "ActionBloodsuckerBatForm");
        _actions.AddAction(ent, "ActionBloodsuckerMistForm");
        _actions.AddAction(ent, "ActionBloodsuckerDrain");
        _actions.AddAction(ent, "ActionBloodsuckerThrall");

        UpdateBloodAlert(ent);

        // Send greeting message
        if (_mind.TryGetMind(ent, out var mindId, out _))
        {
            var briefing = Loc.GetString("bloodsucker-role-greeting");
            _antag.SendBriefing(ent, briefing, Color.DarkRed, _greetingSound);
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<BloodsuckerComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            // Coffin regeneration
            if (comp.Coffin != null && EntityManager.EntityExists(comp.Coffin.Value))
            {
                comp.BloodPoints += 10f * frameTime; // Regen 10 blood/sec in coffin
                if (comp.BloodPoints > comp.MaxBloodPoints)
                    comp.BloodPoints = comp.MaxBloodPoints;
            }

            // Sunlight damage
            if (IsInSunlight(uid))
            {
                var damage = new DamageSpecifier(_proto.Index<DamageTypePrototype>("Heat"), comp.SunlightDamage * frameTime);
                _damage.TryChangeDamage(uid, damage);
                _popup.PopupEntity("You burn in the light!", uid, uid);
            }

            // Blood drain
            if (comp.IsDraining && comp.DrainTarget != null)
            {
                if (!TryDrainBlood(uid, comp.DrainTarget.Value, comp, frameTime))
                {
                    comp.IsDraining = false;
                    comp.DrainTarget = null;
                }
            }

            // Passive blood consumption (only if not in coffin)
            if (comp.Coffin == null && comp.BloodPoints > 0)
            {
                comp.BloodPoints -= 0.5f * frameTime;
                if (comp.BloodPoints < 0)
                    comp.BloodPoints = 0;
            }

            UpdateBloodAlert((uid, comp));
        }
    }

    private bool IsInSunlight(EntityUid uid)
    {
        // Check if entity is in a lit area
        // This is a simplified check - you'd want to check actual light levels
        return false; // TODO: Implement proper light checking
    }

    private bool TryDrainBlood(EntityUid bloodsucker, EntityUid target, BloodsuckerComponent comp, float frameTime)
    {
        if (!_mobState.IsAlive(target))
            return false;

        // Check distance
        var distance = Vector2.Distance(
            _transform.GetWorldPosition(bloodsucker),
            _transform.GetWorldPosition(target)
        );

        if (distance > 1.5f)
        {
            _popup.PopupEntity("Too far away!", bloodsucker, bloodsucker);
            return false;
        }

        // Drain blood
        if (_bloodstream.TryModifyBloodLevel(target, -comp.DrainRate * frameTime))
        {
            comp.BloodPoints += comp.DrainRate * frameTime;
            if (comp.BloodPoints > comp.MaxBloodPoints)
                comp.BloodPoints = comp.MaxBloodPoints;

            return true;
        }

        return false;
    }

    private void OnBloodDrain(Entity<BloodsuckerComponent> ent, ref BloodDrainDoAfterEvent args)
    {
        if (args.Cancelled || args.Target == null)
        {
            ent.Comp.IsDraining = false;
            ent.Comp.DrainTarget = null;
            return;
        }

        _popup.PopupEntity("You finish draining blood!", ent, ent);
        ent.Comp.IsDraining = false;
        ent.Comp.DrainTarget = null;
    }

    private void OnThrallConvert(Entity<BloodsuckerComponent> ent, ref ThrallConvertDoAfterEvent args)
    {
        if (args.Cancelled || args.Target == null)
            return;

        var target = args.Target.Value;

        // Add thrall component
        var thrall = EnsureComp<ThrallComponent>(target);
        thrall.Master = ent;

        ent.Comp.Thralls.Add(target);

        _popup.PopupEntity($"You have enthralled {Name(target)}!", ent, ent);
        _popup.PopupEntity("You feel an overwhelming loyalty to your master...", target, target);
    }

    public bool TryUseBlood(Entity<BloodsuckerComponent> ent, float cost)
    {
        if (ent.Comp.BloodPoints < cost)
        {
            _popup.PopupEntity("Not enough blood!", ent, ent);
            return false;
        }

        ent.Comp.BloodPoints -= cost;
        UpdateBloodAlert(ent);
        return true;
    }

    private void UpdateBloodAlert(Entity<BloodsuckerComponent> ent)
    {
        var percentage = ent.Comp.BloodPoints / ent.Comp.MaxBloodPoints;
        
        // Convert to 0-10 scale (11 levels like bleed)
        var level = (short)Math.Clamp((int)(percentage * 10), 0, 10);
        
        _alerts.ShowAlert(ent, "BloodLevel", level);
    }

    public void ToggleCloak(Entity<BloodsuckerComponent> ent)
    {
        if (!ent.Comp.IsCloaked)
        {
            if (!TryUseBlood(ent, ent.Comp.CloakCost))
                return;

            var stealth = EnsureComp<StealthComponent>(ent);
            stealth.Visibility = 0.66f;
            _stealth.SetVisibility(ent, 0.66f, stealth);

            ent.Comp.IsCloaked = true;
            _popup.PopupEntity("You fade into darkness...", ent, ent);
        }
        else
        {
            RemComp<StealthComponent>(ent);
            ent.Comp.IsCloaked = false;
            _popup.PopupEntity("You become visible again.", ent, ent);
        }
    }

    public void UseGlare(Entity<BloodsuckerComponent> ent, EntityUid target)
    {
        if (!TryUseBlood(ent, ent.Comp.GlareCost))
            return;

        if (HasComp<BloodsuckerComponent>(target) || HasComp<ThrallComponent>(target))
        {
            _popup.PopupEntity("Your glare has no effect!", ent, ent);
            return;
        }

        _stun.TryParalyze(target, TimeSpan.FromSeconds(5), true);
        _popup.PopupEntity("You glare at your victim, freezing them in place!", ent, ent);
        _popup.PopupEntity("You are frozen by an unnatural gaze!", target, target);
    }
}
