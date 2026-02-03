# Blood Sucker Antag - Required Sprites

## Action Icons (32x32 PNG)
Create these in: `Resources/Textures/Interface/Actions/`

1. **bloodsucker_glare.png** - Eye with hypnotic swirl
2. **bloodsucker_cloak.png** - Dark shadowy cloak
3. **bloodsucker_bat.png** - Bat silhouette
4. **bloodsucker_mist.png** - Misty/foggy effect
5. **bloodsucker_drain.png** - Fangs dripping blood
6. **bloodsucker_thrall.png** - Mind control symbol

**Reference examples**: Look at `Resources/Textures/Interface/Actions/` for style
- `harm.png`, `disarm.png`, `scream.png`, `zombie-turn.png`
- `changeling.rsi/armblade.png`

## Alert Icons - USING EXISTING BLEED SPRITES! ✅
**No sprites needed!** The blood level meter now reuses the existing bleed alert sprites.
- It shows a blood drop that fills up as you gain more blood (0-10 levels)
- Empty = low blood, Full = maximum blood
- Located at: `Resources/Textures/Interface/Alerts/bleed.rsi/`

## Optional Mob Sprites
If you want custom bat/mist forms:

### Bat Form
Create RSI folder: `Resources/Textures/_Goobstation/Mobs/Bloodsucker/bat.rsi/`
- **alive.png** - Bat sprite (4 directions)
- **dead.png** - Dead bat sprite

### Mist Form  
Create RSI folder: `Resources/Textures/_Goobstation/Mobs/Bloodsucker/mist.rsi/`
- **mist.png** - Misty cloud sprite

## Coffin Sprite (Optional)
If you want a custom coffin instead of reusing closet:
Create in: `Resources/Textures/_Goobstation/Structures/Bloodsucker/`
- **coffin.rsi/** folder with coffin sprites

---

## Implementation Status

### ✅ Completed:
- Core bloodsucker component system
- Blood points tracking (0-1000)
- **Blood level alert using existing bleed sprites (11 levels)**
- Glare (stun) ability
- Cloak of Darkness ability
- Blood drain mechanic
- Thrall conversion system
- Action system integration

### 🚧 Needs Implementation:
- Bat form transformation (polymorph)
- Mist form transformation (polymorph)
- Sunlight damage system (needs light level checking)
- Coffin resting/regeneration
- Antag role integration (game rules)
- Objectives system
- Thrall abilities/communication

### 📝 Files Created:
1. `Content.Goobstation.Shared/Bloodsucker/Components/BloodsuckerComponent.cs`
2. `Content.Goobstation.Shared/Bloodsucker/Components/ThrallComponent.cs`
3. `Content.Goobstation.Shared/Bloodsucker/Components/BloodsuckerCoffinComponent.cs`
4. `Content.Goobstation.Shared/Bloodsucker/SharedBloodsuckerSystem.cs`
5. `Content.Goobstation.Shared/Bloodsucker/BloodsuckerActions.cs`
6. `Content.Goobstation.Server/Bloodsucker/BloodsuckerSystem.cs`
7. `Content.Goobstation.Server/Bloodsucker/BloodsuckerSystem.Actions.cs`
8. `Resources/Prototypes/_Goobstation/Bloodsucker/actions.yml`
9. `Resources/Prototypes/_Goobstation/Bloodsucker/alerts.yml`
10. `Resources/Prototypes/_Goobstation/Bloodsucker/entities.yml`

### 🎮 Core Features:
- **Blood Points**: 0-1000 system, drains passively (0.5/sec)
- **Blood Alert**: Visual meter showing blood level (reuses bleed sprites)
- **Glare**: 50 blood, paralyzes target for 5 seconds
- **Cloak**: 30 blood, 66% invisibility
- **Drain**: Sucks blood from victims (20 blood/sec)
- **Enthrall**: 200 blood, converts victim to loyal thrall
- **Bat Form**: 100 blood (placeholder)
- **Mist Form**: 150 blood (placeholder)

### 📋 Sprites Still Needed:
**Only 6 action icons!** (32x32 PNG each)
1. bloodsucker_glare.png
2. bloodsucker_cloak.png
3. bloodsucker_bat.png
4. bloodsucker_mist.png
5. bloodsucker_drain.png
6. bloodsucker_thrall.png

Everything else is optional or reuses existing sprites!
