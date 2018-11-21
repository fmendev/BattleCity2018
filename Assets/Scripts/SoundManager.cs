using System;
using System.Collections;
using UnityEngine;

public enum SFX { IntroTankRolling, IntroTankFiring, MouseOnOption, StartGame, PlayerFire, ProjectileHitsBrick, ProjectileHitsWall, ExplosionEnemyRegular, ExplosionEnemyArmored, ExplosionPlayer,
                  ExplosionEagle, SpawnPowerUp, SpawnEnemy, Target, InteractUI, DamageEnemy};

public enum Music { TwinCannons, InDeep, HowlingWind };

public class SoundManager : MonoBehaviour
{
    private static SoundManager singletonInstance = null;

    public AudioSource audioSource;                   

    //Music
    public AudioClip twinCannons;
    public AudioClip inDeep;
    public AudioClip howlingWind;

    //Intro and Main Menu
    public AudioClip introTankRolling;
    public AudioClip introTankFiring;
    public AudioClip mouseOnOption;

    //In game
    public AudioClip playerFire;
    public AudioClip projectileHitsBrick;
    public AudioClip projectileHitsWall;
    public AudioClip explosionEnemyRegular;
    public AudioClip explosionEnemyArmored;
    public AudioClip explosionPlayer;
    public AudioClip explosionEagle;
    public AudioClip target;
    public AudioClip interactUI;
    public AudioClip spawnEnemy;
    public AudioClip spawnPowerUp;
    public AudioClip damageEnemy;

    private float fadeOutTime = 3f;

    void Awake()
    {
        InitializaSingleton();
        DontDestroyOnLoad(gameObject);
        audioSource.volume = .8f;
        audioSource.volume = .8f;
    }

    private void InitializaSingleton()
    {
        if (singletonInstance == null)
        {
            singletonInstance = this;
        }
        else if (singletonInstance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void PlayMusic(Music music)
    {
        singletonInstance.StopAllCoroutines();
        singletonInstance.audioSource.Stop();
        singletonInstance.audioSource.volume = .8f;
        singletonInstance.audioSource.clip = singletonInstance.GetMusic(music);
        singletonInstance.audioSource.loop = true;
        singletonInstance.audioSource.Play();
    }

    public static void PlaySfx(SFX sfx)
    {
        if (sfx == SFX.ExplosionPlayer || sfx == SFX.ExplosionEnemyArmored || sfx == SFX.ExplosionEagle)
        {
            singletonInstance.audioSource.volume = 1f;
        }
        singletonInstance.audioSource.PlayOneShot(singletonInstance.GetSFX(sfx));
        singletonInstance.audioSource.volume = .8f;
    }

    public static void FadeOutMusic(float fadeOutTime)
    {
        singletonInstance.StartCoroutine(singletonInstance.BeginFadingOutMusic(fadeOutTime));
    }

    private IEnumerator BeginFadingOutMusic(float fadeOutTime)
    {
        float startVolume = audioSource.volume;

        if (audioSource.isPlaying)
        {
            
            while (audioSource.volume > 0)
            {
                Debug.Log("Current volume: " + audioSource.volume);
                audioSource.volume -= startVolume * Time.deltaTime / fadeOutTime;
                yield return null;
            }
            audioSource.Stop();
        }

        audioSource.volume = startVolume;
    }

    public static void FadeInMusic(Music music, float fadeInTime)
    {
        singletonInstance.StartCoroutine(singletonInstance.BeginFadingInMusic(music, fadeInTime));
    }

    private IEnumerator BeginFadingInMusic(Music music, float fadeInTime)
    {
        singletonInstance.audioSource.clip = singletonInstance.GetMusic(music);

        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / fadeInTime;
            yield return null;
        }
    }

    public static void SetMusicVolume(float volume)
    {
        singletonInstance.audioSource.volume = volume;
    }

    private AudioClip GetMusic(Music music)
    {
        if (music == Music.TwinCannons) return singletonInstance.twinCannons;
        else if (music == Music.InDeep) return singletonInstance.inDeep;
        else if (music == Music.HowlingWind) return singletonInstance.howlingWind;

        else
            throw new Exception("Music file not found");
    }

    private AudioClip GetSFX(SFX sfx)
    {
        if (sfx == SFX.IntroTankFiring) return singletonInstance.introTankFiring;
        else if (sfx == SFX.IntroTankRolling) return singletonInstance.introTankRolling;
        else if (sfx == SFX.MouseOnOption) return singletonInstance.mouseOnOption;
        else if (sfx == SFX.PlayerFire) return singletonInstance.playerFire;
        else if (sfx == SFX.ProjectileHitsBrick) return singletonInstance.projectileHitsBrick;
        else if (sfx == SFX.ProjectileHitsWall) return singletonInstance.projectileHitsWall;
        else if (sfx == SFX.ExplosionEnemyRegular) return singletonInstance.explosionEnemyRegular;
        else if (sfx == SFX.ExplosionEnemyArmored) return singletonInstance.explosionEnemyArmored;
        else if (sfx == SFX.ExplosionPlayer) return singletonInstance.explosionPlayer;
        else if (sfx == SFX.ExplosionEagle) return singletonInstance.explosionEagle;
        else if (sfx == SFX.SpawnPowerUp) return singletonInstance.spawnPowerUp;
        else if (sfx == SFX.SpawnEnemy) return singletonInstance.spawnEnemy;
        else if (sfx == SFX.Target) return singletonInstance.target;
        else if (sfx == SFX.InteractUI) return singletonInstance.interactUI;
        else if (sfx == SFX.InteractUI) return singletonInstance.interactUI;

        else
            throw new Exception("SFX not found");
    }
}