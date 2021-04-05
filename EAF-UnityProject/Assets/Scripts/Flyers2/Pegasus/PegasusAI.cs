using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegasusAI : Flyer
{
    public int ShotDamage;
    private float shotTimer;
    public float FullShotTimer;

    public Transform AttackPoint;

    public GameObject BulletPF;
    public GameObject MuzzlePF;

    public ParticleSystem SmokeTrailPS;
    public ParticleSystem SmokeMuzzleTrailPS;

    public AudioClip ShotSound;
    public AudioSource ShotAudioSource;

    private GameObject actualTargetGO;

    //ObjectPooler objectPooler;
    protected override void Awake()
    {
        base.Awake();

        shotTimer = FullShotTimer;

        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Alive)
        {
            RandomizeMobility();
            ArrowHost();
            FindAimDistance(15f);
            FindActualDistance();
            if (!needFlyToCenter) Attack();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void Attack()
    {
        shotTimer += Time.deltaTime;

        if (shotTimer > FullShotTimer && aimDistance < 12f)
        {
            UseMainAttack();
        }
    }

    void UseMainAttack()
    {

        //GameObject bulletGO = objectPooler.SpawnFromPool("RifleBullet", transform.position, transform.rotation);
        GameObject bulletGO = Instantiate(BulletPF, AttackPoint.position, transform.rotation);
        DistAttackThing bulletSC = bulletGO.GetComponent<DistAttackThing>();
        bulletSC.DamageAmount = ShotDamage;
        bulletSC.TargetTeam = TargetTeam;

        Instantiate(MuzzlePF, AttackPoint.position, transform.rotation);
        //objectPooler.SpawnFromPool("muzzle", AttackPoint.position, transform.rotation);
        SmokeMuzzleTrailPS.Play();

        //Sound
        ShotAudioSource.pitch += Random.Range(-0.8f, 0.8f);
        ShotAudioSource.PlayOneShot(ShotSound);
        ShotAudioSource.pitch = 1f;

        shotTimer = 0f;
    }

    protected override void UpdateTarget()
    {
        GameObject[] flyers = GameObject.FindGameObjectsWithTag("Flyers");
        float shortestDist = Mathf.Infinity;

        GameObject nearestEnemy = null;
        TargetNumber = 0;
        foreach (GameObject flyer in flyers)
        {
            TeamSeparator team = flyer.GetComponent<TeamSeparator>();
            if (team.Team == TargetTeam)
            {
                TargetNumber++;
                float distance = Vector3.Distance(transform.position, flyer.transform.position);
                if (distance < shortestDist)
                {
                    shortestDist = distance;
                    nearestEnemy = flyer;
                }
            }
            else
            {

            }
        }
        if (TargetNumber > 0)
        {
            actualDistance = shortestDist;
        }
        else
        {
            actualDistance = 0;
        }

        if (TargetNumber > 0)
        {
            actualTargetPos = nearestEnemy.transform.position;
            actualTargetGO = nearestEnemy;
            if (shortestDist > 25f)
            {
                Target = nearestEnemy.transform.position + nearestEnemy.transform.up * 10f;
            }
            else
            {
                Target = nearestEnemy.transform.position;
            }
        }
        
    }

    public override void RamHitVisual()
    {
        SmokeTrailPS.Stop();
        SmokeTrailPS.Clear();
        var mainParticleSystem = SmokeTrailPS.main;
        mainParticleSystem.duration = 2f;
        SmokeTrailPS.Play();
    }

    public override void BlastHitVisual()
    {
        SmokeTrailPS.Stop();
        SmokeTrailPS.Clear();
        var mainParticleSystem = SmokeTrailPS.main;
        mainParticleSystem.duration = 1f;
        SmokeTrailPS.Play();
    }

    public override void DeathVisual()
    {
        SmokeTrailPS.Play();
    }

    protected override void RandomizeMobility()
    {
        RandomizeTimer += Time.deltaTime;
        if (RandomizeTimer > fullRandomizeTimer && CanRandomizeMobility)
        {
            //Debug.Log("Random");
            MoveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);
            RotateSpeed = Random.Range(MinRotateSpeed, MaxRotateSpeed);
            RandomizeTimer = 0f;
        }
    }
}
