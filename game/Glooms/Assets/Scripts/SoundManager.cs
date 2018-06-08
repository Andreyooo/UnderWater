using UnityEngine;
using System.Collections;


public class SoundManager : MonoBehaviour
{
    public static AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public static float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public static float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

    public static AudioClip arrowShotSound, arrowHitSound, ouchSound1, ouchSound2, ouchSound3, ouchSound4, jumpSound, deathSound, hmpfSound1, hmpfSound2, hmpfSound3, hmpfSound4;
    public static AudioClip[] ouch;
    public static AudioClip[] hmpf;
    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);


        arrowShotSound = Resources.Load<AudioClip>("PfeilSchuss");
        arrowHitSound = Resources.Load<AudioClip>("PfeilTreffer");
        ouchSound1 = Resources.Load<AudioClip>("ouch1");
        ouchSound2 = Resources.Load<AudioClip>("ouch2");
        ouchSound3 = Resources.Load<AudioClip>("ouch3");
        ouchSound4 = Resources.Load<AudioClip>("ouch4");
        hmpfSound1 = Resources.Load<AudioClip>("hmpf1");
        hmpfSound2 = Resources.Load<AudioClip>("hmpf2");
        hmpfSound3 = Resources.Load<AudioClip>("hmpf3");
        hmpfSound4 = Resources.Load<AudioClip>("hmpf4");
        deathSound = Resources.Load<AudioClip>("death");
        ouch = new AudioClip[] { ouchSound1, ouchSound2, ouchSound3, ouchSound4};
        hmpf = new AudioClip[] { hmpfSound1, hmpfSound2, hmpfSound3, hmpfSound4 };
        jumpSound = Resources.Load<AudioClip>("Jump");

        efxSource = GetComponent<AudioSource>();
    }


    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        efxSource.clip = clip;

        //Play the clip.
        efxSource.Play();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "arrowShot":
                efxSource.PlayOneShot(RandomizeSfx(arrowShotSound));
                break;
            case "arrowHit":
                efxSource.PlayOneShot(RandomizeSfx(arrowHitSound));
                break;
            case "jump":
                efxSource.PlayOneShot(RandomizeSfx(jumpSound));
                break;
            case "ouch":
                efxSource.PlayOneShot(RandomizeSfx(ouch));
                break;
            case "hmpf":
                efxSource.PlayOneShot(RandomizeSfx(hmpf));
                break;
            case "death":
                efxSource.PlayOneShot(deathSound);
                break;

        }
    }


    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public static AudioClip RandomizeSfx(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        efxSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        return clips[randomIndex];
    }
}