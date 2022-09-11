using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        TMP_Text text = GameObject.FindGameObjectWithTag("PowerUpText").GetComponent<TextMeshProUGUI>();
        text.enabled = true;
        text.SetText("+Vida");
        yield return new WaitForSeconds(0.5f);
        text.SetText(" ");
        Destroy(gameObject);
    }
}
