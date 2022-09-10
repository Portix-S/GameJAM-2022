using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("A.I. Settings")]
    [SerializeField] private float sight;
    [SerializeField] private float speed;
    [SerializeField] private float aggressionRange;
    private Transform target;
    private bool inCombat = false;
    private bool canSee = false;
    private bool canHear = false;
    private Vector3 wanderTarget;

    public enum State{
        Wandering,
        Chasing,
        Combat
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<LookAt>().target;
        wanderTarget = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }


    private void FixedUpdate() {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;

        Vector3 direction = target.position - transform.position;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity)){

            Debug.DrawRay(transform.position, direction*hit.distance, Color.yellow);
            Debug.Log("Hit!");
        }
        else {Debug.DrawRay(transform.position, direction*1000, Color.red);Debug.Log("No hit");}
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        switch (state){
            case State.Wandering:
                if (distance <= sight) {state = State.Chasing;}

                if (Vector3.Distance(transform.position, wanderTarget) <= 0.5f)
                {
                    wanderTarget = transform.position +
                                    new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                }

                Move(wanderTarget);

                break;

            case State.Chasing:
                if (distance <= aggressionRange) {state = State.Combat;}
                break;
            case State.Combat:
                Shoot();
                break;
        }

        
        if (distance <= aggressionRange) {inCombat = true;}

        if (!canSee) {Wander();}
        if (canSee && !inCombat) {Move(target.position);}
        else if (canSee && inCombat) {Shoot();}
    }

    void Move(Vector3 targetPosition){
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
        transform.LookAt(targetPosition);

        //Vector3 direction = Point - transform.position;
        //Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.time);
    }

    void Shoot(){

    }

    void Aim(){

    }

    void Wander(){


    }

}
