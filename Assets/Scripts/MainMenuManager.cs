using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PressStart ()
    {
        SceneManager.LoadScene("samplescene");
    }

    public void PressEndGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}