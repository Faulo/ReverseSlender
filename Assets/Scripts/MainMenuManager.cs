using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip audioClipStart;
    public AudioClip audioClipClick;
    public AudioClip audioClipHover;

    public void PressStart()
    {
        SceneManager.LoadScene("Terrain_mitNavMeshAgent");
    }

    public void PressEndGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void PlaySoundStart()
    {
        audioSource.PlayOneShot(audioClipStart);
    }

    public void PlaySoundClick()
    {
        audioSource.PlayOneShot(audioClipClick);
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(audioClipHover);
    }
}