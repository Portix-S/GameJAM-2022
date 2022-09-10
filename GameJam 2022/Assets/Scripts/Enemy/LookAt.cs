using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
    public Sprite[] spriteList;
    private GameObject sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null){
            ChangeSprite();
            sprite.transform.LookAt(target);
        }   
    }

    void ChangeSprite(){

        float angle = Vector3.SignedAngle(target.forward, transform.forward, transform.up);
        
        if (angle >= -45 && angle < 45) {sprite.GetComponent<SpriteRenderer>().sprite = spriteList[0];}
        else if (angle >= 45 && angle < 135) {sprite.GetComponent<SpriteRenderer>().sprite = spriteList[1];}
        else if (angle >= 135 || angle < -135) {sprite.GetComponent<SpriteRenderer>().sprite = spriteList[2];}
        else if (angle >= -135 && angle < -45) {sprite.GetComponent<SpriteRenderer>().sprite = spriteList[3];}

    }
}
