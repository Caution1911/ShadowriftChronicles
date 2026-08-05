# Scene Setup Guide – Shadowrift Chronicles

This guide explains how to set up all required scenes so the game runs correctly.

---

## 1. Main Menu Scene

**Scene Name:** `MainMenu`

1. Create a new scene and save it as `Assets/Scenes/MainMenu.unity`
2. Create a Canvas (Screen Space - Overlay)
3. Add three UI Buttons:
   - **New Game**
   - **Continue**
   - **Quit**
4. Create an empty GameObject named `MainMenuController`
5. Attach the `MainMenuController` script
6. Assign the Continue button to the `continueButton` field
7. Link the buttons:
   - New Game → `StartNewGame()`
   - Continue → `ContinueGame()`
   - Quit → `QuitGame()`

---

## 2. Realm Scenes

Create these five scenes in `Assets/Scenes/`:

| Scene Name                  | Theme                      |
|----------------------------|----------------------------|
| `Realm_01_NeoArcadia`      | Cyberpunk neon city        |
| `Realm_02_Wildlands`       | Bioluminescent forest      |
| `Realm_03_Clockwork`       | Mechanical floating citadel|
| `Realm_04_BoneDesert`      | Arid bone wasteland        |
| `Realm_05_CrystalSanctum`  | Glowing crystal caves      |

### For every Realm scene, do the following:

1. Create two empty GameObjects:
   - `BaseLayer`
   - `PhaseLayer`
2. Create an empty GameObject named `GameSystems`
3. Attach these scripts to it (or use a prefab):
   - `GameManager`
   - `PhasingManager` (assign BaseLayer + PhaseLayer)
   - `StanceManager`
   - `LoyaltyManager`
   - `ScoreManager`
   - `AudioManager`
   - `RealmLoader`
   - `RealmProgression`
4. Create a Canvas and set up:
   - Loyalty bar (Image fill)
   - Health bar
   - Score text
   - Stance icons (3 Images)
5. Attach `UIManager` and assign all UI references
6. Create the Player:
   - Sprite + Rigidbody2D + CapsuleCollider2D
   - Tag = `Player`
   - Attach `PlayerController` + `PlayerHealth`
7. Create an `EnemySpawner` GameObject
   - Assign enemy prefabs
   - Create several empty child objects as spawn points
8. (Optional) Add a `HandGestureManager` with `QuestHandGestureManager` for Quest builds

---

## 3. Reality Storm (Final Scene)

**Scene Name:** `Realm_06_RealityStorm`

1. Copy the structure from a normal realm scene
2. Add a `BossSpawner` GameObject
3. Assign the Reality Storm Boss prefab and a spawn point
4. When the boss dies, call `EndingManager.ShowEnding(loyalty)`

---

## 4. Build Settings

1. Go to **File → Build Settings**
2. Add scenes in this order:
   1. MainMenu
   2. Realm_01_NeoArcadia
   3. Realm_02_Wildlands
   4. Realm_03_Clockwork
   5. Realm_04_BoneDesert
   6. Realm_05_CrystalSanctum
   7. Realm_06_RealityStorm
3. Switch Platform to **Android** for Meta Quest
4. Set Package Name and Minimum API Level (29+)

---

## 5. Prefabs You Should Create

- Player prefab
- AggressiveEnemy prefab
- RangedEnemy prefab
- LoyaltyEnemy prefab
- Projectile prefab
- RealityStormBoss prefab
- Death particle prefab

Make sure every enemy has the tag **Enemy** and an `EnemyDeathHandler` component.

---

## Tips

- Use different background colors or sprites for each realm so the player can tell them apart
- Test phasing by assigning different objects to BaseLayer vs PhaseLayer
- On Meta Quest, always test hand tracking on actual hardware
