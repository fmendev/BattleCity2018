using System;
using UnityEngine;

public enum SFX { IntroTankRolling, IntroTankFiring, MouseOnOption, StartGame, PlayerFire, ProjectileHitsBrick, ProjectileHitsWall, ExplosionEnemyRegular, ExplosionEnemyArmored, ExplosionPlayer,
                  ExplosionEagle, SpawnPowerUp, SpawnEnemy, Target, InteractUI};

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
    public AudioClip explosionEnemyArmored;
    public AudioClip explosionPlayer;
    public AudioClip explosionEagle;
    public AudioClip target;
    public AudioClip interactUI;
    public AudioClip spawnEnemy;
    public AudioClip spawnPowerUp;

    void Awake()
    {
        InitializaSingleton();
        DontDestroyOnLoad(gameObject);
        sfxSource.volume = .8f;
        musicSource.volume = .8f;
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
        if (sfx == SFX.ExplosionPlayer || sfx == SFX.ExplosionEnemyArmored || sfx == SFX.ExplosionEagle)
        {
            singletonInstance.sfxSource.volume = 1f;
        }
        singletonInstance.sfxSource.PlayOneShot(singletonInstance.GetSFX(sfx));
        singletonInstance.sfxSource.volume = .8f;
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
        else if (sfx == SFX.ExplosionEnemyArmored) return singletonInstance.explosionEnemyArmored;
        else if (sfx == SFX.ExplosionPlayer) return singletonInstance.explosionPlayer;
        else if (sfx == SFX.ExplosionEagle) return singletonInstance.explosionEagle;
        else if (sfx == SFX.SpawnPowerUp) return singletonInstance.spawnPowerUp;
        else if (sfx == SFX.SpawnEnemy) return singletonInstance.spawnEnemy;
        else if (sfx == SFX.Target) return singletonInstance.target;
        else if (sfx == SFX.InteractUI) return singletonInstance.interactUI;

        else
            throw new Exception("SFX not found");
    }
}