using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private AudioMixerGroup mixerGroup;
    public AudioMixerGroup MixerGroup => mixerGroup;

    [SerializeField] private AudioClip clip;
    public AudioClip Clip => clip;

    [SerializeField, Range(0f, 1f)] private float volume = 1f;
    public float Volume => volume;

    [SerializeField, Range(0f, 2f)] private float pitch = 1f;
    public float Pitch => pitch;

    [SerializeField] private bool randomizePitch;
    public bool RandomizePitch => randomizePitch;

    [SerializeField, Range(0f, 1f)] private float pitchRange;
    public float PitchRange => pitchRange;

    [SerializeField] private bool loop;
    public bool Loop => loop;

    [SerializeField] private bool playOnAwake;
    public bool PlayOnAwake => playOnAwake;

    [SerializeField] private bool ignoreGamePaused;
    public bool IgnoreGamePaused => ignoreGamePaused;

    [Header("3D Sound Settings")]

    [SerializeField] private bool is3DSound;
    public bool Is3DSound => is3DSound;

    [SerializeField] private GameObject parentObject;
    public GameObject ParentObject => parentObject;


    [HideInInspector]
    public AudioSource source;
}