using System;
using UnityEngine;

public enum SFX { introTankRolling, introTankFiring, mouseOnOption, startGame, playerFire, bulletHitsBrick, bulletHitsWall, tankExplosion, powerUpSpawn };

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
    public AudioClip bulletHitsBrick;
    public AudioClip bulletHitsWall;
    public AudioClip tankExplosion;
    public AudioClip powerUpSpawn;

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

    public static void PlaySfx(AudioClip clip)
    {
        singletonInstance.sfxSource.PlayOneShot(clip);
    }

    public static AudioClip GetSFX(SFX sfx)
    {
        if (sfx == SFX.introTankFiring) return singletonInstance.introTankFiring;
        else if (sfx == SFX.introTankRolling) return singletonInstance.introTankRolling;
        else if (sfx == SFX.mouseOnOption) return singletonInstance.mouseOnOption;
        else if (sfx == SFX.startGame) return singletonInstance.startGame;
        else if (sfx == SFX.playerFire) return singletonInstance.playerFire;
        else if (sfx == SFX.bulletHitsBrick) return singletonInstance.bulletHitsBrick;
        else if (sfx == SFX.bulletHitsWall) return singletonInstance.bulletHitsWall;
        else if (sfx == SFX.tankExplosion) return singletonInstance.tankExplosion;
        else if (sfx == SFX.powerUpSpawn) return singletonInstance.powerUpSpawn;

        else
            throw new Exception("SFX not found");
    }
}