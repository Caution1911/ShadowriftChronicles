# Unity Integration Guide – Shadowrift Chronicles

This document ensures the project is properly integrated with Unity and ready to run.

---

## 1. Recommended Unity Version

- **Unity 6000.3 LTS** (or newer LTS)
- Template: **2D (URP)** when creating the project

---

## 2. Required Packages

Install these via **Window → Package Manager**:

| Package                      | Minimum Version | Purpose                              |
|-----------------------------|-----------------|--------------------------------------|
| Input System                | 1.7+            | Modern input handling                |
| XR Plugin Management        | 4.4+            | XR foundation                        |
| OpenXR Plugin               | 1.9+            | Cross-platform XR                    |
| XR Hands                    | 1.3+            | Hand tracking                        |
| XR Interaction Toolkit      | 2.5+            | Interactions & locomotion            |
| TextMeshPro                  | 3.0+            | UI text                              |
| Universal RP                | 17.0+           | Rendering (if using URP)             |
| Meta XR Core SDK (optional) | Latest          | Best Meta Quest support              |

### How to Install
1. Open **Window → Package Manager**
2. Set source to **Unity Registry**
3. Search and install each package listed above
4. For Meta XR: use the official Meta XR SDK installer or scoped registry if available

---

## 3. Project Settings Checklist

### Player Settings (Edit → Project Settings → Player)
- **Company Name**: Your name / studio
- **Product Name**: Shadowrift Chronicles
- **Package Name** (Android): `com.yourname.shadowriftchronicles`
- **Minimum API Level**: 29 or higher
- **Scripting Backend**: IL2CPP
- **Target Architectures**: ARM64

### XR Plug-in Management
1. Go to **Edit → Project Settings → XR Plug-in Management**
2. Enable **OpenXR**
3. Under OpenXR → Feature Groups, enable **Meta Quest Support**
4. Enable **Hand Tracking** if the option appears

### Input System
- Set **Active Input Handling** to **Input System Package** (or Both)

---

## 4. First-Time Setup Order

1. Clone the repository
2. Open in Unity Hub
3. Let Unity import and compile scripts
4. Install all required packages listed above
5. Run the package validator (see below)
6. Go to **Shadowrift → Generate All Scenes (Full Setup)**
7. Open `Realm_01_NeoArcadia` and press Play to test

---

## 5. Scene Integration

The Scene Generator automatically:
- Creates all required scenes
- Attaches scripts
- Wires key references (PhasingManager layers, GameManager links)
- Adds scenes to Build Settings

After generation you only need to:
- Assign UI references in the UIManager component
- Create and assign enemy / projectile prefabs
- Assign audio clips in AudioManager

---

## 6. Meta Quest Build Integration

1. Switch Platform to **Android**
2. In Player Settings set:
   - Minimum API Level = 29+
   - Scripting Backend = IL2CPP
   - Target Architectures = ARM64
3. Connect your Quest via Link or USB
4. **Build and Run**

Hand tracking requires actual Quest hardware for full testing.

---

## 7. Common Integration Issues

| Issue                          | Solution                                      |
|--------------------------------|-----------------------------------------------|
| Scripts missing                | Wait for compilation, then re-run generator   |
| Hand tracking not working      | Enable OpenXR + Meta Quest Support + XR Hands |
| Input not responding           | Set Active Input Handling to Input System     |
| UI text missing                | Ensure TextMeshPro is imported (or use legacy Text) |
| Layers not switching           | Check that BaseLayer & PhaseLayer are assigned |

---

## 8. Validation Tool

Use the built-in validator:

**Shadowrift → Validate Project Setup**

This checks for missing packages and common configuration problems.
