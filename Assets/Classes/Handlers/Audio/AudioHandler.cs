using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : Singleton<AudioHandler>
{
    [NonReorderable]
    public Sound[] music;

    [NonReorderable]
    public Sound[] sounds;

    private Sound currentMusic;

    protected override void Awake()
    {
        base.Awake();

        foreach (Sound sound in music)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.spatialBlend = 1f;
            sound.source.loop = sound.loop;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.spatialBlend = 1f;
            sound.source.loop = sound.loop;
        }
    }

    public static void BlendMusic(string name)
    {
        foreach (Sound sound in instance.music)
        {
            if (sound.name == name)
            {
                instance.currentMusic = sound;
                sound.source.Play();
                return;
            }
        }
    }

    public static void StopMusic()
    {
        if (instance.currentMusic != null)
        {
            instance.currentMusic.source.Stop();
            instance.currentMusic = null;
        }
    }

    public static void FadeOutMusic()
    {
        if (instance.currentMusic != null)
        {
            instance.StartCoroutine(instance.FadeCurrentMusic());
        }
    }

    public static void PlayMusic(string name)
    {
        foreach (Sound sound in instance.music)
        {
            if (sound.name == name)
            {
                instance.currentMusic = sound;
                sound.source.Play();
                return;
            }
        }
    }

    public static void PlaySound(string name)
    {
        foreach (Sound sound in instance.sounds)
        {
            if (sound.name == name)
            {
                sound.source.Play();
                return;
            }
        }
    }

    private IEnumerator FadeCurrentMusic()
    {
        if (instance.currentMusic != null)
        {
            AudioSource source = instance.currentMusic.source;

            float originalVolume = source.volume;
            float fadeAmount = instance.currentMusic.source.volume;

            while (source.volume > 0)
            {
                fadeAmount = source.volume - (0.5f * Time.deltaTime);
                source.volume = fadeAmount;

                yield return null;
            }

            source.Stop();
            source.volume = originalVolume;
            instance.currentMusic = null;
        }

        yield return null;
    }
}
