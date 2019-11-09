using UnityEngine;
using System;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    public int SoundCount => sounds.Length;

    [SerializeField] private AudioSource sampleSourceFor3DSettings;

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private AudioSource oneShotSource;

    private void Awake()
    {
        instance = this;

        oneShotSource = gameObject.AddComponent<AudioSource>();

        foreach (Sound s in sounds)
        {
            if (s.Is3DSound == false)
            {
                GameObject sourceGameobject = new GameObject("AudioSource Sound : " + s.Name);
                sourceGameobject.transform.parent = transform;
                AudioSource newSource = sourceGameobject.AddComponent<AudioSource>();
                s.source = newSource;
                s.source.clip = s.Clip;
                s.source.volume = s.Volume;
                s.source.pitch = s.Pitch;
                s.source.outputAudioMixerGroup = s.MixerGroup;
                s.source.loop = s.Loop;
                s.source.playOnAwake = s.PlayOnAwake;
                s.source.ignoreListenerPause = s.IgnoreGamePaused;
            }
            if (s.PlayOnAwake)
                s.source.Play();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlaySound("EnemyKilled");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlaySound("EnemyStunned");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlaySound("PlayerHit");
        }
    }

    public void Create3DAudioSources()
    {
        foreach (Sound s in sounds)
        {
            if (s.Is3DSound)
            {
                if (s.ParentObject == null)
                    throw new Exception("3D sound " + s.Name + " needs a parent object!");
                for (int i = s.ParentObject.transform.childCount - 1; i >= 0; i--)
                {
                    if (s.ParentObject.transform.GetChild(i).GetComponent<AudioSource>() != null)
                        DestroyImmediate(s.ParentObject.transform.GetChild(i).gameObject);
                }

                GameObject sourceGameobject = new GameObject("AudioSource Sound : " + s.Name);
                sourceGameobject.transform.parent = s.ParentObject.transform;
                AudioSource newSource = sourceGameobject.AddComponent<AudioSource>();
                s.source = newSource;
                s.source.clip = s.Clip;
                s.source.volume = s.Volume;
                s.source.pitch = s.Pitch;
                s.source.outputAudioMixerGroup = s.MixerGroup;
                s.source.loop = s.Loop;
                s.source.playOnAwake = s.PlayOnAwake;
                s.source.ignoreListenerPause = s.IgnoreGamePaused;
            }
        }
    }

    public Sound GetSound(int i) => sounds[i];

    public void PlaySoundOneShot(AudioClip clip, float volume = 1f)
    {
        oneShotSource.PlayOneShot(clip, volume);
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s != null)
        {
            s.source.volume = s.Volume;
            s.source.pitch = s.RandomizePitch ? s.Pitch + UnityEngine.Random.Range(-s.PitchRange, s.PitchRange) : s.Pitch;
            s.source.time = 0f;
            s.source.Play();
        }
        else
        {
            Debug.LogError("Cannot find sound with name: " + name);
        }
    }

    public void PlaySound(string name, float pitchRange)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s != null)
        {
            SetAudiosourceToOriginalVolume(name);
            s.source.time = 0f;
            s.source.pitch = s.Pitch + UnityEngine.Random.Range(-pitchRange, pitchRange);
            s.source.Play();
        }
        else
        {
            Debug.LogError("Cannot find sound with name: " + name);
        }
    }

    public void PlaySoundOverrideVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s != null)
        {
            s.source.volume = volume;
            s.source.pitch = s.Pitch;
            s.source.time = 0f;
            s.source.Play();
        }
        else
        {
            Debug.LogError("Cannot find sound with name: " + name);
        }
    }

    public void PlaySoundOverridePitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s != null)
        {
            s.source.volume = s.Volume;
            s.source.pitch = pitch;
            s.source.time = 0f;
            s.source.Play();
        }
        else
        {
            Debug.LogError("Cannot find sound with name: " + name);
        }
    }

    public void PlayRandomSound(params string[] soundNames)
    {
        int numSounds = soundNames.Length;
        if (numSounds < 2)
            throw new ArgumentException("PlayRandomSound needs at least to soundeffect names!");
        Sound[] soundsToChoseFrom = new Sound[numSounds];
        for (int i = 0; i < numSounds; i++)
        {
            var sound = Array.Find(sounds, x => x.Name == soundNames[i]);
            if (sound == null)
                throw new ArgumentException("PlayRandomSound sound could not be found: " + soundNames[i]);
            soundsToChoseFrom[i] = sound;
        }

        Sound soundToPlay = soundsToChoseFrom.RandomElement();
        soundToPlay.source.volume = soundToPlay.Volume;
        soundToPlay.source.pitch = soundToPlay.RandomizePitch ? soundToPlay.Pitch + UnityEngine.Random.Range(-soundToPlay.PitchRange, soundToPlay.PitchRange) : soundToPlay.Pitch;
        soundToPlay.source.time = 0f;
        soundToPlay.source.Play();
    }

    public AudioSource GetAudioSource(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s != null)
        {
            return s.source;
        }
        else
        {
            Debug.LogError("Cannot find Sound with name: " + name);
            return null;
        }
    }

    public float GetSoundDuration(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s != null)
        {
            return s.Clip.length;
        }
        else
        {
            Debug.LogError("Cannot find sound with name: " + name);
            return 0f;
        }
    }

    public void SetAudiosourceToOriginalVolume(string name)
    {
        GetAudioSource(name).volume = GetOriginalVolume(name);
    }

    public void SetAudiosourceToOriginalPitch(string name)
    {
        GetAudioSource(name).pitch = GetOriginalPitch(name);
    }

    public float GetOriginalVolume(string name)
    {
        Sound sound = Array.Find(sounds, s => s.Name == name);
        if (sound != null)
        {
            return sound.Volume;
        }
        else
        {
            Debug.LogError("Cannot find sound with name: " + name + "\n Volume returned is 0f!");
            return 0f;
        }
    }

    public float GetOriginalPitch(string name)
    {
        Sound sound = Array.Find(sounds, s => s.Name == name);
        if (sound != null)
        {
            return sound.Pitch;
        }
        else
        {
            Debug.LogError("Cannot find sound with name: " + name + "\n Pitch returned is 0f!");
            return 0f;
        }
    }

    public string[] GetAllSoundNames()
    {
        string[] names = new string[sounds.Length];
        for (int i = 0; i < sounds.Length; i++)
        {
            names[i] = sounds[i].Name;
        }
        return names;
    }

