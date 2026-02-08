using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Source")]
    public AudioSource musicTitleSource;
    public AudioSource musicIntroSource;
    public AudioSource musicLoopASource;
    public AudioSource musicLoopBSource;
    public AudioSource sfxSource;
    public AudioSource voiceSfxSource;
    public AudioSource heartBeatSource;
    public AudioSource breathingSource;

    [Header("Music")]
    public AudioClip titleMusic;
    public AudioClip introMusic;
    public AudioClip loopMusic;
    
    [Header("Overlap Timing (Seconds)")]
    public float introToLoopOverlap = 0.857f;
    public float loopToLoopOverlap = 0.429f;
    
    [Header("SFX")]
    public AudioClip explosionClip;
    public AudioClip clickClip;
    public AudioClip hitClip;
    public AudioClip moneyPickupClip;
    public AudioClip playerHitClip;
    public AudioClip powerupClip;
    public AudioClip gunClip;
    public AudioClip dieClip;
    public AudioClip instructionTextAppear;
    
    [Header("Voice Lines")]
    public List<AudioClip> voiceLines;

    [Header("Settings")]
    public float healthEffectsStartThreshold = 0.5f;
    public float healthEffectsMaxThreshold = 0.10f;

    void Start()
    {
        musicTitleSource.clip = titleMusic;
        musicTitleSource.volume = 0f;
        musicTitleSource.Stop();
        StartCoroutine(FadeTitleMusic(true));
    }
    
    void Update()
    {
        float healthPercent = Game.Instance.player.currentHealth / Game.Instance.player.maxHealth;
        
        float intensity = Mathf.InverseLerp(healthEffectsStartThreshold, healthEffectsMaxThreshold, healthPercent);

        heartBeatSource.volume = intensity;
        breathingSource.volume = intensity;
        
        if (intensity <= 0 && heartBeatSource.isPlaying)
        {
            heartBeatSource.Pause();
            breathingSource.Pause();
        }
        else if (intensity > 0 && !heartBeatSource.isPlaying)
        {
            heartBeatSource.Play();
            breathingSource.Play();
        }
    }

    private IEnumerator FadeTitleMusic(bool isIn)
    {
        float currentTime = 0f;
        float startVolume = isIn ? 0f : 0.75f;
        float targetVolume = isIn ? 0.75f : 0f;
        float duration = 1f;
        
        if (isIn && !musicTitleSource.isPlaying)
            musicTitleSource.Play();
        
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            musicTitleSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        musicTitleSource.volume = targetVolume;
        
        if (!isIn)
            musicTitleSource.Stop();
    }
    
    private IEnumerator FadeIntroMusicIn(float targetVolume = 0.75f, float duration = 1f)
    {
        float t = 0f;
        musicIntroSource.volume = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            musicIntroSource.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        musicIntroSource.volume = targetVolume;
    }
    
    public IEnumerator PlayMusic()
    {
        StartCoroutine(FadeTitleMusic(false));
        StartCoroutine(FadeIntroMusicIn());
        
        double introDuration = (double)introMusic.samples / introMusic.frequency;
        double loopDuration  = (double)loopMusic.samples  / loopMusic.frequency;

        double dspTime = AudioSettings.dspTime;
        double nextTime = dspTime + 0.1;

        musicIntroSource.clip = introMusic;
        musicIntroSource.PlayScheduled(nextTime);

        nextTime += introDuration - introToLoopOverlap;

        musicLoopASource.clip = loopMusic;
        musicLoopBSource.clip = loopMusic;

        AudioSource current = musicLoopASource;
        AudioSource next    = musicLoopBSource;

        current.PlayScheduled(nextTime);

        while (true)
        {
            nextTime += loopDuration - loopToLoopOverlap;

            while (AudioSettings.dspTime < nextTime - 0.5)
                yield return null;

            next.PlayScheduled(nextTime);

            (current, next) = (next, current);
        }
    }

    public void PlayExplosion()
    {
        sfxSource.pitch = Random.Range(0.8f, 1.0f);
        sfxSource.volume = 0.6f;
        if (explosionClip) sfxSource.PlayOneShot(explosionClip);
    }

    public void PlayClick()
    {
        sfxSource.pitch = 1.0f;
        sfxSource.volume = 0.6f;
        if (clickClip) sfxSource.PlayOneShot(clickClip);
    }

    public void PlayEnemyHit()
    {
        sfxSource.pitch = Random.Range(0.6f, 0.8f);
        sfxSource.volume = 0.6f;
        if (hitClip) sfxSource.PlayOneShot(hitClip);
    }

    public void PlayMoney()
    {
	    sfxSource.pitch = 1.0f;
	    sfxSource.volume = 0.8f;
        if (moneyPickupClip) sfxSource.PlayOneShot(moneyPickupClip);
    }

    public void PlayPlayerHit()
    {
            sfxSource.pitch = Random.Range(0.8f, 1.0f);
            sfxSource.volume = 0.6f;
        if (playerHitClip) sfxSource.PlayOneShot(playerHitClip);
    }

    public void PlayPowerUp()
    {
        if (powerupClip) sfxSource.PlayOneShot(powerupClip);
    }

    public void PlayGunFire()
    {
        if (gunClip)
        {
            sfxSource.pitch = Random.Range(0.2f, 0.8f);
            sfxSource.volume = Random.Range(0.6f, 0.7f);
            sfxSource.PlayOneShot(gunClip);
        }
    }

    public void PlayInstructionTextAppear(float pitch = -1f)
    {
        sfxSource.pitch = (pitch == -1) ? Random.Range(0.7f, 1.3f) : pitch;
        sfxSource.volume = 0.9f;
        sfxSource.PlayOneShot(instructionTextAppear);
    }

    public void PlayPlayerDie()
    {
        if (dieClip) sfxSource.PlayOneShot(dieClip);
    }

    public void PlayVoiceLineRandom()
    {
        int rand = Random.Range(1, voiceLines.Count);
	voiceSfxSource.volume = 0.5f;
        voiceSfxSource.PlayOneShot(voiceLines[rand]);
	voiceSfxSource.volume = 0.5f;
    }

    public void PlayVoiceLineIndex(int index)
    {
	voiceSfxSource.volume = 0.5f;
        voiceSfxSource.PlayOneShot(voiceLines[index]);
	voiceSfxSource.volume = 0.5f;
    }
}
