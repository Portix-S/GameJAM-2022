using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    public int healAmount = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerStats playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            if (playerScript == null)
                playerScript = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerStats>();
            playerScript.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
