using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBlast : DistAttackThing//, PooledObject
{
    public GameObject GreenExplosionCollidePF;

    public ParticleSystem ps;
    public SpriteRenderer srendererV;

    void Start()
    {
        //objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody>();
        bcollider = GetComponent<BoxCollider>();
        srenderer = GetComponent<SpriteRenderer>();
        srendererV = srendererV.GetComponent<SpriteRenderer>();
        DamageType = "blast";
        //Invoke("DeactivateGameObject", DeactivateTimer);
        EndOfFly(DeactivateTimer, 1);
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
        SetHalfActive();
        Instantiate(GreenExplosionCollidePF, transform.position, transform.rotation);
    }

    public override void EnemyBlastCollide()
    {
        SetHalfActive();
        Instantiate(GreenExplosionCollidePF, transform.position, transform.rotation);
    }

    public override void EnemyChnCollide()
    {
        SetHalfActive();
        Instantiate(GreenExplosionCollidePF, transform.position, transform.rotation);
    }

    public override void EnemyPgsCollide()
    {
        SetHalfActive();
        Instantiate(GreenExplosionCollidePF, transform.position, transform.rotation);
    }

    public override void EnemyGrfCollide()
    {
        SetHalfActive();
        Instantiate(GreenExplosionCollidePF, transform.position, transform.rotation);
    }

    public override void EnemyDefCollide()
    {
        SetHalfActive();
        Instantiate(GreenExplosionCollidePF, transform.position, transform.rotation);
    }

    public override void SetHalfActive()
    {
        base.SetHalfActive();
        ps.Stop();
        srendererV.enabled = false;
    }
}
