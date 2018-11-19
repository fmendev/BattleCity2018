using System;
using UnityEngine;

public enum SFX { IntroTankRolling, IntroTankFiring, MouseOnOption, StartGame, PlayerFire, ProjectileHitsBrick, ProjectileHitsWall, ExplosionEnemyRegular, PowerUpSpawn,
                  Target};

public class SoundManager : MonoBehaviour
{
    private static SoundManager singletonInstance = null;

    public AudioSource sfxSource;                   
    public AudioSource musicSource;

    public AudioClip[] music;

    //Intro and Main Menu
    public AudioClip introTankRolling;
    public AudioClip introTankFiring;
    public AudioClip mouseOnOption;
    public AudioClip startGame;

    //In game
    public AudioClip playerFire;
    public AudioClip projectileHitsBrick;
    public AudioClip projectileHitsWall;
    public AudioClip explosionEnemyRegular;
    public AudioClip powerUpSpawn;
    public AudioClip target;

    void Awake()
    {
        InitializaSingleton();
        DontDestroyOnLoad(gameObject);
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

    public static void PlayMusic(int levelNumber)
    {
        singletonInstance.sfxSource.clip = singletonInstance.music[levelNumber];
        singletonInstance.sfxSource.loop = true;
        singletonInstance.sfxSource.Play();
    }

    public static void PlaySfx(SFX sfx)
    {
        singletonInstance.sfxSource.PlayOneShot(singletonInstance.GetSFX(sfx));
    }

    private AudioClip GetSFX(SFX sfx)
    {
        if (sfx == SFX.IntroTankFiring) return singletonInstance.introTankFiring;
        else if (sfx == SFX.IntroTankRolling) return singletonInstance.introTankRolling;
        else if (sfx == SFX.MouseOnOption) return singletonInstance.mouseOnOption;
        else if (sfx == SFX.StartGame) return singletonInstance.startGame;
        else if (sfx == SFX.PlayerFire) return singletonInstance.playerFire;
        else if (sfx == SFX.ProjectileHitsBrick) return singletonInstance.projectileHitsBrick;
        else if (sfx == SFX.ProjectileHitsWall) return singletonInstance.projectileHitsWall;
        else if (sfx == SFX.ExplosionEnemyRegular) return singletonInstance.explosionEnemyRegular;
        else if (sfx == SFX.PowerUpSpawn) return singletonInstance.powerUpSpawn;
        else if (sfx == SFX.Target) return singletonInstance.target;

        else
            throw new Exception("SFX not found");
    }
}