using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GunScript gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunScript>();
            gun.pistolDamage += 5; 
            gun.shotgunDamage += 2;
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        TMP_Text text = GameObject.FindGameObjectWithTag("PowerUpText").GetComponent<TextMeshProUGUI>();
        text.enabled = true;
        text.SetText("+Dano");
        yield return new WaitForSeconds(0.5f);
        text.SetText(" ");
        Destroy(gameObject);
    }

}
