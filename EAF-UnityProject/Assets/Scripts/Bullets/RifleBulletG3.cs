using UnityEngine;

public class RifleBulletG3 : ProjectileG3
{
    public GameObject ChnCollidePF;
    public GameObject PgsCollidePF;
    public GameObject DefaultCollidePF;

    protected override void Start()
    {
        base.Start();
        //objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody>();
        DamageType = "bullet";
        Invoke("DeactivateGameObject", DeactivateTimer);
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
