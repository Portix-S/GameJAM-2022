using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("A.I. Settings")]
    [SerializeField] private float sight;
    [SerializeField] private float speed;
    [SerializeField] private float aggressionRange;
    [SerializeField] private float fleeRange;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject[] attackPoints; 
    [SerializeField] private float[] dirMultiplier;
    [SerializeField] private float shootingRate;
    [SerializeField] private float shootForce;
    private bool canShoot = true;

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

                Move(target.position);
                break;
                
            case State.Combat:
                if (distance >= fleeRange) {state = State.Chasing;}

                if (canShoot) {Shoot();}
                
                transform.LookAt(target.position);
                
                break;
        }
    }

    void Move(Vector3 targetPosition){
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
        transform.LookAt(targetPosition);

        //Vector3 direction = Point - transform.position;
        //Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.time);
    }

    void Shoot(){

        for (int i = 0; i < attackPoints.Length; i++)
        {
            Vector3 direction = attackPoints[i].transform.forward + attackPoints[i].transform.right*dirMultiplier[i];

            //INSTANCIAR A BALA, INSTANCIATE BULLET
            GameObject currentBullet = Instantiate(Bullet, attackPoints[i].transform.position, Quaternion.identity);
            //rodar a bala na direcao correta
            currentBullet.transform.forward = direction.normalized;

            //ADD FORCES TO BULLET
            currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
        }
        canShoot = false;
        Invoke("ResetShot", shootingRate + Random.Range(0, 1));
    }

    private void ResetShot()
    {
        canShoot = true;
    }
}
