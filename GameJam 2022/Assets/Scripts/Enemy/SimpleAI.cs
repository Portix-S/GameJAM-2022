using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    private Transform target;
    private float sight;
    private float speed;
    private float aggressionRange;
    private bool inCombat = false;
    private bool canSee = false;
    private bool canHear = false;

    // Start is called before the first frame update
    void Start()
    {
        target = this.GetComponent<LookAt>().target; 
        sight = GetComponent<Stats>().sightRange;
        speed = GetComponent<Stats>().speed;
        aggressionRange = GetComponent<Stats>().aggressionRange;
        Debug.Log(target);
    }

    // Update is called once per frame
    void Update()
    {
        // change to a enumerator
        //if (isDead) {return;}

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= sight) {canSee = true;}
        if (distance <= aggressionRange) {inCombat = true;}

        if (canSee && !inCombat) {Move();}
        else if (canSee && inCombat) {Shoot();}
    }


    void Move(){
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        transform.LookAt(target.position);

        //Vector3 direction = Point - transform.position;
        //Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.time);
    }

    void Shoot(){

    }

    void Aim(){

    }

}
