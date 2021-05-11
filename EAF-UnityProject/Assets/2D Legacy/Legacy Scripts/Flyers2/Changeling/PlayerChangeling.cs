using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerChangeling : FlyerPlayer
{
    public int ShotDamage;
    public float FullShotTimer;
    private float shotTimer;
    private bool canShot;

    public float ShotMaxEnergy;
    public float ShotEnergy;
    private float canShotRegenTimer;

    public GameObject Ram;

    public float RamSpeed;
    public float RamRotation;
    public int RamDamageAmount;

    public float RamEnergy;
    public float RamMaxEnergy;
    private float canRamRegenTimer;

    public bool CanDisguise;
    public GameObject Disguise;
    public float DisguiseMinDistance;
    private bool disguiseActive;

    private GameObject RamEnergyBarGO;
    private EnergyBar RamEnergyBarSC;

    private GameObject ShotEnergyBarGO;
    private EnergyBar ShotEnergyBarSC;

    public GameObject BlastPR;
    public GameObject GreenRamExplosionV;

    public AudioSource ShotSound;

    public ParticleSystem GreenTrailPS;
    public ParticleSystem GreenExplosionPS;
    public ParticleSystem GreenRamExplosionPS;
    public ParticleSystem GreenHornMagiclPS;
    public ParticleSystem SmokeTrailPS;

    protected override void Awake()
    {
        base.Awake();

        shotTimer = FullShotTimer;
        ShotEnergy = ShotMaxEnergy;
    }

    protected override void Start()
    {
        base.Start();
        
        RamEnergyBarGO = GameObject.Find("Ram Energy bar");
        RamEnergyBarSC = RamEnergyBarGO.GetComponent<EnergyBar>();
        RamEnergyBarSC.SetMaxEnergy(RamMaxEnergy);

        ShotEnergyBarGO = GameObject.Find("Shot Energy bar");
        ShotEnergyBarSC = ShotEnergyBarGO.GetComponent<EnergyBar>();
        ShotEnergyBarSC.SetMaxEnergy(ShotMaxEnergy);

        FindBorders();
    }

    protected override void Update()
    {
        Attack();
    }

    public void Attack()
    {
        shotTimer += Time.deltaTime;
        if (shotTimer > FullShotTimer)
        {
            canShot = true;
        }

        if ((Input.GetKey(KeyCode.K) || CrossPlatformInputManager.GetButton("Shot")) && !needFlyToCenter && canShot && ShotEnergy > 1 && !Ram.activeSelf)
        {
            UseDistAttack();
            canShotRegenTimer = 0;
        }
        else
        {
            canShotRegenTimer += Time.deltaTime;
            if (ShotEnergy < ShotMaxEnergy && canShotRegenTimer > 1)
            {
                ShotEnergy += 0.2f * Time.deltaTime;
                ShotEnergyBarSC.SetEnergy(ShotEnergy);
            }
        }

        if ((Input.GetKey(KeyCode.I) || CrossPlatformInputManager.GetButton("Ram")) && !needFlyToCenter && RamEnergy>0)
        {
            ActivateRam(true);
            RamEnergy -= Time.deltaTime;
            RamEnergyBarSC.SetEnergy(RamEnergy);
            canRamRegenTimer = 0f;
        }
        else
        {
            ActivateRam(false);
            canRamRegenTimer += Time.deltaTime;
            if (RamEnergy < RamMaxEnergy && canRamRegenTimer > 2f)
            {
                RamEnergy += 0.5f * Time.deltaTime;
                RamEnergyBarSC.SetEnergy(RamEnergy);
            }
        }
    }

    public void UseDistAttack()
    {
        GreenHornMagiclPS.Play();
        GameObject blastGO = Instantiate(BlastPR, transform.position, transform.rotation);
        //GameObject bulletGO = objectPooler.SpawnFromPool("player_bullet", AttackPoint.position, transform.rotation);
        DistAttackThing blastSC = blastGO.GetComponent<DistAttackThing>();
        blastSC.DamageAmount = ShotDamage;
        blastSC.TargetTeam = TargetTeam;
        ShotEnergy -= 1f;
        //objectPooler.SpawnFromPool("muzzle", AttackPoint.position, transform.rotation);
        ShotEnergyBarSC.SetEnergy(ShotEnergy);

        ShotSound.Play();

        canShot = false;
        shotTimer = 0f;
    }

    public void ActivateRam(bool i)
    {
        if (i)
        {
            Ram.SetActive(true);
            GreenTrailPS.Play();
            MoveSpeed = RamSpeed;
            RotateSpeed = RamRotation;
        }
        else if (!i)
        {
            Ram.SetActive(false);
            GreenTrailPS.Pause();
            MoveSpeed = prMoveSpeed;
            RotateSpeed = prRotateSpeed;
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!needFlyToCenter)
        {
            FinalSpeed = MoveSpeed + ChangeSpeed * CrossPlatformInputManager.GetAxis("Vertical");
            rotationZ = CrossPlatformInputManager.GetAxis("Horizontal");

            Vector3 direction = (transform.position + transform.right * rotationZ) - rb.position;
            direction.Normalize();
            Vector3 rotationAmount = Vector3.Cross(transform.up, direction);
            rb.angularVelocity = rotationAmount * (RotateSpeed * Mathf.Abs(rotationZ));

            rb.velocity = transform.up * FinalSpeed;
        }
    }

    public override void RamHitVisual()
    {
        var mainParticleSystem = SmokeTrailPS.main;
        SmokeTrailPS.Pause();
        mainParticleSystem.duration = 2f;
        SmokeTrailPS.Play();
    }

    public override void BulletHitVisual()
    {

    }

    public override void BlastHitVisual()
    {
        var mainParticleSystem = SmokeTrailPS.main;
        SmokeTrailPS.Pause();
        mainParticleSystem.duration = 1f;
        SmokeTrailPS.Play();
    }
}

