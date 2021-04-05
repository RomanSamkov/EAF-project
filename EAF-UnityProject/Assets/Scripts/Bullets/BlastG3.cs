using UnityEngine;

public class BlastG3 : ProjectileG3
{
    public GameObject StandartHitPF;

    protected override void Start()
    {
        base.Start();
        //objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody>();
        DamageType = "blast";
        Invoke("DeactivateGameObject", DeactivateTimer);
    }

    void OnTriggerEnter(Collider thing)
    {
        TargetTestAttack(thing);
    }

    public override void EnemyBulletCollide()
    {
        Instantiate(StandartHitPF, transform.position, transform.rotation);
        DeactivateGameObject();
    }

    public override void EnemyBlastCollide()
    {
        Instantiate(StandartHitPF, transform.position, transform.rotation);
        DeactivateGameObject();
    }

    public override void EnemyChnCollide()
    {
        Instantiate(StandartHitPF, transform.position, transform.rotation);
        DeactivateGameObject();
    }      
           
    public override void EnemyPgsCollide()
    {

        Instantiate(StandartHitPF, transform.position, transform.rotation);
        DeactivateGameObject();
    }      
           
    public override void EnemyGrfCollide()
    {
        Instantiate(StandartHitPF, transform.position, transform.rotation);
        DeactivateGameObject();
    }      
           
    public override void EnemyDefCollide()
    {
        Instantiate(StandartHitPF, transform.position, transform.rotation);
        DeactivateGameObject();
    }
}
