using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central audio router for Shadowrift Chronicles.
/// Assign clips using names from Docs/AUDIO_CLIP_NAMES.md.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

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

    [Header("Volumes")]
    [Range(0f, 1f)] public float musicVolume = 0.6f;
    [Range(0f, 1f)] public float sfxVolume = 0.85f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    public void PlayMusicForScene(string sceneName)
    {
        AudioClip clip = musicMenu;

        if (sceneName.Contains("NeoArcadia") || sceneName.Contains("Realm_01"))
            clip = musicNeoArcadia;
        else if (sceneName.Contains("Wildlands") || sceneName.Contains("Realm_02"))
            clip = musicWildlands;
        else if (sceneName.Contains("Clockwork") || sceneName.Contains("Realm_03"))
            clip = musicClockwork;
        else if (sceneName.Contains("Bone") || sceneName.Contains("Realm_04"))
            clip = musicBoneDesert;
        else if (sceneName.Contains("Crystal") || sceneName.Contains("Realm_05"))
            clip = musicCrystalSanctum;
        else if (sceneName.Contains("Storm") || sceneName.Contains("Reality"))
            clip = musicRealityStorm;
        else if (sceneName.Contains("Menu"))
            clip = musicMenu;

        PlayMusic(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayPhase() => PlaySfx(sfxPhase);
    public void PlayStanceSwitch() => PlaySfx(sfxStanceSwitch);
    public void PlayEnemyHit() => PlaySfx(sfxEnemyHit);
    public void PlayEnemyDeath() => PlaySfx(sfxEnemyDeath);
    public void PlayPlayerHurt() => PlaySfx(sfxPlayerHurt);
    public void PlayShardPickup() => PlaySfx(sfxShardPickup);
    public void PlayLoyaltyUp() => PlaySfx(sfxLoyaltyUp);
    public void PlayLoyaltyDown() => PlaySfx(sfxLoyaltyDown);
    public void PlayUiConfirm() => PlaySfx(sfxUiConfirm);
    public void PlayUiBack() => PlaySfx(sfxUiBack);
    public void PlayBossTelegraph() => PlaySfx(sfxBossTelegraph);

    public void PlayAttack()
    {
        var stance = GameManager.Instance != null && GameManager.Instance.stanceManager != null
            ? GameManager.Instance.stanceManager.CurrentStance
            : Stance.Ravager;

        switch (stance)
        {
            case Stance.Sentinel:
                PlaySfx(sfxAttackSentinel != null ? sfxAttackSentinel : sfxAttackRavager);
                break;
            case Stance.Harmonist:
                PlaySfx(sfxAttackHarmonist != null ? sfxAttackHarmonist : sfxAttackRavager);
                break;
            default:
                PlaySfx(sfxAttackRavager);
                break;
        }
    }
}
