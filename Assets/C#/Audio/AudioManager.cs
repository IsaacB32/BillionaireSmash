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
    
    public IEnumerator PlayMusic()
    {
        double introDuration = (double)introMusic.samples / introMusic.frequency;
        double loopDuration = (double)loopMusic.samples / loopMusic.frequency;
        
        double startTime = AudioSettings.dspTime + 0.1;
        musicIntroSource.clip = introMusic;
        musicIntroSource.PlayScheduled(startTime);
        
        double nextStartTime = startTime + introDuration - introToLoopOverlap;
        
        musicLoopASource.clip = loopMusic;
        musicLoopASource.PlayScheduled(nextStartTime);

        bool useSourceA = false;
        
        while (true)
        {
            double timeUntilSchedule = nextStartTime - AudioSettings.dspTime - 1.0; 
            if (timeUntilSchedule > 0)
                yield return new WaitForSecondsRealtime((float)timeUntilSchedule);

            nextStartTime = nextStartTime + loopDuration - loopToLoopOverlap;

            AudioSource activeSource = useSourceA ? musicLoopASource : musicLoopBSource;
            activeSource.PlayScheduled(nextStartTime);

            useSourceA = !useSourceA;

            yield return new WaitForSecondsRealtime((float)(loopDuration - loopToLoopOverlap - 0.1f));
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
        if (gunClip) sfxSource.PlayOneShot(gunClip);
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
