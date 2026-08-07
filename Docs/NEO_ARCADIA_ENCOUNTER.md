# Neo-Arcadia Encounter Setup

Script: `Assets/Scripts/Encounters/NeoArcadiaEncounter.cs`

## Flow

1. **Intro** — short objective
2. **Wave 1** — street patrol (melee-focused)
3. **Phase Gate** — barrier that teaches Phase Shift
4. **Wave 2** — mixed melee + ranged
5. **Elite** — scaled enforcer
6. **Extract** — gate opens → load next scene / menu

## Scene Setup

1. Open `Realm_01_NeoArcadia`
2. Create empty `EncounterDirector`
3. Add component **NeoArcadiaEncounter**
4. Create empty children as spawn points:
   - `Wave1_Spawns` (3 transforms)
   - `Wave2_Spawns` (4 transforms)
   - `Elite_Spawn`
5. Assign spawn point arrays in the inspector
6. Assign enemy prefabs (sample or art-wired)
7. Create a **PhaseBarrier** object (collider blocking the path)
8. Create **ExtractGate** with trigger collider + `ExtractGate.cs`
9. Optional: UI Text for objectives

## Extract Gate

- Tag player as `Player`
- Gate collider **Is Trigger = true**
- `ExtractGate.encounter` → reference the director (or leave empty to auto-find)

## Tuning

| Field | Default |
|-------|---------|
| wave1Count | 3 |
| wave2Count | 4 |
| introDelay | 1.5 |
| nextSceneName | MainMenu |

## Vertical Slice Fit

This encounter is the gameplay spine of `Docs/VERTICAL_SLICE_CHECKLIST.md`.
