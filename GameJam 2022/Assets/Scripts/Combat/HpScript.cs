using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpScript : MonoBehaviour
{
    public int health = 100;
    public int xpDrop = 10;

    private int xpMax = 100;

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
        PlayerStats playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if(playerScript == null)
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerStats>();

        playerScript.xp += xpDrop; // Adds xp to player
        if (playerScript.xp == xpMax) // Only Drop item when get max xp
        {
            playerScript.xp = 0;
            int randomNum = Random.Range(0, playerScript.DropObjects.Length);
            DropItem(playerScript.DropObjects[randomNum]);
        }
        Destroy(transform.parent.gameObject);
    }

    private void DropItem(GameObject drop)
    {
        Instantiate(drop, gameObject.transform.position, Quaternion.identity);
    }
}
