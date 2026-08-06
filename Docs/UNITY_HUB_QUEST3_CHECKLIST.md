# Unity Hub + Meta Quest 3 — Master Checklist

Use this when preparing a clean machine or a new clone.

## Unity Hub
- [ ] Unity Hub installed and logged in
- [ ] Editor **6000.3 LTS** (or current LTS) installed
- [ ] Modules: Android Build Support, OpenJDK, SDK & NDK
- [ ] Project added via **Open** / **Add** from this repository folder

## Packages
- [ ] Input System
- [ ] XR Plugin Management
- [ ] OpenXR
- [ ] XR Hands
- [ ] XR Interaction Toolkit
- [ ] TextMeshPro essentials imported
- [ ] Meta XR SDK (recommended)

## Project Settings
- [ ] XR Plug-in Management → Android → OpenXR enabled
- [ ] Meta Quest Support feature enabled
- [ ] Hand Tracking enabled where available
- [ ] Active Input Handling = Input System Package (or Both)
- [ ] Android: IL2CPP, ARM64, Vulkan, Min API 29+

## Shadowrift Tools
- [ ] **Shadowrift → Validate Project Setup** passes
- [ ] **Shadowrift → First Run Setup Wizard** completed
- [ ] **Shadowrift → Generate All Scenes** run once
- [ ] Sample prefabs created and assigned to EnemySpawner

## Quest 3 Device
- [ ] Developer Mode on
- [ ] Headset connected (USB or Link)
- [ ] Build and Run succeeds
- [ ] Hand tracking visible in headset
- [ ] Pinch attack works
- [ ] Swipe stance switch works
- [ ] Open-palm phase works
- [ ] Thumbs-up Harmonist works

## Performance Smoke Test (Quest 3)
- [ ] Stable frame feel in a realm scene
- [ ] Phase spam does not tank FPS
- [ ] No pink materials / missing shaders on device

When all boxes are checked, the project is **Unity Hub + Quest 3 ready**.
