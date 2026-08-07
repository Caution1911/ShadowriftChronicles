# AudioManager — Placeholder Clip Names

Use these exact field / file names so wiring stays consistent.

## Folder Layout

```
Assets/Audio/
  Music/
    music_menu.wav
    music_neo_arcadia.wav
    music_wildlands.wav
    music_clockwork.wav
    music_bone_desert.wav
    music_crystal_sanctum.wav
    music_reality_storm.wav
  SFX/
    sfx_phase.wav
    sfx_attack_ravager.wav
    sfx_attack_sentinel.wav
    sfx_attack_harmonist.wav
    sfx_stance_switch.wav
    sfx_enemy_hit.wav
    sfx_enemy_death.wav
    sfx_player_hurt.wav
    sfx_shard_pickup.wav
    sfx_loyalty_up.wav
    sfx_loyalty_down.wav
    sfx_ui_confirm.wav
    sfx_ui_back.wav
    sfx_boss_telegraph.wav
```

## AudioManager Serialized Fields

```csharp
[Header("Music")]
public AudioClip musicMenu;
public AudioClip musicNeoArcadia;
public AudioClip musicWildlands;
public AudioClip musicClockwork;
public AudioClip musicBoneDesert;
public AudioClip musicCrystalSanctum;
public AudioClip musicRealityStorm;

[Header("SFX")]
public AudioClip sfxPhase;
public AudioClip sfxAttackRavager;
public AudioClip sfxAttackSentinel;
public AudioClip sfxAttackHarmonist;
public AudioClip sfxStanceSwitch;
public AudioClip sfxEnemyHit;
public AudioClip sfxEnemyDeath;
public AudioClip sfxPlayerHurt;
public AudioClip sfxShardPickup;
public AudioClip sfxLoyaltyUp;
public AudioClip sfxLoyaltyDown;
public AudioClip sfxUiConfirm;
public AudioClip sfxUiBack;
public AudioClip sfxBossTelegraph;
```

## Priority to fill first

1. `sfx_phase`
2. `sfx_attack_*` (3)
3. `sfx_stance_switch`
4. `sfx_enemy_death`
5. `music_neo_arcadia` + `music_menu`
