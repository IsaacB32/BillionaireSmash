using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Source")]
    public AudioSource musicIntroSource;
    public AudioSource musicLoopASource;
    public AudioSource musicLoopBSource;
    public AudioSource sfxSource;
    public AudioSource heartBeatSource;
    public AudioSource breathingSource;

    [Header("Music")]
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

    [Header("Voice Lines")]
    public List<AudioClip> voiceLines;

    [Header("Settings")]
    public float healthEffectsStartThreshold = 0.5f;
    public float healthEffectsMaxThreshold = 0.10f;
    
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
    
    public IEnumerator PlayMusic()
    {
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
        if (explosionClip) sfxSource.PlayOneShot(explosionClip);
    }

    public void PlayClick()
    {
        if (clickClip) sfxSource.PlayOneShot(clickClip);
    }

    public void PlayEnemyHit()
    {
        if (hitClip) sfxSource.PlayOneShot(hitClip);
    }

    public void PlayMoney()
    {
        if (moneyPickupClip) sfxSource.PlayOneShot(moneyPickupClip);
    }

    public void PlayPlayerHit()
    {
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
            sfxSource.pitch = Random.Range(0.8f, 1.2f);
            sfxSource.volume = Random.Range(0.8f, 1.2f);
            sfxSource.PlayOneShot(gunClip);
            sfxSource.pitch = 1f;
            sfxSource.volume = 1f;
        }
    }

    public void PlayVoiceLineRandom()
    {
        int rand = Random.Range(1, voiceLines.Count);
        sfxSource.PlayOneShot(voiceLines[rand]);
    }

    public void PlayVoiceLineIndex(int index)
    {
        sfxSource.PlayOneShot(voiceLines[index]);
    }
}
