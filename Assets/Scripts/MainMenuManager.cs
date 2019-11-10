using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip audioClip;
    
    public void PressStart ()
    {
        SceneManager.LoadScene("Terrain_mitNavMeshAgent");
    }

    public void PressEndGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void PlaySound ()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}