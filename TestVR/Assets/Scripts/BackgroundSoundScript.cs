using UnityEngine;
using System.Collections;

public class BackgroundSoundScript : MonoBehaviour {

    public AudioSource eyesOpenAudioSource;
    public float maxVolumeEyesOpen;

    public AudioSource eyesClosedAudioSource;
    public float maxVolumeEyesClosed;

    public Blink blinkScript;

    public void StartBackgroundSound()
    {
        eyesOpenAudioSource.Play();
        eyesClosedAudioSource.Play();
    }

    public void StopBackgroundSound()
    {
        eyesOpenAudioSource.Stop();
        eyesClosedAudioSource.Stop();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        eyesOpenAudioSource.volume = (1.0f - blinkScript.invisible)* (1.0f - blinkScript.invisible) * maxVolumeEyesOpen;
        eyesClosedAudioSource.volume = blinkScript.invisible * blinkScript.invisible * maxVolumeEyesClosed;
    }
}
