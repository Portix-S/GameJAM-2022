using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpScript : MonoBehaviour
{
    public int health = 100;
    public int xpDrop = 10;

    private int xpMax = 100;

    [Header("Referências")]
    public GameObject dropObject;
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
        playerScript.xp += xpDrop; // Adds xp to player
        if (playerScript.xp == xpMax) // Only Drop item when get max xp
        {
            playerScript.xp = 0;
            DropItem();
        }
        Destroy(transform.parent.gameObject);
    }

    private void DropItem()
    {
        Instantiate(dropObject, gameObject.transform.position, Quaternion.identity);
    }
}
