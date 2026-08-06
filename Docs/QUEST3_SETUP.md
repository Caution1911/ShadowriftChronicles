# Meta Quest 3 + Unity Hub Setup — Shadowrift Chronicles

This guide makes the project **production-ready for Meta Quest 3** with reliable hand tracking and gestures.

---

## 1. Create / Open Project in Unity Hub

1. Open **Unity Hub**
2. Install **Unity 6000.3 LTS** (or newer LTS)
3. Add modules:
   - **Android Build Support**
   - **OpenJDK**
   - **Android SDK & NDK Tools**
4. Open this repository as an existing project (or create a 2D URP project and copy the `Assets` folder in)

**Recommended template when starting fresh:** 2D (URP)

---

## 2. Required Packages

Install via **Window → Package Manager**:

| Package | Why |
|---------|-----|
| Input System | Modern input |
| XR Plugin Management | XR foundation |
| OpenXR Plugin | Quest support |
| XR Hands | Hand tracking API |
| XR Interaction Toolkit | Interactions |
| TextMeshPro | UI |
| Universal RP | Rendering |

**Strongly recommended for Quest 3:**
- **Meta XR All-in-One SDK** (from Meta / Package Manager scoped registry)

---

## 3. XR Project Settings (Critical for Quest 3)

### XR Plug-in Management
1. **Edit → Project Settings → XR Plug-in Management**
2. Android tab → enable **OpenXR**
3. **OpenXR → Feature Groups**
   - Enable **Meta Quest Support**
   - Enable **Hand Tracking** (if listed)
   - Enable **Meta Quest Touch Plus Controller Profile** (optional fallback)

### Player Settings (Android)
- **Package Name:** `com.yourstudio.shadowriftchronicles`
- **Minimum API Level:** 32 (Quest 3 recommended) or at least 29
- **Target API Level:** Automatic (highest installed)
- **Scripting Backend:** IL2CPP
- **Target Architectures:** ARM64 only
- **Graphics APIs:** Vulkan (remove OpenGL ES if present)
- **Multithreaded Rendering:** On

### Quality / Performance (Quest 3)
- Default quality: **Medium** or a custom Quest preset
- Enable **Fixed Foveated Rendering** when using URP + Quest features
- Keep particle counts modest during phase transitions
- Prefer Single Pass Instanced stereo rendering

---

## 4. Hand Tracking Gestures (Implemented)

Script: `Assets/Scripts/Mechanics/QuestHandGestureManager.cs`

| Gesture | Action |
|---------|--------|
| **Right Hand Pinch** | Voidblade Attack |
| **Left Hand Swipe Right** | Ravager Stance |
| **Left Hand Swipe Left** | Sentinel Stance |
| **Right Hand Thumbs Up** | Harmonist Stance |
| **Both Hands Open Palm** (hold ~0.25s) | Dimensional Phase Shift |

Thresholds are tuned for **Quest 3** hand tracking. Adjust in the Inspector if needed:
- `pinchThreshold` (default 0.028)
- `swipeDistanceThreshold` (default 0.16)
- `openPalmMinFingerDistance` (default 0.055)

---

## 5. Scene Setup for Hand Tracking

In every playable realm scene:

1. Ensure an **XR Origin** (XR Interaction Toolkit) exists  
   - Or Meta XR Camera Rig if using Meta SDK
2. Create empty GameObject → `HandGestureManager`
3. Add component **QuestHandGestureManager**
4. Make sure **GameManager** exists in the scene (or DontDestroyOnLoad from bootstrap)

Use **Shadowrift → First Run Setup Wizard** then **Generate All Scenes** to create hierarchy automatically.

---

## 6. Build & Run on Quest 3

1. Enable **Developer Mode** on the Quest 3 (Meta Quest app on phone)
2. Connect via USB-C or use **Meta Quest Link / Air Link** for iteration
3. In Unity: **File → Build Settings → Android → Switch Platform**
4. **Build and Run**

**Important:** Hand tracking quality is significantly better on **device** than in Editor / Link for some gestures. Always validate pinches and swipes on the headset.

---

## 7. Comfort & Stability (Quest 3)

- Phase transition is already short (0.18s) — keep it that way
- Add a subtle vignette during rapid phase spam if players report discomfort
- Avoid large camera shakes
- Snap-turn optional if you later add locomotion beyond the current 2D plane prototype

---

## 8. Troubleshooting Hand Tracking

| Problem | Fix |
|---------|-----|
| No hand subsystem | Install XR Hands + enable OpenXR Meta Quest Support |
| Pinch not registering | Lower `pinchThreshold` slightly (e.g. 0.032) |
| Accidental phases | Increase `openPalmHoldTime` or `phaseCooldown` |
| Swipes too sensitive | Raise `swipeDistanceThreshold` or `minSwipeSpeed` |
| Works in Editor, not on device | Rebuild with ARM64 + Vulkan; test on headset |

---

## 9. Unity Hub Checklist (Copy/Paste)

- [ ] Unity 6000.3 LTS + Android modules installed in Hub
- [ ] Project opens without compile errors
- [ ] Packages listed above installed
- [ ] OpenXR + Meta Quest Support enabled
- [ ] XR Hands package present
- [ ] Scenes generated via Shadowrift menu
- [ ] HandGestureManager in play scenes
- [ ] Android build settings (IL2CPP, ARM64, Vulkan)
- [ ] Tested on Quest 3 hardware

---

**Shadowrift is configured to be Unity Hub friendly and Quest 3 hand-tracking ready.**
