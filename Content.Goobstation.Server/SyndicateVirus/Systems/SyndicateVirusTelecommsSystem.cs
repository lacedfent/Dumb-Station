using Content.Server.Chat.Systems;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Server.Popups;
using Content.Server.StationEvents.Components;
using Content.Server.Station.Systems;
using Content.Shared.GameTicking.Components;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Timing;

namespace Content.Goobstation.Server.SyndicateVirus.Systems;

/// <summary>
/// Event raised when the telecommunications virus is purchased from the uplink.
/// </summary>
[DataDefinition]
public sealed partial class PurchaseTelecommsVirusEvent : EntityEventArgs;

/// <summary>
/// Handles the telecommunications virus by spawning a solar flare event.
/// </summary>
public sealed class SyndicateVirusTelecommsSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly StationSystem _station = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;

    private readonly List<(TimeSpan, float, float, bool, bool)> _pendingViruses = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PurchaseTelecommsVirusEvent>(OnPurchaseGlobal);
    }

    private void OnPurchaseGlobal(PurchaseTelecommsVirusEvent args)
    {
        var currentTime = _timing.CurTime;
        _pendingViruses.Add((currentTime, 30f, 45f, false, false)); // activation time, announcement delay, virus delay, announcement sent, virus triggered

        // Note: Can't show popup since this is a broadcast event without entity context
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var currentTime = _timing.CurTime;
        var toRemove = new List<(TimeSpan, float, float, bool, bool)>();

        for (int i = 0; i < _pendingViruses.Count; i++)
        {
            var (activationTime, announcementDelay, virusDelay, announcementSent, virusTriggered) = _pendingViruses[i];
            var elapsed = (currentTime - activationTime).TotalSeconds;

            // Send announcement once
            if (!announcementSent && elapsed >= announcementDelay)
            {
                var stations = _station.GetStations();
                foreach (var station in stations)
                {
                    _chat.DispatchStationAnnouncement(
                        station,
                        Loc.GetString("virus-telecomms-announcement"),
                        Loc.GetString("virus-announcement-sender"),
                        playDefaultSound: true,
                        colorOverride: Color.Red);
                }
                _pendingViruses[i] = (activationTime, announcementDelay, virusDelay, true, virusTriggered);
            }

            // Trigger virus once
            if (!virusTriggered && elapsed >= virusDelay)
            {
                TriggerTelecommsVirus();
                toRemove.Add(_pendingViruses[i]);
            }
        }

        foreach (var item in toRemove)
        {
            _pendingViruses.Remove(item);
        }
    }

    private void TriggerTelecommsVirus()
    {
        // Start a solar flare event (it will run for its default duration)
        _gameTicker.StartGameRule("SolarFlare", out var ruleEntity);
        
        // Suppress the default announcement
        if (TryComp<StationEventComponent>(ruleEntity, out var stationEvent))
        {
            stationEvent.StartAnnouncement = null;
        }
    }
}
