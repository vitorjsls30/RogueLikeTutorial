using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;
    public AudioSource efxSource;                   //Drag a reference to the AudioSource wich will play the sound effects
    public AudioSource musicSource;                 //Drag a reference to the AudioSource wich will play the music
    public float lowPitchRange = .95f;              //The lowest a sound effect will be ramdomly pitched
    public float hightPitchRange = 1.05f;           //The highest a sound effect will be ramdomly pitched

    void Start () {
	    //Check if there is already an instance of sound manager
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SOundManager
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}
	
    //Plays a single sound clips
	public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx (params AudioClip [] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, hightPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];

        efxSource.Play();
    }
}
