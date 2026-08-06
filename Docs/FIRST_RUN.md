# First Run Guide – Shadowrift Chronicles

Follow these steps the first time you open the project.

---

## Option A: Automatic Wizard (Recommended)

1. Open the project in Unity and wait for scripts to compile
2. Go to the top menu:

   **Shadowrift → First Run Setup Wizard**

3. Follow the steps:
   - Welcome
   - Check Packages
   - Generate Scenes
   - Create Sample Prefabs
   - Finish

---

## Option B: Manual Steps

1. **Install Packages**
   - Window → Package Manager
   - Install: Input System, XR Plugin Management, OpenXR, XR Hands, XR Interaction Toolkit, TextMeshPro

2. **Validate Setup**
   - Shadowrift → Validate Project Setup

3. **Generate Scenes**
   - Shadowrift → Generate All Scenes (Full Setup)

4. **Create Sample Prefabs**
   - Shadowrift → First Run Setup Wizard → Step 4
   - Or run the Sample Prefab Generator from the wizard

5. **Test**
   - Open `Realm_01_NeoArcadia`
   - Assign sample enemy prefabs to the EnemySpawner
   - Press Play

---

## Controls for Testing

- **WASD** – Move
- **Space** – Phase Shift
- **1 / 2 / 3** – Change Stance

---

## Next Steps After First Run

- Replace placeholder sprites with real art
- Assign audio clips in AudioManager
- Wire UI references in UIManager
- Build to Meta Quest to test hand tracking
