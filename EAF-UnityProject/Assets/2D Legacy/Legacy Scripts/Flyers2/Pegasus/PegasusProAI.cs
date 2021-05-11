using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegasusProAI : Flyer
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

    private bool collideDanger = false;

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
            ArrowHost();
            FindAimDistance(targetDistance);
            FindTargetDistance();
            FindActualDistance();
            if(!needFlyToCenter)Attack();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void AutoFly()
    {
        RandomizeDirection();
        BorderControl();
        if (Alive && TargetNumber > 0 && !collideDanger && !needFlyToCenter)
        {
            FlyToVector(Target);
            if (needResetSpeed)
            {
                SetCurMinMaxMoveSpeed(prMoveSpeed, prMinMoveSpeed, prMaxMoveSpeed);
                SetCurMinMaxRotateSpeed(prRotateSpeed, prMinRotateSpeed, prMaxRotateSpeed);
                needResetSpeed = false;
            }
        }
        else if (Alive && needFlyToCenter)
        {
            FlyToVector(CentralMapPoint);
        }
        else if (Alive && TargetNumber > 0 && collideDanger)
        {
            FlyAround();
        }
        else if (Alive && TargetNumber == 0)
        {
            FlyAround();
            SetCurMinMaxMoveSpeed(7f, 4f, 8f);
            SetCurMinMaxRotateSpeed(0.6f, 0.4f, 1f);
            needResetSpeed = true;
        }
        if (!Alive) DeathFall();
    }

    private void Attack()
    {
        shotTimer += Time.deltaTime;

        if (40 > actualDistance && shotTimer > FullShotTimer && aimDistance < 8f)
        {
            UseMainAttack();
        }

        if (actualDistance > 20)
        {
            if (MoveSpeed < MaxMoveSpeed) { MoveSpeed += MaxMoveSpeed * Time.deltaTime; }
            if (RotateSpeed > MinRotateSpeed) { RotateSpeed -= MinRotateSpeed * Time.deltaTime; }    
        }
        else if(actualDistance <= 20)
        {
            //if (MoveSpeed > MinMoveSpeed) { MoveSpeed -= MinMoveSpeed * Time.deltaTime; }
            if (RotateSpeed < MaxRotateSpeed) { RotateSpeed += MaxRotateSpeed * Time.deltaTime; }

            if (Vector3.Distance((actualTargetPos + actualTargetGO.transform.up * actualDistance), transform.position) < actualDistance)
            {
                collideDanger = true;
            }
            else
            {
                collideDanger = false;
            }

            if (!collideDanger && actualDistance < 10f)
            {
                Flyer nearestEnemySC = actualTargetGO.GetComponent<Flyer>();
                float speed = nearestEnemySC.FinalSpeed;
                if (MinMoveSpeed <= speed && speed <= MaxMoveSpeed)
                {
                    MoveSpeed = speed;
                }
                else if (MinMoveSpeed > speed)
                {
                    MoveSpeed = MinMoveSpeed;
                }
                else if (MaxMoveSpeed < speed)
                {
                    MoveSpeed = MaxMoveSpeed;
                }
            }
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
        ShotAudioSource.pitch += Random.Range(-0.1f, 0.1f);
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
            Flyer nearestEnemySC = nearestEnemy.GetComponent<Flyer>();
            float speed = nearestEnemySC.FinalSpeed;

            Target = nearestEnemy.transform.position + nearestEnemy.transform.up * (actualDistance * (speed / 40));

            actualTargetPos = nearestEnemy.transform.position;
            actualTargetGO = nearestEnemy;
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
}
