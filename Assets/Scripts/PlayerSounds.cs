using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    private AudioSource SFXSource;
    public AudioClip tankMovingSFX;
    public AudioClip tankIdleSFX;

    private void Awake ()
    {
        SFXSource = gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayIdleSFX();
    }

    private void PlayMovingSFX()
    {
        //SFXSource.volume = .75f;
        //SFXSource.clip = tankMovingSFX;
        //SFXSource.Play();
    }

    private void PlayIdleSFX()
    {
        //SFXSource.clip = tankIdleSFX;
        //SFXSource.volume = .75f;
        //SFXSource.Play();
    }
}
