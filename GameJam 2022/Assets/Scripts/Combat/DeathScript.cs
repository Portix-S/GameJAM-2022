using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class DeathScript : MonoBehaviour
{
    public FirstPersonController fpsControl;

    public void TryAgain()
    {
        fpsControl.isOnHUD = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Portix");
    }
}
