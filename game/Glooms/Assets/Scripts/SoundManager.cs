using UnityEngine;
using System.Collections;


public class SoundManager : MonoBehaviour
{
    public static AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public static float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public static float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

    public static AudioClip rocket, explosion;
    public static AudioClip arrowShotSound, arrowHitSound, ouchSound1, ouchSound2, ouchSound3, ouchSound4, jumpSound, deathSound, hmpfSound1, hmpfSound2, hmpfSound3, hmpfSound4;
    //Bandit Sounds
    public static AudioClip banditHitSound1, banditHitSound2, banditMissSound1, banditMissSound2, banditStartSound1, banditStartSound2, banditDeathSound, banditJumpSound1, banditJumpSound2, banditLevelUpSound;
    //Nerd Sounds
    public static AudioClip nerdHitSound1, nerdHitSound2, nerdMissSound1, nerdMissSound2, nerdStartSound1, nerdStartSound2, nerdDeathSound, nerdJumpSound1, nerdJumpSound2, nerdLevelUpSound;
    //Viking Sounds
    public static AudioClip vikingHitSound1, vikingHitSound2, vikingHitSound3, vikingHitSound4, vikingJumpSound1, vikingJumpSound2, vikingDeathSound, vikingJoySound1, vikingJoySound2, vikingMissSound1, vikingMissSound2, vikingLevelUpSound, vikingStartSound1, vikingStartSound2;
    public static AudioClip[] ouch;
    public static AudioClip[] hmpf;
    public static AudioClip[] vikingHit;
    public static AudioClip[] vikingJump;
    public static AudioClip[] vikingStart;
    public static AudioClip[] vikingJoy;
    public static AudioClip[] vikingMiss;
    public static AudioClip[] banditHit;
    public static AudioClip[] banditMiss;
    public static AudioClip[] banditStart;
    public static AudioClip[] banditJump;
    public static AudioClip[] nerdHit;
    public static AudioClip[] nerdMiss;
    public static AudioClip[] nerdStart;
    public static AudioClip[] nerdJump;


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

        rocket = Resources.Load<AudioClip>("rocket");
        explosion = Resources.Load<AudioClip>("explosion");

        arrowShotSound = Resources.Load<AudioClip>("PfeilSchuss");
        arrowHitSound = Resources.Load<AudioClip>("PfeilTreffer");
        //bandits
        ouchSound1 = Resources.Load<AudioClip>("ouch1");
        ouchSound2 = Resources.Load<AudioClip>("ouch2");
        ouchSound3 = Resources.Load<AudioClip>("ouch3");
        ouchSound4 = Resources.Load<AudioClip>("ouch4");
        banditJumpSound1 = Resources.Load<AudioClip>("Bandit_Jump");
        banditJumpSound2 = Resources.Load<AudioClip>("Bandit_Jump2");
        banditDeathSound = Resources.Load<AudioClip>("Bandit_Death2");
        banditHitSound1 = Resources.Load<AudioClip>("Bandit_Hit");
        banditHitSound2 = Resources.Load<AudioClip>("Bandit_Hit2");
        banditMissSound1 = Resources.Load<AudioClip>("Bandit_Miss");
        banditMissSound2 = Resources.Load<AudioClip>("Bandit_Miss3");
        banditStartSound1 = Resources.Load<AudioClip>("Bandit_newRound4");
        banditStartSound2 = Resources.Load<AudioClip>("Bandit_newRound5");
        banditLevelUpSound = Resources.Load<AudioClip>("Bandit_Levelup");

        //nerds
        hmpfSound1 = Resources.Load<AudioClip>("hmpf1");
        hmpfSound2 = Resources.Load<AudioClip>("hmpf2");
        hmpfSound3 = Resources.Load<AudioClip>("hmpf3");
        hmpfSound4 = Resources.Load<AudioClip>("hmpf4");
        deathSound = Resources.Load<AudioClip>("death");
        nerdJumpSound1 = Resources.Load<AudioClip>("nerd_jump1");
        nerdJumpSound2 = Resources.Load<AudioClip>("nerd_jump2");
        nerdDeathSound = Resources.Load<AudioClip>("nerd_death");
        nerdHitSound1 = Resources.Load<AudioClip>("nerd_hit1");
        nerdHitSound2 = Resources.Load<AudioClip>("nerd_hit2");
        nerdMissSound1 = Resources.Load<AudioClip>("nerd_miss1");
        nerdMissSound2 = Resources.Load<AudioClip>("nerd_miss2");
        nerdStartSound1 = Resources.Load<AudioClip>("nerd_start1");
        nerdStartSound2 = Resources.Load<AudioClip>("nerd_start2");
        nerdLevelUpSound = Resources.Load<AudioClip>("nerd_levelup");

