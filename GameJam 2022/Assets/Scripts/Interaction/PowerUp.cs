using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GunScript gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunScript>();
            gun.totalAmmo += 10; // PickUp Drop and gain ammo (create new powerups?)
            Destroy(gameObject);
        }
    }

    private void GetRandomPowerUp()
    {

    }
}
