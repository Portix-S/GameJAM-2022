using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float timer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyObjectAfterTimer()); // AutoDestruct
    }


    IEnumerator DestroyObjectAfterTimer()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
