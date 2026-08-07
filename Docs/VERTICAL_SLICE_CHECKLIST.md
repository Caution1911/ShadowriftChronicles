# Vertical Slice Checklist — Shadowrift Chronicles

**Goal:** A 10–15 minute playable slice that proves dimensional phasing, combat identity, and one realm’s fantasy.

**Recommended slice:** Neo-Arcadia district → combat encounters → phase puzzles → mini-boss or exit gate.

---

## 1. Scope Lock

- [ ] One realm only (Neo-Arcadia)
- [ ] One playable character (Zara)
- [ ] Three stances functional
- [ ] Phase shift functional (Base ↔ Phase)
- [ ] 2–3 enemy types max
- [ ] No full narrative; short objective text only
- [ ] No full save system required (optional checkpoint)
- [ ] Target platforms for demo: PC + Quest 3 (or PC only if hardware limited)

---

## 2. Core Gameplay

### Movement & Camera
- [ ] Move (WASD / stick)
- [ ] Camera stable and readable
- [ ] Collision with world bounds

### Phasing
- [ ] Toggle phase (keyboard + Quest open-palm)
- [ ] Visual difference between layers clear in <0.5s
- [ ] Transition ≤ ~0.2s
- [ ] At least one obstacle solvable only by phasing
- [ ] At least one enemy interaction that changes with layer

### Combat
- [ ] Attack (click / pinch)
- [ ] Ravager / Sentinel / Harmonist change damage or behavior
- [ ] Enemy aggro + death
- [ ] Player health + death / restart
- [ ] Hit feedback (flash, SFX, or both)

### Progression in slice
- [ ] Clear start objective
- [ ] Clear end condition (gate, boss defeat, or extract)
- [ ] Optional: score or shard pickup

---

## 3. Content

- [ ] Blockout complete for slice path (no softlock)
- [ ] 1 combat arena
- [ ] 1 exploration / phase moment
- [ ] 1 climax moment (elite enemy or small storm beat)
- [ ] Placeholder or final art on player + 2 enemies
- [ ] Realm background readable as Neo-Arcadia

---

## 4. UI / UX

- [ ] Health visible
- [ ] Active stance visible
- [ ] Current layer visible (Base / Phase)
- [ ] Objective text or marker
- [ ] Pause / restart
- [ ] Quest: gestures documented on a splash or pause card

---

## 5. Audio

- [ ] Phase SFX
- [ ] Attack SFX (at least one)
- [ ] Enemy death SFX
- [ ] Realm music loop (can be placeholder)
- [ ] UI click (optional)

---

## 6. Technical

- [ ] Scene loads from Main Menu or boot scene
- [ ] No console spam of hard errors
- [ ] 60 FPS target on PC; stable experience on Quest 3
- [ ] Hand tracking gestures work on device (if Quest build)
- [ ] Build ships without missing script references

---

## 7. Demo Polish Gate (ship slice only if true)

- [ ] New player can complete without developer help
- [ ] Phase feels good (not laggy, not confusing)
- [ ] Combat not spongy or one-shotty
- [ ] One “wow” moment (phase through threat or dual-layer arena)
- [ ] Credits / title card with logo

---

## 8. Out of Scope for Vertical Slice

- Full Luna dialogue tree
- All five realms
- Full endings / loyalty arcs
- Co-op
- Full economy
- Final boss Reality Storm (optional teaser only)

---

## Exit Criteria

**Vertical slice is done when:** a cold player can start the build, understand phase + stance within 2 minutes, complete the path in 10–15 minutes, and say the phasing feels unique.

---

*Ship the feeling first. Expand content second.*
