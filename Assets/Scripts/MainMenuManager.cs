using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip audioClipStart;
    public AudioClip audioClipClick;
    
    public void PressStart ()
    {
        SceneManager.LoadScene("Terrain_mitNavMeshAgent");
    }

    public void PressEndGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void PlaySoundStart ()
    {
        audioSource.clip = audioClipStart;
        audioSource.Play();
    }

    public void PlaySoundClick()
    {
        audioSource.clip = audioClipClick;
        audioSource.Play();
    }
}