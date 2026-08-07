# Shadowrift Chronicles — Art Asset Catalog

Reference catalog of concept art generated for production, marketing, and Unity integration.
Download images from the design sessions and place them using the suggested paths below.

---

## Suggested Unity Folder Layout

```
Assets/Art/
├── Characters/
│   ├── Zara/
│   │   ├── zara_hero.png
│   │   ├── zara_front.png
│   │   ├── zara_side.png
│   │   └── zara_back.png
│   └── Luna/
│       └── luna_brightforge.png
├── Weapons/
│   └── voidblade.png
├── Realms/
│   ├── neo_arcadia.png
│   ├── wildlands.png
│   ├── clockwork_citadel.png
│   ├── bone_desert.png
│   └── crystal_sanctum.png
├── Enemies/
│   ├── enemy_sheet.png
│   └── reality_storm_boss.png
├── UI/
│   ├── loyalty_meter.png
│   ├── stance_icons.png
│   └── system_icons.png
├── VFX/
│   └── phase_transition.png
└── Marketing/
    ├── logo_shadowrift_chronicles.png
    ├── keyart_portrait.png
    ├── keyart_banner.png
    ├── capsule_quest_square.png
    ├── capsule_landscape.png
    ├── readme_hero.png
    └── readme_secondary.png
```

---

## Character Art

| Asset | Description | Suggested Filename |
|-------|-------------|--------------------|
| Zara Hero | Main promotional pose with Voidblade | `zara_hero.png` |
| Zara Front | Turnaround front | `zara_front.png` |
| Zara Side | Turnaround side | `zara_side.png` |
| Zara Back | Turnaround back with cyan X-plate | `zara_back.png` |
| Luna Brightforge | Ethereal companion, loyalty theme | `luna_brightforge.png` |

**Style notes:** Neon-arcane cyan/magenta, athletic silhouette, readable at gameplay scale.

---

## Weapons

| Asset | Description | Suggested Filename |
|-------|-------------|--------------------|
| Voidblade | Living energy sword, violet-cyan | `voidblade.png` |

---

## Realms

| Realm | Palette | Suggested Filename |
|-------|---------|--------------------|
| Neo-Arcadia | Deep blue, magenta neon, rain | `neo_arcadia.png` |
| Wildlands | Emerald, bioluminescent teal | `wildlands.png` |
| Clockwork Citadel | Brass, amber, steel | `clockwork_citadel.png` |
| Bone Desert | Bleached bone, rust, sand | `bone_desert.png` |
| Crystal Sanctum | Violet, cyan crystals | `crystal_sanctum.png` |

**Layer rule:** Base Reality = more grounded/desaturated; Phase Reality = higher saturation, glow edges, subtle glitch.

---

## Enemies & Boss

| Asset | Description | Suggested Filename |
|-------|-------------|--------------------|
| Enemy Sheet | Phasing Stalker, Crystal Wraith, Loyalty Shade | `enemy_sheet.png` |
| Reality Storm Boss | Colossal dual-layer storm entity | `reality_storm_boss.png` |

---

## UI

| Asset | Description | Suggested Filename |
|-------|-------------|--------------------|
| Loyalty Meter | Luna Loyalty gauge mockup | `loyalty_meter.png` |
| Stance Icons | Ravager (red), Sentinel (blue), Harmonist (purple) | `stance_icons.png` |
| System Icons | Phase layers, shard, health, score | `system_icons.png` |

---

## VFX

| Asset | Description | Suggested Filename |
|-------|-------------|--------------------|
| Phase Transition | Mid-shift rings, chromatic burst (~0.18s feel) | `phase_transition.png` |

---

## Marketing & Store

| Asset | Use | Suggested Filename |
|-------|-----|--------------------|
| Logo | Title screen, splash, docs | `logo_shadowrift_chronicles.png` |
| Portrait Key Art | Store feature, posters | `keyart_portrait.png` |
| Wide Banner | Steam header, web | `keyart_banner.png` |
| Quest Square Capsule | Meta Quest store icon | `capsule_quest_square.png` |
| Landscape Capsule | Quest/Steam landscape | `capsule_landscape.png` |
| README Hero | GitHub social / header | `readme_hero.png` |
| README Secondary | Minimal repo banner | `readme_secondary.png` |

---

## Color Language (quick reference)

| Element | Colors |
|---------|--------|
| Brand | Cyan `#00E5FF`, Violet `#A855F7`, Magenta `#FF2D95` |
| Ravager | Red / orange neon |
| Sentinel | Blue neon |
| Harmonist | Purple neon |
| UI text | White / soft cyan |
| Backgrounds | Near-black `#0A0A12` |

---

## Integration Tips

1. Import textures as **Sprites** for 2D UI and character placeholders.
2. For Quest UI, keep icons high-contrast and readable at ~64–128 px.
3. Use realm images as skybox references or background sprites in each `Realm_0X` scene.
4. Phase VFX still is a **timing/color reference** for particle systems — not a final flipbook.
5. Replace sample prefab colors in `Assets/Prefabs/Sample/` with real sprites when ready.

---

## Status

| Category | Status |
|----------|--------|
| Concept characters | Complete |
| Realms (5) | Complete |
| Enemies + boss | Complete |
| UI icons + loyalty | Complete |
| Logo + store art | Complete |
| In-engine final art | Pending (artist production) |

*Reality is a weapon. Use it wisely.*
