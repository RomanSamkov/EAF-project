using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangelingAI : Flyer
{
    public GameObject Ram;

    public int ShotDamage;
    private float shotTimer;
    public float FullShotTimer;

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

    //ObjectPooler objectPooler;
    protected override void Awake()
    {
        base.Awake();

        shotTimer = FullShotTimer;
        fullDisguiseTimer = DisguiseTimer;

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
            FindAimDistance(actualDistance);
            if (CanDisguise && !needFlyToCenter) Disguise();
            if(aimDistance < 1f && actualDistance<10f)
            {
                disguise.SetActive(false);
                disguiseActive = false;
            }
            if (!disguiseActive)
            {
                ArrowHost();
                RandomizeMobility();
                if (!needFlyToCenter)
                {
                    Attack();
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
        if (actualDistance > 10 && shotTimer > FullShotTimer && aimDistance < 15f)
        {
             UseMainAttack();
        }
        else if (actualDistance <= 10)
        {
            if (Vector3.Distance(Target, transform.position + transform.up * 5f) < 6.5f)
            {
                ActivateRam(true);
            }
            else
            {
                ActivateRam(false);
            }
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

    //Ram on collide
    void OnTriggerEnter(Collider thing)
    {
        if(thing.tag == "Flyers" && Ram.activeSelf)
        {
            TeamSeparator team = thing.GetComponent<TeamSeparator>();
            if (team.Team == TargetTeam)
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
        if (DisguiseTimer > fullDisguiseTimer) {
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

