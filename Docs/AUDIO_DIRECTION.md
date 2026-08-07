# Shadowrift Chronicles — Audio Direction

Audio supports the core fantasy: **phasing feels instantaneous, combat feels sharp, loyalty feels human.**

---

## Design Pillars

1. **Phase is the sonic signature** — The most recognizable sound in the game
2. **Layers sound different** — Base vs Phase reality must be audible without looking
3. **Stance identity** — Each Voidblade stance has a distinct color *and* tone
4. **Loyalty is subtle** — Luna’s presence shifts with the meter, never noisy
5. **Quest 3 first** — Spatial, clear at moderate volume, no muddy low-end spam

---

## Music

| Context | Style | Notes |
|---------|-------|-------|
| Main Menu | Sparse, ethereal pads + distant pulse | Logo sting optional |
| Neo-Arcadia | Cyber-noir, mid-tempo synth, rain bed | Wet, reflective |
| Wildlands | Organic drones, soft percussion, choir pads | Alive, ancient |
| Clockwork | Industrial rhythm, metallic hits, ticking motifs | Precise, tense |
| Bone Desert | Sparse winds, dry percussion, low drones | Lonely, harsh |
| Crystal Sanctum | Crystal chimes, harmonic overtones, deep bass | Mystical, dangerous |
| Reality Storm | Full mix of all realms tearing together | Climax only |
| Low Loyalty | Slight dissonance / thinner arrangement | When meter critical |
| High Loyalty | Warmer pads, clearer melody | Subtle reward |

**Target:** Adaptive stems preferred (explore / combat / phase intensity).  
**Budget fallback:** One loop per realm + one combat overlay + phase stinger.

---

## Core SFX Map

| Event | Direction |
|-------|-----------|
| **Phase shift** | Short (0.12–0.20s), whoosh + digital tear + soft impact. Stereo width expands then snaps. Most important SFX. |
| **Attack — Ravager** | Aggressive, mid-high blade + grit |
| **Attack — Sentinel** | Cleaner, heavier impact, shield resonance |
| **Attack – Harmonist** | Balanced, harmonic overtone on hit |
| **Stance switch** | Color-coded one-shot (red/blue/purple filter) |
| **Enemy hit** | Layer-tinted; Phase hits slightly more “ethereal” |
| **Enemy death** | Short dissolve + shard chime |
| **Player hurt** | Tight body hit + brief UI tick |
| **Low health** | Soft heartbeat bed (very quiet) |
| **Shard pickup** | Bright crystal tick |
| **Loyalty up** | Soft chime, Luna motif fragment |
| **Loyalty down** | Thin, colder tick |
| **Boss telegraphs** | Clear rising tones — readability first |
| **UI confirm / back** | Minimal neon clicks |

---

## Spatial / Quest 3 Notes

- Phase SFX: slight head-related width, not extreme reverb
- Combat one-shots: dry-to-short room; avoid long tails in dense fights
- Music: stereo or first-order spatial; keep dialogue/UI non-spatial
- Master bus: leave headroom for phase spikes; limit harshly only on output
- Test at **moderate Quest volume** — if phase isn’t obvious, raise it before music

---

## Implementation (Unity)

| System | Hook |
|--------|------|
| `AudioManager` | Realm music crossfade, phase/attack/stance one-shots |
| `PhasingManager` | Fire phase SFX at transition start |
| `StanceManager` | Stance switch SFX on change |
| `LoyaltyManager` | Optional soft cues on threshold cross |
| `EnemyDeathHandler` | Death + score sting |
| UI | Button clicks only; no music duck on every click |

**Placeholder:** Use free synth one-shots until custom pack is ready. Name clips to match `AudioManager` fields.

---

## Priority Order (when budget is limited)

1. Phase shift SFX  
2. Three stance attacks + stance switch  
3. Enemy hit / death  
4. One music loop per realm  
5. Loyalty / UI / boss polish  

---

*Reality is a weapon. Use it wisely.*