#if UNITY_EDITOR

    public void AddNewSound()
    {
        if (sounds == null)
            sounds = new Sound[1];
        else
            Array.Resize(ref sounds, SoundCount + 1);
    }

    public void RemoveSound(int index)
    {
        Sound sound = sounds[index];
        if(sound.Is3DSound && sound.ParentObject != null)
        {
            for (int i = sound.ParentObject.transform.childCount - 1; i >= 0; i--)
            {
                if (sound.ParentObject.transform.GetChild(i).GetComponent<AudioSource>() != null)
                    DestroyImmediate(sound.ParentObject.transform.GetChild(i).gameObject);
            }
        }

        sounds[index] = null;
        var remainingSounds = sounds.Where(s => s != null);
        sounds = remainingSounds.ToArray();
    }

    private GameObject testingSoundGameobject;
    private AudioSource testingSoundSource;

    public void PlayTestingSound(int index)
    {
        Sound s = sounds[index];
        if (testingSoundGameobject == null)
        {
            testingSoundGameobject = new GameObject();
            testingSoundGameobject.hideFlags = HideFlags.HideAndDontSave;
            testingSoundGameobject.AddComponent(typeof(AudioSource));
            testingSoundSource = testingSoundGameobject.GetComponent<AudioSource>();
        }
        if (testingSoundSource.isPlaying)
            testingSoundSource.Stop();

        testingSoundSource.volume = s.Volume;
        testingSoundSource.pitch = s.RandomizePitch ? s.Pitch + UnityEngine.Random.Range(-s.PitchRange, s.PitchRange) : s.Pitch;
        testingSoundSource.clip = s.Clip;
        testingSoundSource.outputAudioMixerGroup = s.MixerGroup;

        testingSoundSource.Play();
    }

    public void StopTestingSound()
    {
        if (testingSoundSource?.isPlaying == true)
            testingSoundSource.Stop();
    }

#endif
}
