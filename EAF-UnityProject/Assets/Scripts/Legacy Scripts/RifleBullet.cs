using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : DistAttackThing
{
    public GameObject ChnCollidePF;
    public GameObject PgsCollidePF;
    public GameObject DefaultCollidePF;

    void Start()
    {
        //objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody>();
        DamageType = "bullet";
        Invoke("DeactivateGameObject", DeactivateTimer);
    }

    void FixedUpdate()
    {
        FlyForward();
    }

    void OnTriggerEnter(Collider thing)
    {
        TargetTestAttack(thing);
    }

    public override void EnemyBulletCollide()
    {
        DeactivateGameObject();
    }

    public override void EnemyBlastCollide()
    {
        DeactivateGameObject();
    }

    public override void EnemyChnCollide()
    {
        Instantiate(ChnCollidePF, transform.position, transform.rotation);
        DeactivateGameObject();
    }      
           
    public override void EnemyPgsCollide()
    {

        Instantiate(PgsCollidePF, transform.position, transform.rotation);
        DeactivateGameObject();
    }      
           
    public override void EnemyGrfCollide()
    {
        Instantiate(DefaultCollidePF, transform.position, transform.rotation);
        DeactivateGameObject();
    }      
           
    public override void EnemyDefCollide()
    {
        Instantiate(DefaultCollidePF, transform.position, transform.rotation);
        DeactivateGameObject();
    }
}
