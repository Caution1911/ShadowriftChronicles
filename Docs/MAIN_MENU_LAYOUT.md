# Main Menu Scene Layout — Shadowrift Chronicles

## Hierarchy

```
MainMenu (scene)
├─ MainMenu_Systems
│   ├─ AudioManager
│   └─ MainMenuController
├─ Main Camera
└─ Canvas (Screen Space Overlay)
    ├─ BackgroundImage          ← keyart_banner or keyart_portrait
    ├─ LogoImage               ← logo_shadowrift_chronicles
    ├─ MainPanel
    │   ├─ Button_Play
    │   ├─ Button_Controls
    │   ├─ Button_Credits
    │   └─ Button_Quit
    ├─ ControlsPanel (disabled by default)
    │   ├─ ControlsText
    │   └─ Button_Back
    └─ CreditsPanel (disabled by default)
        ├─ CreditsText
        └─ Button_Back
```

## Button Wiring

| Button | OnClick |
|--------|---------|
| Play | `MainMenuController.OnPlayPressed` |
| Controls | `MainMenuController.OnControlsPressed` |
| Credits | `MainMenuController.OnCreditsPressed` |
| Quit | `MainMenuController.OnQuitPressed` |
| Back | `MainMenuController.OnBackPressed` |

## Controller Fields

- `firstRealmScene` = `Realm_01_NeoArcadia`
- Drag **MainPanel**, **ControlsPanel**, **CreditsPanel** into the matching slots

## Controls Text (copy/paste)

```
KEYBOARD
WASD — Move
Space — Phase Shift
1 / 2 / 3 — Ravager / Sentinel / Harmonist
LMB — Attack

META QUEST 3
Right Pinch — Attack
Left Swipe — Change Stance
Both Open Palms — Phase Shift
Right Thumbs Up — Harmonist
```

## Credits Text (copy/paste)

```
SHADOWRIFT CHRONICLES
Prototype / Vertical Slice

Design & Code — Caution1911
Engine — Unity
Target — PC + Meta Quest 3

Reality is a weapon. Use it wisely.
```

## Build Settings Order

0. `MainMenu`
1. `Realm_01_NeoArcadia`
2. (other realms later)

## Quest Notes

- Prefer **World Space** canvas only if you add a full VR lobby later
- For flat / Link testing, Screen Space Overlay is fine
- Keep buttons large for laser/hand poke
