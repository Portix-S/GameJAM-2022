using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GunScript gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunScript>();
            gun.totalPistolAmmo += 10; // PickUp Drop and gain ammo (create new powerups?)
            gun.totalShotgunAmmo += 10; // PickUp Drop and gain ammo (create new powerups?)
            StartCoroutine(ShowText());
        }
    }
    
    IEnumerator ShowText()
    {
        TMP_Text text = GameObject.FindGameObjectWithTag("PowerUpText").GetComponent<TextMeshProUGUI>();
        text.enabled = true;
        text.SetText("+Munição");
        yield return new WaitForSeconds(0.5f);
        text.SetText(" ");
        Destroy(gameObject);
    }
}
