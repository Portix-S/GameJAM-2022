using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private int health = 100;
    public int maxHealth = 100;
    public int xp = 0;

    public TMP_Text healthText;
    public GameObject DeathHUD;
    public FirstPersonController fpsControl;
    public Image healthImage;
    public Sprite[] healthSprites;

    public GameObject[] DropObjects;
    private void Start()
    {
        health = maxHealth;
        healthText.SetText("Vida:\n" + health);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(10);
        }
        if(health > 90)
            healthImage.sprite = healthSprites[0];
        else if(health > 80)
            healthImage.sprite = healthSprites[1];
        else if(health > 60)
            healthImage.sprite = healthSprites[2];
        else if(health > 40)
            healthImage.sprite = healthSprites[3];
        else if (health > 30)
            healthImage.sprite = healthSprites[4];
        else if (health > 20)
            healthImage.sprite = healthSprites[5];
        else
            healthImage.sprite = healthSprites[6];

    }
    public void TakeDamage(int damage)
    {
        if (health > damage)
            health -= damage;
        else
        {
            health = 0;
            Dead();
        }
        healthText.SetText("Vida:\n" + health);
    }

    public void Heal(int amount)
    {
        if(health + amount > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += amount;
        }
        healthText.SetText("Vida:\n" + health);
    }

    void Dead()
    {
        fpsControl.isOnHUD = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        DeathHUD.SetActive(true);
    }
}
