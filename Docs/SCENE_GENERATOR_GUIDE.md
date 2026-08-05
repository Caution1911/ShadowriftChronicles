# Scene Generator Guide

This project includes an **automatic Scene Generator** that creates all required scenes for you.

## How to Use

1. Open the project in Unity
2. Wait for scripts to compile
3. In the top menu bar, click:

   **Shadowrift → Generate All Scenes**

4. Confirm the dialog

The tool will automatically create:

- `MainMenu`
- `Realm_01_NeoArcadia`
- `Realm_02_Wildlands`
- `Realm_03_Clockwork`
- `Realm_04_BoneDesert`
- `Realm_05_CrystalSanctum`
- `Realm_06_RealityStorm`

It will also add all scenes to **Build Settings**.

## What Gets Created in Each Realm Scene

- Main Camera (with unique background color per realm)
- `GameSystems` empty object (ready for your managers)
- `BaseLayer` and `PhaseLayer`
- Player object (tagged "Player") with Rigidbody2D + Collider
- EnemySpawner with 6 spawn points
- Canvas with basic UI text (Loyalty, Score, Health, Realm name)
- EventSystem
- HandGestureManager (for Meta Quest)
- BossSpawner (only in the final Reality Storm scene)

## After Generation

You still need to:

1. Attach the actual scripts (`GameManager`, `PhasingManager`, etc.) to the GameSystems object
2. Assign references in the Inspectors
3. Create and assign enemy prefabs to the EnemySpawner
4. Create a proper Player prefab with sprites
5. Improve the UI (replace the placeholder Text components with TextMeshPro if desired)

## Notes

- The generator uses the built-in Unity UI (Text + Image) so it works without extra packages
- You can run the generator multiple times safely (it overwrites the scenes)
