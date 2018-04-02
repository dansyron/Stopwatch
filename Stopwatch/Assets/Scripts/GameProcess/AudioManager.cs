/**
 * Added to project by: William Riden
 * Date: 25 September 2016
 * email: william.riden@gmail.com 
 * 
 * This class will handle the back ground music and the playing of individual sound effects.
 * code from unity tutorial on sound management
 */

using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Any sound effect should be added to this enumeration
/// </summary>
public enum SoundEffect
{
    PlayerJump, PlayerLand, PlayerDodge, PlayerAttack, PlayerHurt, PlayerFootsteps,
    PlayerBoost, GameOver, SuccessfulStab, EnemyDie, TimeIncrement, TimeSlowDown, TimeSpeedUp
};

public class AudioManager : MonoBehaviour
    {

    #region fields
    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public AudioSource musicSourceAlt;
    public static AudioManager instance = null;     //Allows other scripts to call functions from AudioManager.             
    //for creating varying pitch in repeating soundeffects
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.


    public float musicVolume;
    public float sfxVolume;

    //field for game music
    public AudioClip[] music;
    int musicPos = 0;

    Dictionary<string, AudioClip> musicDict;
    Dictionary<SoundEffect, AudioClip> soundEffects;

    #endregion


    #region Private Methods
    void Start()
    {
        musicDict = (Resources.LoadAll<AudioClip>("Audio/Music")).ToDictionary(s => s.name);
        StartMusic();
        musicVolume = musicSource.volume;
        musicSource.volume = 0;
        FadeIn(musicSource);
        musicSource.loop = true;
        musicSourceAlt.loop = true;
        soundEffects = new Dictionary<SoundEffect, AudioClip>()
        {
            { SoundEffect.PlayerJump, Resources.Load<AudioClip>("Audio/Sounds/Player/PlayerJump") },
            { SoundEffect.PlayerAttack, Resources.Load<AudioClip>("Audio/Sounds/Player/PlayerAttack") },
            { SoundEffect.PlayerLand, Resources.Load<AudioClip>("Audio/Sounds/Player/PlayerLand") },
            { SoundEffect.PlayerDodge, Resources.Load<AudioClip>("Audio/Sounds/Player/PlayerDodge") },
            { SoundEffect.PlayerHurt, Resources.Load<AudioClip>("Audio/Sounds/Player/PlayerHurt") },
            { SoundEffect.PlayerFootsteps, Resources.Load<AudioClip>("Audio/Sounds/Player/PlayerFootsteps") },
            { SoundEffect.SuccessfulStab, Resources.Load<AudioClip>("Audio/Sounds/Enemy/SuccessfulStab") },
            { SoundEffect.EnemyDie, Resources.Load<AudioClip>("Audio/Sounds/Enemy/EnemyDie") },
            { SoundEffect.TimeIncrement, Resources.Load<AudioClip>("Audio/Sounds/UI/TimeIncrement") },
            { SoundEffect.TimeSlowDown, Resources.Load<AudioClip>("Audio/Sounds/UI/TimeSlowDown") },
            { SoundEffect.TimeSpeedUp, Resources.Load<AudioClip>("Audio/Sounds/UI/TimeSpeedUp") },
            { SoundEffect.PlayerBoost, Resources.Load<AudioClip>("Audio/Sounds/Player/PlayerBoost") },
            { SoundEffect.GameOver, Resources.Load<AudioClip>("Audio/Sounds/UI/GameOver") },
        };

    }

    
    /// <summary>
    /// update, self explanatory, if you need a comment here explaining what this method does. Reconsider your major.
    /// </summary> 
    void Update()
    {
        SlowDownMusic(GameManager.Instance.Timer.getSlowDownTime);
        if (Input.GetKeyDown(KeyCode.M)) {
            MuteMusic();
        }
    }
    void Awake()
        {
            //Check if there is already an instance of AudioManager
            if (instance == null)
            {
            //if not, set it to this.
            instance = this;
            }
            //If instance already exists:
            else if (instance != this)
            {
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);
            }
            
            //Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad(gameObject);
        }

    void MuteMusic() {
        if (musicSource.volume > 0 || musicSourceAlt.volume > 0) {
            musicSource.volume = 0;
            musicSourceAlt.volume = 0;
        } else
        {
            musicSource.volume = musicVolume;
            musicSourceAlt.volume = 0;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// play a single sound effect
    /// </summary>
    /// <param name="clip">the sound effect to be played</param>
    public void PlaySingle(SoundEffect sound)
    {
        if(soundEffects.ContainsKey(sound))
        {
            //play the clip
            efxSource.PlayOneShot(soundEffects[sound], 1);
        }
        else
        {
            Debug.LogError("Sound effect not contained in dictionary");
        }
    }

    /// <summary>
    /// called by the player animation on every footstep
    /// </summary>
    public void PlayFootstepSound()
    {
        PlaySingle(SoundEffect.PlayerFootsteps);
    }


    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    //use this for playing sound effects that repeat, ie footsteps, to avoid them becoming annoying
    public void RandomizeSfx(params SoundEffect[] sounds) //params allows us to pass in as many comma seperated audio clips as we'd like
    {

        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, sounds.Length);

            
        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        efxSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        efxSource.clip = soundEffects[sounds[randomIndex]];

        //Play the clip.
        efxSource.Play();
    }

    //handles starting the music
    public void StartMusic() {
        if (Random.value > 0.5)
        {
            musicSource.clip = musicDict["Milliseconds"];
        }
        else
        {
            musicSource.clip = musicDict["Synchro"];
        }

        musicSourceAlt.clip = musicDict["TimeSpace"];
        musicSourceAlt.volume = 0;
        
        //play from musicSource
        musicSource.Play();
        musicSourceAlt.Play();
    }


    public void PauseMusic() {

        //pause the music
        musicSource.Pause();
        musicSourceAlt.Pause();

    }
    

    //for stopping the music
    public void StopMusic()
    {
        //stop the music
        musicSource.Stop();
        musicSource.clip = null;

    }

    //play the next track in the array of music
    void NextTrack(){
        if (musicPos < 2) {
            musicPos++;
        }
        else { musicPos = 0; }
        musicSource.clip = music[musicPos];
        Debug.Log("Current Track: " + music[musicPos]);
        musicSource.Play();
    }

    //play the previous track in the array of music
    public void PrevTrack() {
        if (musicPos > 0)
        {
            musicPos--;
        }
        else { musicPos = 2; }
        musicSource.clip = music[musicPos];
        Debug.Log("Current Track: " + music[musicPos]);
        musicSource.Play();
    }

    /// <summary>
    /// for fading out a music track
    /// </summary>
    /// <param name="audioSource">the source playing the track we want to fade out</param>
    public void FadeOut(AudioSource audioSource) {
        if (audioSource.volume > 0) {
            audioSource.volume -= 0.1f;
                }
    }

    /// <summary>
    /// for fading out a music track
    /// </summary>
    /// <param name="audioSource">the source playing the track we want to fade out</param>
    public void PitchDown(AudioSource audioSource)
    {
        if (audioSource.pitch > 0.5) //JORDAN CHANGE THIS VALUE
        {
            audioSource.pitch -= 0.5f;
        }
    }

    /// <summary>
    /// for fading out a music track
    /// </summary>
    /// <param name="audioSource">the source playing the track we want to fade out</param>
    public void PitchUp(AudioSource audioSource)
    {
        if (audioSource.pitch < 1)
        {
            audioSource.pitch += 0.5f;
        }
    }
    /// <summary>
    /// for fading in a music track
    /// </summary>
    /// <param name="audioSource">the audio source playing the track we want to fade in</param>
    public void FadeIn(AudioSource audioSource) {
        if (audioSource.volume < musicVolume) {
            audioSource.volume += 0.1f;
        }
    }

    /// <summary>
    /// for handling the transition of the music between the main track and the underlying track, trigger this when the player slows time
    /// </summary>
    /// <param name="SlowingDown">is the player Slowing down time? pass true, if not pass in false</param>
    public void SlowDownMusic(bool SlowingDown) {
        if (SlowingDown == true)
        {
            PitchDown(musicSource);
            //FadeOut(musicSource); //fade out the current music track
            //FadeIn(musicSourceAlt); //fade in the underlying track
        }
        else {
            PitchUp(musicSource);
            // FadeOut(musicSourceAlt); //fade out the underlying music track
            //FadeIn(musicSource); //fade in the current track
        }
    }

    public void Restart() {
        StartMusic();
        FadeIn(musicSource);
    }
    #endregion


}
