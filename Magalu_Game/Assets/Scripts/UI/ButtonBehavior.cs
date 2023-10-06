using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene("Level1");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void PlayCutScene()
    {
        SceneManager.LoadScene("Cinematic");
    }
}
