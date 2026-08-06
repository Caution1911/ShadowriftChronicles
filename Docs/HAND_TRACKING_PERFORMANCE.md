# Hand Tracking Performance — Meta Quest 3

Optimizations applied to `QuestHandGestureManager` for stable frame times on Quest 3.

## What was optimized

| Technique | Benefit |
|-----------|---------|
| Early-out if subsystem missing / not running | Avoids work when tracking is off |
| Attack (pinch) every frame; other gestures ~30 Hz | Keeps combat responsive, cuts secondary CPU cost |
| Squared-distance checks (no `Vector3.Distance` / `sqrt`) | Cheaper math |
| Cached `Pose` fields (no per-frame allocations) | Zero GC from gesture code in steady state |
| No `List<>` allocation in `Update` | Subsystem resolved once in `Start`/`OnEnable` |
| `debugLogs` default **off** | No string formatting cost in shipping builds |
| Cooldowns on all gestures | Prevents repeated work and input spam |

## Recommended Inspector values (Quest 3)

- `secondaryGestureInterval` = **0.033** (~30 Hz) — good balance
- For maximum responsiveness on swipe/phase: **0.016** (~60 Hz)
- For maximum savings on low-end thermal state: **0.05** (~20 Hz)
- Leave `debugLogs` **unchecked** on device builds

## Project-level tips (Quest 3)

1. **Player Settings → Android**
   - IL2CPP, ARM64, Vulkan
   - Multithreaded Rendering on
2. **XR**
   - OpenXR + Meta Quest Support
   - Hand Tracking enabled
   - Prefer Single Pass Instanced
3. **URP / Quality**
   - Medium quality tier for Quest
   - Fixed Foveated Rendering when available
   - Keep phase VFX short and lightweight (already 0.18s transition)
4. **Do not** poll `SubsystemManager.GetSubsystems` every frame
5. Test on **device** — Editor/Link hand data is not a performance reference

## Profiling

On device, use:
- Meta Quest Developer Hub performance HUD
- Unity Profiler over Wi-Fi / Link
- Look for `QuestHandGestureManager.Update` staying well under 0.2 ms when idle

## Gesture map (unchanged)

| Gesture | Action |
|---------|--------|
| Right pinch | Attack |
| Left swipe | Ravager / Sentinel |
| Both open palms (hold) | Phase |
| Right thumbs up | Harmonist |
