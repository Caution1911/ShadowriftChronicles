using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip[] realmMusic;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip phaseSound;
    public AudioClip attackSound;
    public AudioClip enemyDeathSound;
    public AudioClip playerHurtSound;
    public AudioClip stanceSwitchSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlayMusic(int realmIndex)
    {
        if (realmMusic == null || realmIndex >= realmMusic.Length) return;
        musicSource.clip = realmMusic[realmIndex];
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlayPhase() => PlaySFX(phaseSound);
    public void PlayAttack() => PlaySFX(attackSound);
    public void PlayEnemyDeath() => PlaySFX(enemyDeathSound);
    public void PlayPlayerHurt() => PlaySFX(playerHurtSound);
    public void PlayStanceSwitch() => PlaySFX(stanceSwitchSound);
}
