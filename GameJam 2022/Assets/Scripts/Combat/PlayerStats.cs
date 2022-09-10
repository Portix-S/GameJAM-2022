using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public int xp = 0;

    public GameObject DeathHUD;
    public FirstPersonController fpsControl;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(10);
        }
    }
    public void TakeDamage(int damage)
    {
        if(health > damage)
            health -= damage;
        else
        {
            health = 0;
            Dead();
        }
    }

    void Dead()
    {
        fpsControl.isOnHUD = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        DeathHUD.SetActive(true);
    }
}
