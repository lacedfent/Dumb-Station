# Vampire API Compatibility Fixes

## API Mapping: Starlight → Goobstation

### Stun System

**Starlight (doesn't exist):**
```csharp
_stun.TryAddParalyzeDuration(uid, duration);
_stun.TryAddStunDuration(uid, duration);
_stun.TryUnstun(uid, SharedStunSystem.StunId);
```

**Goobstation (correct):**
```csharp
_stun.TryParalyze(uid, duration, true);  // paralyze = knockdown + stun
_stun.TryStun(uid, duration, true);      // just stun
_statusEffects.TryRemoveStatusEffect(uid, "Stun");  // remove stun
_statusEffects.TryRemoveStatusEffect(uid, "KnockedDown");  // remove knockdown
```

### Stamina System

**Starlight (doesn't exist):**
```csharp
_stamina.AdjustStatus(uid, value);
_stamina.UpdateStaminaVisuals(uid);
SharedStaminaSystem.StaminaLow
```

**Goobstation (correct):**
```csharp
_stamina.TakeStaminaDamage(uid, value);  // take stamina damage
// Visuals update automatically, no manual call needed
// No StaminaLow constant - check stamina.StaminaDamage directly
```

### Movement Speed Modifiers

**Starlight (doesn't exist):**
```csharp
_movementMod.TryAddMovementSpeedModDuration(uid, key, duration, multiplier);
```

**Goobstation (alternative):**
```csharp
// Use status effects or direct movement speed modification
// For temporary effects, use stun system which affects movement
// Or use MovementSpeedModifierSystem directly
```

### Status Effect Components

**Starlight (doesn't exist):**
- `StunnedStatusEffectComponent`
- `KnockdownStatusEffectComponent`  
- `MovementModStatusEffectComponent`

**Goobstation (correct):**
- Use `StatusEffectsComponent` with string keys: "Stun", "KnockedDown"
- Check via `_statusEffects.HasStatusEffect(uid, "Stun")`

### Temperature

**Starlight:**
- `TemperatureComponent`

**Goobstation:**
- Check if this exists or use alternative

### WarpPoint

**Starlight:**
```csharp
warpPoint.Blacklist
```

**Goobstation:**
- Check current WarpPointComponent structure

## Files to Fix

1. `VampireSystem.Abilities.cs` - Glare, Rejuvenate abilities
2. `DantalionSystem.cs` - Pacify, Rally abilities  
3. `GargantuaSystem.cs` - Overwhelming Force, Charge abilities
4. `HemomancerSystem.cs` - Blood Tendrils
5. `UmbraeSystem.cs` - Various abilities
6. `ShadowSnareSystem.cs` - Trap effects
7. `VampireSystem.cs` - Core system

## Strategy

1. Replace all `TryAddParalyzeDuration` with `TryParalyze`
2. Replace all `TryAddStunDuration` with `TryStun`
3. Replace all `TryUnstun` with `TryRemoveStatusEffect`
4. Replace all stamina `AdjustStatus` with `TakeStaminaDamage`
5. Remove all `UpdateStaminaVisuals` calls (automatic)
6. Comment out or remove status effect component checks
7. Fix temperature and warp point issues
