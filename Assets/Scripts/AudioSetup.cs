using UnityEngine;
using UnityEngine.Audio;

public class AudioSetup : MonoBehaviour
{
    public AudioMixer audioMixer;

    public string paramaterName;

    public void OnSliderValueChanged(float value)
    {
        audioMixer.SetFloat(paramaterName, value);
    }
}
