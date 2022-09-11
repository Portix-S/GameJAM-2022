using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GunScript gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunScript>();
            gun.smgDamage += 5; 
            gun.pistolDamage += 5; 
            gun.shotgunDamage += 1;
            Destroy(gameObject);
        }
    }

}
