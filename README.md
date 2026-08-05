# Shadowrift Chronicles

**Unity 2D / 2.5D Action-Adventure Game**  
Seamless dimensional phasing • Voidblade combat • Luna Loyalty system • Meta Quest VR support

---

## Overview

Shadowrift Chronicles is a complete game prototype built from a full Game Design Document. Players control Zara Nightfall, a dimension-hopping mercenary who can phase between two overlapping realities while wielding the sentient Voidblade.

### Core Features
- **Dimensional Phasing** – Instantly switch between Base Reality and Phase Reality
- **Voidblade Stances** – Ravager (aggressive), Sentinel (defensive), Harmonist (balanced)
- **Luna Loyalty System** – Your actions affect Luna and determine one of three endings
- **5 Distinct Realms** + Reality Storm final boss
- **Multiple Enemy Types** with layer-based behavior
- **Meta Quest Hand Tracking** – Natural gesture controls
- Full UI, Audio, Health, Score, Save/Load, and Ending systems

---

## Quick Start

### Requirements
- Unity **6000.3 LTS** or newer
- Template: **2D (URP)** recommended
- Meta Quest 2 / 3 / 3S (for hand tracking)

### Required Packages
| Package                    | Purpose                        |
|---------------------------|--------------------------------|
| XR Plugin Management      | XR foundation                  |
| OpenXR                    | Cross-platform XR              |
| XR Hands                  | Hand tracking                  |
| XR Interaction Toolkit    | Interactions                   |
| Input System              | Modern input                   |
| TextMeshPro                | UI text                        |
| Meta XR Core SDK          | Best Quest support (optional)  |

### Installation
1. Clone the repository
2. Open the project in Unity Hub
3. Install the packages listed above via Package Manager
4. Follow the **Scene Setup Guide** (`Docs/SCENE_SETUP.md`)
5. Build for Android and deploy to your Meta Quest

---

## Controls

### Keyboard / Gamepad
- **WASD / Left Stick** – Move
- **Space / South Button** – Phase Shift
- **1 / 2 / 3** – Switch Stances (Ravager / Sentinel / Harmonist)

### Meta Quest Hand Tracking
| Gesture                  | Action                      |
|-------------------------|-----------------------------|
| Right Hand Pinch        | Voidblade Attack            |
| Left Hand Swipe Right   | Ravager Stance              |
| Left Hand Swipe Left    | Sentinel Stance             |
| Both Hands Open Palm    | Dimensional Phase Shift     |

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/               # GameManager, Phasing, Stance, Loyalty, Audio, Save, Realm systems
│   ├── Mechanics/          # Player, Enemies, Projectiles, Hand Tracking, Boss
│   └── UI/                 # UIManager, MainMenu, Ending
├── Scenes/                 # Create realm scenes here
├── Art/                    # Sprites, effects, UI art
└── Prefabs/                # Enemy and projectile prefabs
```

---

## Systems Included

- GameManager (singleton)
- PhasingManager
- StanceManager
- LoyaltyManager
- PlayerController + PlayerHealth
- QuestHandGestureManager (full hand tracking)
- AggressiveEnemy / RangedEnemy / LoyaltyEnemy
- Projectile system
- EnemyDeathHandler + ScoreManager
- RealityStormBoss + BossSpawner
- EnemySpawner
- RealmLoader + RealmProgression
- AudioManager
- UIManager
- MainMenuController
- EndingManager
- SaveSystem

---

## Documentation

- [Scene Setup Guide](Docs/SCENE_SETUP.md)
- [Hand Tracking Setup](Assets/Scripts/Mechanics/HandTrackingSetup.md)

---

## License

This project was created as a collaborative implementation of a full Game Design Document. You are free to use, modify, and expand it.

**Shadowrift Chronicles** – Reality is a weapon. Use it wisely.
