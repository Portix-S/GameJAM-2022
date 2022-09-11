using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject howToPlay;
    public GameObject credits;
    public void Play()
    {
        SceneManager.LoadScene("Portix");
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
    public void HowTo()
    {
        howToPlay.SetActive(true);
    }
    public void GoBackFromHowTo()
    {
        howToPlay.SetActive(false);
    }
    public void GoBackFromCredits()
    {
        credits.SetActive(false);
    }
    public void Credits()
    {
        credits.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
