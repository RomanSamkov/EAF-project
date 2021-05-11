using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangelingProAI : Flyer
{
    public GameObject Ram;

    public int ShotDamage;
    private float shotTimer;
    public float FullShotTimer;
    public float BlastSpeed;

    public float RamSpeed;
    public float RamRotation;
    public int RamDamageAmount;

    public bool CanDisguise;
    public float DisguiseTimer;
    private float fullDisguiseTimer;
    public GameObject disguise;
    public float DisguiseMinDistance;
    private bool disguiseActive;

    public GameObject BlastPR;
    public GameObject GreenRamExplosionV;

    public AudioSource ShotSound;

    public ParticleSystem GreenTrailPS;
    public ParticleSystem GreenExplosionPS;
    public ParticleSystem GreenRamExplosionPS;
    public ParticleSystem GreenHornMagiclPS;
    public ParticleSystem SmokeTrailPS;

    private GameObject actualTargetGO;

    private bool hitDanger = false;

    //ObjectPooler objectPooler;
    protected override void Awake()
    {
        base.Awake();

        shotTimer = FullShotTimer;
        fullDisguiseTimer = DisguiseTimer;

        InvokeRepeating("UpdateTarget", 0f, 0.02f);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Alive)
        {
            FindAimDistance(targetDistance);
            FindTargetDistance();
            if (CanDisguise && !needFlyToCenter) Disguise();
            if (aimDistance < 1f && actualDistance < 10f)
            {
                disguise.SetActive(false);
                disguiseActive = false;
            }
            if (!disguiseActive)
            {
                ArrowHost();
                if (!needFlyToCenter && TargetNumber>0)
                {
                    Attack();
                }
                if(TargetNumber == 0)
                {
                    ActivateRam(false);
                }
            }
        }
    }

    protected override void FixedUpdate()
    {
        RandomizeDirection();
        BorderControl();
        FinalSpeed = MoveSpeed;

        if (Alive && TargetNumber > 0 && !needFlyToCenter)
        {
            if (!disguiseActive)
            {
                FlyToVector(Target);
            }
            else
            {
                RotateToVector(Target);

                rb.velocity = transform.up * 0f;
            }

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
        else if (Alive && TargetNumber > 0 && hitDanger)
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

    public void Attack()
    {
        shotTimer += Time.deltaTime;
        if (/*40*/200 > actualDistance && actualDistance > 5 && shotTimer > FullShotTimer && aimDistance < 2f && !Ram.activeSelf)
        {
            UseMainAttack();
        }

        if (actualDistance <= 6.5f)
        {
            if (Vector3.Distance(Target, transform.position + transform.up * targetDistance) < 6.5f)
            {
                ActivateRam(true);
            }   
        }
        else
        {
            ActivateRam(false);
        }

        if (actualDistance > 10)
        {
            if (MoveSpeed < MaxMoveSpeed) { MoveSpeed += MaxMoveSpeed * Time.deltaTime; }
            if (RotateSpeed > MinRotateSpeed) { RotateSpeed -= MinRotateSpeed * Time.deltaTime; }
        }
        else if (actualDistance <= 10)
        {
            if (RotateSpeed < MaxRotateSpeed) { RotateSpeed += MaxRotateSpeed * Time.deltaTime; }
            Flyer nearestEnemySC = actualTargetGO.GetComponent<Flyer>();
            float speed = nearestEnemySC.FinalSpeed;
            
            if (Vector3.Distance((actualTargetPos + actualTargetGO.transform.up * actualDistance), transform.position) < actualDistance)
            {
                hitDanger = true;
            }
            else
            {
                hitDanger = false;
            }

            if (Vector3.Distance(Target, transform.position + transform.up * targetDistance) > 3f) {
                if (MinMoveSpeed <= speed && speed <= MaxMoveSpeed && !hitDanger)
                {
                    MoveSpeed = speed + 2f;
                }
                else if (MinMoveSpeed > speed && !hitDanger)
                {
                    MoveSpeed = MinMoveSpeed;
                }
                else if (MaxMoveSpeed < speed && !hitDanger)
                {
                    MoveSpeed = MaxMoveSpeed;
                }
            }
        }
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
            //Target = nearestEnemy.transform.position + nearestEnemy.transform.up * (actualDistance*(speed/35));
            //Testing new system
            Target = nearestEnemy.transform.position + nearestEnemy.transform.up * (speed / BlastSpeed)*(actualDistance / BlastSpeed) * speed;
            //Debug.Log("add a "+ (speed / BlastSpeed) * (actualDistance / BlastSpeed)*speed+" to target");
            //
            actualTargetPos = nearestEnemy.transform.position;
            actualTargetGO = nearestEnemy;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Target, 2);

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
            Flyer nearestEnemySC = nearestEnemy.GetComponent<Flyer>();
            float speed = nearestEnemySC.FinalSpeed;
            //Target = nearestEnemy.transform.position + nearestEnemy.transform.up * (actualDistance*(speed/35));
            //Testing new system
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + transform.up * actualDistance, 1);

        }
    }

    void UseMainAttack()
    {
        GreenHornMagiclPS.Play();
        //GameObject blastGO = objectPooler.SpawnFromPool("blast", transform.position, transform.rotation);
        GameObject blastGO = Instantiate(BlastPR, transform.position, transform.rotation);
        DistAttackThing blastSC = blastGO.GetComponent<DistAttackThing>();
        blastSC.DamageAmount = ShotDamage;
        blastSC.TargetTeam = TargetTeam;
        blastSC.Speed = BlastSpeed;
        //
        blastSC.DeactivateTimer = 15;
        //

        ShotSound.Play();

        shotTimer = 0f;
    }

    public void ActivateRam(bool i)
    {
        if (i)
        {
            Ram.SetActive(true);
            GreenTrailPS.Play();
            MoveSpeed = Random.Range(RamSpeed - 2f, RamSpeed + 2f);
            RotateSpeed = Random.Range(RamRotation - 0.2f, RamRotation + 0.2f);
            CanRandomizeMobility = false;
        }
        else if (!i)
        {
            Ram.SetActive(false);
            GreenTrailPS.Pause();
            CanRandomizeMobility = true;
        }
    }

    void OnTriggerEnter(Collider thing)
    {
        if (thing.tag == "Flyers")
        {
            TeamSeparator team = thing.GetComponent<TeamSeparator>();
            if (team.Team == TargetTeam && Ram.activeSelf)
            {
                Flyer target = thing.GetComponent<Flyer>();
                Instantiate(GreenRamExplosionV, transform.position, transform.rotation);
                //GreenRamExplosionPS.Play();
                target.SetDamage(RamDamageAmount, "ram");
            }
        }
    }

    void Disguise()
    {
        DisguiseTimer += Time.deltaTime;
        if (DisguiseTimer > fullDisguiseTimer)
        {
            int i = Random.Range(0, 5);

            if (actualDistance > DisguiseMinDistance && actualDistance < DetectDistance + 10f && aimDistance > 8f && Alive && i < 3)
            {
                disguise.SetActive(true);
                Ram.SetActive(false);
                Arrow.SetActive(false);
                GreenTrailPS.Pause();
                AvailableAsTarget = false;
                disguiseActive = true;
            }
            else
            {
                disguise.SetActive(false);
                AvailableAsTarget = true;
                disguiseActive = false;
            }
            fullDisguiseTimer = Random.Range(0.5f, 3);
            DisguiseTimer = 0f;
        }
    }

    public override void SetDeathSettings()
    {
        base.SetDeathSettings();

        if (disguiseActive) { MoveSpeed = 0f; }

        Ram.SetActive(false);
        GreenTrailPS.Pause();

        Arrow.SetActive(false);

        disguise.SetActive(false);
        disguiseActive = false;

    }

    public override void DeathVisual()
    {
        GreenExplosionPS.Play();
        SmokeTrailPS.Play();
    }
}
