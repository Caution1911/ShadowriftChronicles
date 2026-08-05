# Development Notes – Shadowrift Chronicles

## Architecture Overview

The game uses a simple singleton-based architecture centered around `GameManager`.

### Key Singletons
- `GameManager` – Central access point
- `ScoreManager`
- `AudioManager`
- `UIManager`
- `RealmLoader`

### Layer System
Every combat entity has a `nativeLayer` (`"base"` or `"phase"`).  
Damage and aggression only occur when the player is on the same layer.

### Progression Flow
1. Player clears all enemies in a realm (`RealmProgression`)
2. Progress is saved via `SaveSystem`
3. `RealmLoader` loads the next scene
4. Final realm spawns the Reality Storm Boss
5. Boss defeat triggers one of three endings based on Luna Loyalty

---

## Recommended Next Steps

1. Create actual art assets (Zara sprites, realm backgrounds, particles)
2. Build the 6 scenes following `SCENE_SETUP.md`
3. Create enemy and projectile prefabs
4. Assign audio clips in the AudioManager
5. Polish hand tracking thresholds on real Quest hardware
6. Add more VFX for phasing and stance switching

---

## Known Limitations

- Hand tracking works best on actual Meta Quest hardware
- Open palm detection is simplified and can be improved with more joint checks
- No networking / co-op yet (planned in original GDD)
- Scenes and prefabs are not included (code only)
