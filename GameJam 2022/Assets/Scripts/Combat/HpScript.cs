using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpScript : MonoBehaviour
{
    public int health = 100;
    public int xpDrop = 10;

    private int xpMax = 100;
    PlayerStats playerScript;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerScript == null)
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerStats>();
    }
    public void TakeDamage(int damage)
    {
        if (health >= damage)
            health -= damage;
        else if(health >= 0)
        {
            health = -1; // So it doesn't die() twice;
            Die();
        }
    }

    private void Die()
    {
        

        playerScript.xp += xpDrop; // Adds xp to player
        if (playerScript.xp >= xpMax && tag == "Enemy") // Only Drop item when get max xp
        {
            playerScript.xp = 0;
            int randomNum = Random.Range(0, playerScript.DropObjects.Length);
            DropItem(playerScript.DropObjects[randomNum]);
        }
        else if(tag == "FlyingEnemy")
        {
            GunScript gunScript = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunScript>();
            gunScript.totalShotgunAmmo += 4;
            gunScript.totalPistolAmmo += 5;
        }
        playerScript.killedEnemies++;
        if (playerScript.killedEnemies == playerScript.numberOfEnemies)
            playerScript.WinGame();
        Destroy(transform.parent.gameObject);
    }

    private void DropItem(GameObject drop)
    {
        Instantiate(drop, gameObject.transform.position, Quaternion.identity);
    }
}
