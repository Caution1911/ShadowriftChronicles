# Unity Art Wiring Guide — Shadowrift Chronicles

How to connect downloaded concept art into the existing project structure.

---

## 1. Folder Setup

Create under `Assets/`:

```
Art/
  Characters/Zara/
  Characters/Luna/
  Weapons/
  Realms/
  Enemies/
  UI/
  VFX/
  Marketing/
```

Drop PNGs using names from `Docs/ART_ASSET_LIST.md`.

---

## 2. Import Settings (2D / URP)

For each gameplay sprite:

1. Select texture
2. **Texture Type:** Sprite (2D and UI)
3. **Pixels Per Unit:** 32 or 100 (stay consistent)
4. **Filter Mode:** Bilinear (or Point for crisp pixel look)
5. **Compression:** none for UI icons; normal for characters if needed
6. Apply

UI icons: enable **Alpha Is Transparency**.

---

## 3. Player (Zara)

1. Open sample Player prefab or scene Player object
2. `SpriteRenderer` → assign `zara_front` or `zara_hero` cropped sprite
3. Adjust scale so capsule collider matches silhouette
4. Optional: child object for Voidblade sprite (`voidblade.png`)

**Quest note:** In VR, Zara may be a first-person / tracked hands presentation; use the art as HUD portrait or menu model reference instead of world sprite if you go full VR avatar later.

---

## 4. Enemies

1. Open `AggressiveEnemy`, `RangedEnemy`, `LoyaltyEnemy` sample prefabs
2. Assign corresponding regions from `enemy_sheet.png` (or split sprites)
3. Match collider roughly to visible body
4. Tint optional: keep sheet colors (red / cyan / purple) for stance readability

Boss:
- `RealityStormBoss` → `reality_storm_boss.png`
- Scale up 2–3× vs normal enemies

---

## 5. Realms (backgrounds)

Per realm scene (`Realm_01_NeoArcadia`, etc.):

1. Create Quad or Sprite background
2. Assign realm image
3. Place behind gameplay layer (sorting order / Z)
4. Optional: second dimmer background for Phase layer with higher saturation material

**Phase layer tip:** Duplicate background, parent under Phase layer root, add slight hue shift or additive glow material when phase is active.

---

## 6. UI Wiring

| Element | Source art | Wire to |
|---------|------------|---------|
| Stance icons | `stance_icons.png` (slice) | Stance UI images |
| Phase icon | `system_icons.png` | Layer indicator |
| Health | system icons or solid fill | Health bar |
| Loyalty | `loyalty_meter.png` as reference | Loyalty fill image |
| Shards / score | system icons | HUD counters |

**Slicing:** Use Unity Sprite Editor to split icon sheets into multiple sprites.

`UIManager` fields should reference the Image components, not the raw textures.

---

## 7. Marketing-Only Assets

Do **not** put logo/key art in gameplay atlases.

Use for:
- Main Menu title image
- Boot splash
- GitHub / store pages (export from `Art/Marketing/`)

Main Menu:
1. Canvas → Image full screen or logo image
2. Assign `logo_shadowrift_chronicles.png` or `keyart_banner.png`

---

## 8. VFX Reference

`phase_transition.png` is a **look target**, not a flipbook.

Build actual effect with:
- Particle System (burst + residual)
- Short chromatic post (URP Renderer Feature or volume)
- Optional full-screen flash 1–2 frames

Time total effect to **~0.18s** to match design docs.

---

## 9. Quick Validation

- [ ] Player readable at gameplay zoom
- [ ] Enemies distinct by silhouette/color
- [ ] Stance icons readable on Quest
- [ ] Phase vs Base background distinguishable
- [ ] No pink missing-texture materials
- [ ] Menu shows logo or key art

---

## 10. Sample Prefab Path

If you used **Shadowrift → First Run Setup Wizard**:

1. Generate sample prefabs
2. Replace their SpriteRenderer sprites with real art
3. Re-assign prefabs on EnemySpawner in each realm

---

*Art only ships when it’s wired. Wire the slice first.*
