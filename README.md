# Shadowrift Chronicles

**Unity 2D/2.5D Action-Adventure** with seamless dimensional phasing, Voidblade combat, Luna Loyalty system, and Meta Quest VR support.

---

## Features

- **Dimensional Phasing** – Instantly switch between two reality layers
- **Voidblade Stances** – Ravager, Sentinel, Harmonist
- **Luna Loyalty System** – Choices affect the world and determine the ending
- **5 Realms** + Reality Storm final boss
- **Multiple Enemy Types** – Aggressive, Ranged, Loyalty-sensitive
- **Meta Quest Hand Tracking** – Pinch, swipe, and open palm gestures
- Full UI, Audio, Score, Health, Save/Load, and Ending systems

---

## Getting Started

### Requirements
- Unity **6000.3 LTS** or newer (2D URP template recommended)
- Meta Quest 2 / 3 / 3S (for hand tracking)

### Required Packages
- XR Plugin Management
- OpenXR
- XR Hands
- XR Interaction Toolkit
- Input System
- TextMeshPro
- Addressables (optional)
- Meta XR Core SDK (recommended for best Quest support)

### Setup Steps
1. Clone this repository
2. Open the project in Unity Hub
3. Install the required packages listed above
4. Open `Assets/Scenes/` and create the realm scenes if they don't exist yet
5. Set up a Canvas and assign UI references to `UIManager`
6. Build for Android and deploy to your Meta Quest

---

## Controls

### Keyboard / Gamepad
- **WASD / Left Stick** – Move
- **Space / A Button** – Phase Shift
- **1 / 2 / 3** – Switch Stances

### Meta Quest Hand Tracking
| Gesture                    | Action                     |
|---------------------------|----------------------------|
| Right Hand Pinch          | Voidblade Attack           |
| Left Hand Swipe Right     | Ravager Stance             |
| Left Hand Swipe Left      | Sentinel Stance            |
| Both Hands Open Palm      | Dimensional Phase Shift    |

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/           # GameManager, Phasing, Stance, Loyalty, Audio, Save, Realm
│   ├── Mechanics/      # Player, Enemies, Projectiles, Hand Tracking, Boss
│   └── UI/             # UIManager, MainMenu, Ending
├── Scenes/             # Realm scenes (create these)
└── Art/                # Place your sprites and effects here
```

---

## Current Systems Included

- GameManager
- PhasingManager
- StanceManager
- LoyaltyManager
- PlayerController + PlayerHealth
- QuestHandGestureManager (full hand tracking)
- AggressiveEnemy / RangedEnemy / LoyaltyEnemy
- Projectile system
- EnemyDeathHandler + ScoreManager
- RealityStormBoss
- RealmLoader + RealmProgression
- AudioManager
- UIManager
- MainMenuController
- EndingManager
- SaveSystem

---

## License

This project was built collaboratively as a full Game Design Document implementation. Feel free to use and expand it.

**Created for the Shadowrift Chronicles GDD.**
