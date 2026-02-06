# Vampire Admin Guide

## How to Make Someone a Vampire

### Method 1: Using Admin Commands (Recommended)

1. **Open the console** (F12 or ~ key)

2. **Add the Vampire component:**
   ```
   addcomp <username> Vampire
   ```
   Example: `addcomp YourUsername Vampire`

3. **Add the VampireSunlight component** (for space/sunlight damage):
   ```
   addcomp <username> VampireSunlight
   ```
   Example: `addcomp YourUsername VampireSunlight`

4. **Optional - Add to Vampire faction:**
   ```
   addcomp <username> NpcFactionMember
   ```
   Then you may need to manually set the faction to "Vampire" (this might require editing)

### Method 2: Using Game Rules

Start a vampire round:
```
addgamerule Vampire
```

This will automatically select players to become vampires based on the game rule settings (25+ players, 1-2 vampires).

### Method 3: Using Entity UID (if you know the entity ID)

If you have the entity UID:
```
addcomp <entityUid> Vampire
addcomp <entityUid> VampireSunlight
```

## Testing Vampire Features

### Basic Testing
1. Make yourself a vampire using the commands above
2. Press your action hotkeys to see vampire abilities
3. You should start with:
   - Toggle Fangs
   - Glare
   - Rejuvenate I

### Drinking Blood
1. Extend your fangs (use the Toggle Fangs action)
2. Left-click on a humanoid target
3. Watch your blood counter increase

### Class Selection
1. Drink blood until you reach 150 total blood
2. A "Select Vampire Class" action will appear
3. Click it to choose from:
   - **Hemomancer** - Blood magic
   - **Umbrae** - Stealth and shadows
   - **Gargantua** - Tank and melee
   - **Dantalion** - Thralling and illusions

### Unlocking Abilities
- **150 blood**: Choose class + get first class ability
- **300 blood**: Unlock more abilities
- **400 blood**: Unlock more abilities
- **600 blood**: Unlock more abilities
- **1000+ blood + 8 unique victims**: Full power!

## Useful Admin Commands

### Check Blood Level
```
comp <username> Vampire
```
This will show the vampire component data including TotalBlood and DrunkBlood.

### Give Blood Instantly (if needed for testing)
You'll need to use the entity editor or manually edit the component:
```
comp <username> Vampire TotalBlood 500
```
(This syntax may vary - check your admin tools)

### Remove Vampire Status
```
rmcomp <username> Vampire
rmcomp <username> VampireSunlight
```

### Spawn Vampire Mobs
```
spawn VampireMob
```

### Test Thralls (Dantalion class)
1. Become a Dantalion vampire
2. Reach 300+ blood to unlock Enthrall
3. Use Enthrall on a crew member
4. They become your thrall!

## Troubleshooting

### "I don't see any vampire abilities!"
- Make sure you added the Vampire component
- Check your action bar (default: number keys)
- Try relogging or respawning

### "I can't drink blood!"
- Make sure you extended your fangs first
- Target must be a humanoid with blood
- You can only drink 200 units per target

### "Class selection isn't appearing!"
- Make sure you have 150+ total blood
- Check your action bar
- Try: `comp <username> Vampire` to verify blood count

### "Abilities aren't unlocking!"
- Each ability has a blood threshold
- Make sure you've drunk enough blood
- Some abilities require choosing a class first

## Quick Test Scenario

```bash
# 1. Make yourself a vampire
addcomp YourUsername Vampire
addcomp YourUsername VampireSunlight

# 2. Spawn a test dummy to drink from
spawn MobHuman

# 3. Extend fangs and drink blood
# (Use the Toggle Fangs action, then left-click the dummy)

# 4. Check your blood level
comp YourUsername Vampire

# 5. When you hit 150 blood, select a class
# (Use the Select Vampire Class action)

# 6. Test abilities as they unlock!
```

## Game Mode Testing

### Start a Vampire Round
```
addgamerule Vampire
```

### Start a Midround Vampire Event
```
addgamerule VampireMidround
```

### Check Active Game Rules
```
lsgamerules
```

## Notes

- Vampires take damage from sunlight/space light
- Holy water damages vampires and frees thralls
- Chapel (holy places) damage vampires
- Mindshields prevent enthrallment
- Blood fullness replaces hunger system

## Admin Panel Alternative

If the admin panel has an "Add Component" option:
1. Right-click the player
2. Select "Add Component"
3. Search for "Vampire"
4. Add both "Vampire" and "VampireSunlight"

If it has an "Make Antag" option:
1. Right-click the player
2. Select "Make Antag"
3. Look for "Vampire" in the list
4. Select it

(Note: The exact UI may vary depending on your admin tools version)
