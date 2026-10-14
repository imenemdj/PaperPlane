using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip buttonClickSound;

    [Header("Menu Music")]
    [SerializeField] private AudioClip menuTheme;

    [Header("Game Music (switches every 10 score)")]
    [SerializeField] private AudioClip musicTrack1;
    [SerializeField] private AudioClip musicTrack2;
    [SerializeField] private AudioClip musicSwitchSound;

    [Header("Ambient Sound")]
    [SerializeField] private AudioClip ambientSound;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip hitSound;

    private int currentTrack = 1;
    private AudioLowPassFilter lowPassFilter;

    // Mute states
    private bool isMusicMuted = false;
    private bool isSfxMuted = false;
    private bool isBgSfxMuted = false;

    private void Awake()
    {
        // Singleton pattern - keeps only one AudioManager across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Add low pass filter to the music source's GameObject
        if (musicSource != null)
        {
            lowPassFilter = musicSource.GetComponent<AudioLowPassFilter>();
            if (lowPassFilter == null)
            {
                lowPassFilter = musicSource.gameObject.AddComponent<AudioLowPassFilter>();
            }
            lowPassFilter.enabled = false; // Start disabled
        }
    }

    // Play menu theme (for main menu and options)
    public void PlayMenuMusic()
    {
        if (isMusicMuted) return;

        if (menuTheme != null && musicSource != null)
        {
            // Only start if not already playing this track
            if (musicSource.clip != menuTheme || !musicSource.isPlaying)
            {
                musicSource.clip = menuTheme;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
    }

    // Apply muffled effect (for options menu)
    public void MuffleMusic()
    {
        if (lowPassFilter != null)
        {
            lowPassFilter.enabled = true;
            lowPassFilter.cutoffFrequency = 900f; // Lower = more muffled (range: 10-22000)
        }
    }

    // Remove muffled effect (back to main menu)
    public void UnmuffleMusic()
    {
        if (lowPassFilter != null)
        {
            lowPassFilter.enabled = false;
        }
    }

    // Play music track 1 (default start)
    public void PlayMusic()
    {
        // Always set track to 1 at game start, regardless of mute
        currentTrack = 1;

        if (isMusicMuted) return;

        if (musicTrack1 != null && musicSource != null)
        {
            musicSource.clip = musicTrack1;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Switch music based on score (call this when score changes)
    public void CheckMusicSwitch(int score)
    {
        if (score <= 0) return;

        // Every 10 points, switch track
        if (score % 10 == 0)
        {
            if (currentTrack == 1)
            {
                SwitchToTrack2();
            }
            else
            {
                SwitchToTrack1();
            }
        }
    }

    private void SwitchToTrack1()
    {
        currentTrack = 1;

        // Always play the wind transitional sound, regardless of mute
        PlayMusicSwitchSound();

        // Only switch music track if not muted
        if (!isMusicMuted && musicTrack1 != null && musicSource != null)
        {
            musicSource.clip = musicTrack1;
            musicSource.Play();
        }

        // Deactivate speed effects
        CameraShake.Instance?.StopShake();
        SpeedLines.Instance?.HideLines();
        Spawner.Instance?.SetNormalSpeed();
        Pipes.SetNormalSpeed();

        // Set all parallax backgrounds to normal speed
        foreach (var parallax in FindObjectsByType<Parallax>(FindObjectsSortMode.None))
        {
            parallax.SetNormalSpeed();
        }
    }

    private void SwitchToTrack2()
    {
        currentTrack = 2;

        // Always play the wind transitional sound, regardless of mute
        PlayMusicSwitchSound();

        // Only switch music track if not muted
        if (!isMusicMuted && musicTrack2 != null && musicSource != null)
        {
            musicSource.clip = musicTrack2;
            musicSource.Play();
        }

        // Activate speed effects
        CameraShake.Instance?.StartShake();
        SpeedLines.Instance?.ShowLines();
        Spawner.Instance?.SetFastSpeed();
        Pipes.SetFastSpeed();

        // Set all parallax backgrounds to fast speed
        foreach (var parallax in FindObjectsByType<Parallax>(FindObjectsSortMode.None))
        {
            parallax.SetFastSpeed();
        }
    }

    // Play the music switch notification sound
    private void PlayMusicSwitchSound()
    {
        if (musicSwitchSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(musicSwitchSound);
        }
    }

    // Stop background music
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    // Stop ambient sound
    public void StopAmbient()
    {
        if (sfxSource != null && sfxSource.isPlaying && sfxSource.loop)
        {
            sfxSource.Stop();
            sfxSource.loop = false;
        }
    }

    // Play ambient sound (looping)
    public void PlayAmbient()
    {
        if (isBgSfxMuted) return;

        if (ambientSound != null && sfxSource != null)
        {
            // Only start if not already playing this clip as a loop
            if (sfxSource.clip != ambientSound || !sfxSource.isPlaying || !sfxSource.loop)
            {
                sfxSource.clip = ambientSound;
                sfxSource.loop = true;
                sfxSource.Play();
            }
        }
    }

    // UI Sounds (always plays, not affected by mute)
    public void PlayButtonClick()
    {
        if (buttonClickSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }

    // Player Sounds
    public void PlayHit()
    {
        if (isSfxMuted) return;

        if (hitSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(hitSound);
        }
    }

    // Generic method to play any sound effect
    public void PlaySound(AudioClip clip)
    {
        if (isSfxMuted) return;

        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Toggle Methods
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;

        if (isMusicMuted)
        {
            // Stop music if muted
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }
        // Don't restart music here - let each scene handle its own music
    }

    public void ToggleSfx()
    {
        isSfxMuted = !isSfxMuted;
    }

    public void ToggleBgSfx()
    {
        isBgSfxMuted = !isBgSfxMuted;

        if (isBgSfxMuted)
        {
            // Stop ambient if muted
            StopAmbient();
        }
        // Don't restart here - let the game scene handle starting it via PlayAmbient()
    }

    // Get mute states (for UI)
    public bool IsMusicMuted() => isMusicMuted;
    public bool IsSfxMuted() => isSfxMuted;
    public bool IsBgSfxMuted() => isBgSfxMuted;
}