        //vikings
        vikingHitSound1 = Resources.Load<AudioClip>("Viking Hit1");
        vikingHitSound2 = Resources.Load<AudioClip>("Viking Hit2");
        vikingHitSound3 = Resources.Load<AudioClip>("Viking Hit3");
        vikingHitSound4 = Resources.Load<AudioClip>("Viking Hit4");
        vikingJumpSound1 = Resources.Load<AudioClip>("Viking Jump1");
        vikingJumpSound2 = Resources.Load<AudioClip>("Viking Jump2");
        vikingDeathSound = Resources.Load<AudioClip>("Viking Death");
        vikingJoySound1 = Resources.Load<AudioClip>("Viking Joy1");
        vikingJoySound2 = Resources.Load<AudioClip>("Viking Joy2");
        vikingMissSound1 = Resources.Load<AudioClip>("Viking Miss1");
        vikingMissSound2 = Resources.Load<AudioClip>("Viking Miss2");
        vikingStartSound1 = Resources.Load<AudioClip>("Viking Start1");
        vikingStartSound2 = Resources.Load<AudioClip>("Viking Start2");
        vikingLevelUpSound = Resources.Load<AudioClip>("Viking LevelUP");

        ouch = new AudioClip[] { ouchSound1, ouchSound2, ouchSound3, ouchSound4 };
        hmpf = new AudioClip[] { hmpfSound1, hmpfSound2, hmpfSound3, hmpfSound4 };
        //vikings
        vikingHit = new AudioClip[] { vikingHitSound1, vikingHitSound2, vikingHitSound3, vikingHitSound4 };
        vikingJump = new AudioClip[] { vikingJumpSound1, vikingJumpSound2 };
        vikingJoy = new AudioClip[] { vikingJoySound1, vikingJoySound2 };
        vikingMiss = new AudioClip[] { vikingMissSound1, vikingMissSound2 };
        vikingStart = new AudioClip[] { vikingStartSound1, vikingStartSound2 };
        nerdJump = new AudioClip[] { nerdJumpSound1, nerdJumpSound2 };
        nerdHit = new AudioClip[] { nerdHitSound1, nerdHitSound2 };
        nerdMiss = new AudioClip[] { nerdMissSound1, nerdMissSound2 };
        nerdStart = new AudioClip[] { nerdStartSound1, nerdStartSound2 };
        banditJump = new AudioClip[] { banditJumpSound1, banditJumpSound2 };
        banditHit = new AudioClip[] { banditHitSound1, banditHitSound2 };
        banditMiss = new AudioClip[] { banditMissSound1, banditMissSound2 };
        banditStart = new AudioClip[] { banditStartSound1, banditStartSound2 };
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
            //bandits
            case "ouch":
                efxSource.PlayOneShot(RandomizeSfx(ouch));
                break;
            case "Bandit Jump":
                efxSource.PlayOneShot(RandomizeSfx(banditJump));
                break;
            case "Bandit Death":
                efxSource.PlayOneShot(banditDeathSound);
                break;
            case "Bandit Miss":
                efxSource.PlayOneShot(RandomizeSfx(banditMiss));
                break;
            case "Bandit Joy":
                efxSource.PlayOneShot(RandomizeSfx(banditHit));
                break;
            case "Bandit Start":
                efxSource.PlayOneShot(RandomizeSfx(banditStart));
                break;
            case "Bandit LevelUp":
                efxSource.PlayOneShot(banditLevelUpSound);
                break;
            //nerds
            case "hmpf":
                efxSource.PlayOneShot(RandomizeSfx(hmpf));
                break;
            case "Nerd Jump":
                efxSource.PlayOneShot(RandomizeSfx(nerdJump));
                break;
            case "Nerd Death":
                efxSource.PlayOneShot(nerdDeathSound);
                break;
            case "Nerd Miss":
                efxSource.PlayOneShot(RandomizeSfx(nerdMiss));
                break;
            case "Nerd Joy":
                efxSource.PlayOneShot(RandomizeSfx(nerdHit));
                break;
            case "Nerd Start":
                efxSource.PlayOneShot(RandomizeSfx(nerdStart));
                break;
            case "Nerd LevelUp":
                efxSource.PlayOneShot(nerdLevelUpSound);
                break;
            //vikings
            case "Viking Hit":
                efxSource.PlayOneShot(RandomizeSfx(vikingHit));
                break;
            case "Viking Jump":
                efxSource.PlayOneShot(RandomizeSfx(vikingJump));
                break;  
            case "Viking Death":
                efxSource.PlayOneShot(vikingDeathSound);
                break;
            case "Viking Miss":
                efxSource.PlayOneShot(RandomizeSfx(vikingMiss));
                break;
            case "Viking Joy":
                efxSource.PlayOneShot(RandomizeSfx(vikingJoy));
                break;
            case "Viking Start":
                efxSource.PlayOneShot(RandomizeSfx(vikingStart));
                break;
            case "Viking LevelUp":
                efxSource.PlayOneShot(vikingLevelUpSound);
                break;
            //other
            case "arrowShot":
                efxSource.PlayOneShot(RandomizeSfx(arrowShotSound));
                break;
            case "arrowHit":
                efxSource.PlayOneShot(RandomizeSfx(arrowHitSound));
                break;
            case "explosion":
                efxSource.PlayOneShot(explosion);
                break;
            case "rocket":
                efxSource.PlayOneShot(rocket);
                break;
        }
    }

    public static void PlayAudioClip(AudioClip sound)
    {
        efxSource.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void LoopMusic(bool bo)
    {
        musicSource.loop = bo;
    }

    public static void StopAudioClip()
    {
        efxSource.Stop();
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