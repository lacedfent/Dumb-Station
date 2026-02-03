// Bloodsucker System - Action Handlers
using Content.Goobstation.Shared.Bloodsucker;
using Content.Goobstation.Shared.Bloodsucker.Components;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Mobs;

namespace Content.Goobstation.Server.Bloodsucker;

public sealed partial class BloodsuckerSystem
{
    private void SubscribeActions()
    {
        SubscribeLocalEvent<BloodsuckerComponent, BloodsuckerGlareActionEvent>(OnGlareAction);
        SubscribeLocalEvent<BloodsuckerComponent, BloodsuckerCloakActionEvent>(OnCloakAction);
        SubscribeLocalEvent<BloodsuckerComponent, BloodsuckerBatFormActionEvent>(OnBatFormAction);
        SubscribeLocalEvent<BloodsuckerComponent, BloodsuckerMistFormActionEvent>(OnMistFormAction);
        SubscribeLocalEvent<BloodsuckerComponent, BloodsuckerDrainActionEvent>(OnDrainAction);
        SubscribeLocalEvent<BloodsuckerComponent, BloodsuckerThrallActionEvent>(OnThrallAction);
    }

    private void OnGlareAction(Entity<BloodsuckerComponent> ent, ref BloodsuckerGlareActionEvent args)
    {
        if (args.Handled)
            return;

        UseGlare(ent, args.Target);
        args.Handled = true;
    }

    private void OnCloakAction(Entity<BloodsuckerComponent> ent, ref BloodsuckerCloakActionEvent args)
    {
        if (args.Handled)
            return;

        ToggleCloak(ent);
        args.Handled = true;
    }

    private void OnBatFormAction(Entity<BloodsuckerComponent> ent, ref BloodsuckerBatFormActionEvent args)
    {
        if (args.Handled)
            return;

        if (!TryUseBlood(ent, ent.Comp.BatFormCost))
            return;

        // Toggle bat form
        if (!ent.Comp.InBatForm)
        {
            if (_polymorph.PolymorphEntity(ent, "BloodsuckerBatForm") != null)
            {
                ent.Comp.InBatForm = true;
                _popup.PopupEntity("You transform into a bat!", ent, ent);
            }
        }
        else
        {
            if (_polymorph.Revert((ent, null)))
            {
                ent.Comp.InBatForm = false;
                _popup.PopupEntity("You return to your normal form.", ent, ent);
            }
        }

        args.Handled = true;
    }

    private void OnMistFormAction(Entity<BloodsuckerComponent> ent, ref BloodsuckerMistFormActionEvent args)
    {
        if (args.Handled)
            return;

        if (!TryUseBlood(ent, ent.Comp.MistFormCost))
            return;

        // Mist form is temporary (10 seconds)
        if (_polymorph.PolymorphEntity(ent, "BloodsuckerMistForm") != null)
        {
            ent.Comp.InMistForm = true;
            _popup.PopupEntity("You transform into mist!", ent, ent);
        }

        args.Handled = true;
    }

    private void OnDrainAction(Entity<BloodsuckerComponent> ent, ref BloodsuckerDrainActionEvent args)
    {
        if (args.Handled)
            return;

        var target = args.Target;

        if (!_mobState.IsAlive(target))
        {
            _popup.PopupEntity("Target must be alive!", ent, ent);
            return;
        }

        if (HasComp<BloodsuckerComponent>(target))
        {
            _popup.PopupEntity("You cannot drain another bloodsucker!", ent, ent);
            return;
        }

        // Start draining
        var doAfterArgs = new DoAfterArgs(EntityManager, ent, TimeSpan.FromSeconds(5), new BloodDrainDoAfterEvent(), ent, target: target)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = false
        };

        if (_doAfter.TryStartDoAfter(doAfterArgs))
        {
            ent.Comp.IsDraining = true;
            ent.Comp.DrainTarget = target;
            _popup.PopupEntity("You sink your fangs into your victim!", ent, ent);
            _popup.PopupEntity("You feel your blood being drained!", target, target);
        }

        args.Handled = true;
    }

    private void OnThrallAction(Entity<BloodsuckerComponent> ent, ref BloodsuckerThrallActionEvent args)
    {
        if (args.Handled)
            return;

        var target = args.Target;

        if (!TryUseBlood(ent, ent.Comp.ThrallCost))
            return;

        if (!_mobState.IsAlive(target))
        {
            _popup.PopupEntity("Target must be alive!", ent, ent);
            return;
        }

        if (HasComp<BloodsuckerComponent>(target) || HasComp<ThrallComponent>(target))
        {
            _popup.PopupEntity("Target is already enthralled or is a bloodsucker!", ent, ent);
            return;
        }

        // Start enthralling
        var doAfterArgs = new DoAfterArgs(EntityManager, ent, TimeSpan.FromSeconds(10), new ThrallConvertDoAfterEvent(), ent, target: target)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = false
        };

        if (_doAfter.TryStartDoAfter(doAfterArgs))
        {
            _popup.PopupEntity("You begin to enthrall your victim...", ent, ent);
            _popup.PopupEntity("Your mind is being invaded!", target, target);
        }

        args.Handled = true;
    }
}
