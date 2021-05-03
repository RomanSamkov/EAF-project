using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerPegasus : FlyerPlayer
{
    public int ShotDamage;
    public float FullShotTimer;
    private float shotTimer;
    private bool canShot;

    public Transform AttackPoint;

    public GameObject AmmoCounterUIObject;
    public AmmoCounter AmmoCounterScript;
    public GameObject RifleBulletPR;
    public GameObject MuzzlePR;

    private bool canAttack;

    public int MaxAmmo;
    public static int CurrentAmmo;

    private GameObject ammoCounterGO;
    private AmmoCounter ammoCounterSC;

    public ParticleSystem SmokeTrailPS;
    public ParticleSystem SmokeMuzzleTrailPS;

    public AudioClip ShotSound;
    public AudioSource ShotAudioSource;

    Animator anim;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();

        CurrentAmmo = MaxAmmo;
    }

    protected override void Start()
    {
        base.Start();

        ammoCounterGO = GameObject.Find("Ammo counter");
        ammoCounterSC = ammoCounterGO.GetComponent<AmmoCounter>();
        ammoCounterSC.SetInt(CurrentAmmo);
    }

    protected override void Update()
    {
        Attack();
        CurrentAnimationSpeed();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!needFlyToCenter)
        {
            FinalSpeed = MoveSpeed + ChangeSpeed * CrossPlatformInputManager.GetAxis("Vertical");
            rotationZ = CrossPlatformInputManager.GetAxis("Horizontal");// * -1;
            //transform.Rotate(0, 0, rotationZ * RotateSpeed * Time.timeScale);

            Vector3 direction = (transform.position + transform.right * rotationZ) - rb.position;
            direction.Normalize();
            Vector3 rotationAmount = Vector3.Cross(transform.up, direction);
            rb.angularVelocity = rotationAmount * (RotateSpeed * Mathf.Abs(rotationZ));

            rb.velocity = transform.up * FinalSpeed;
        }
    }

    private void CurrentAnimationSpeed()
    {
        anim.speed = ((FinalSpeed / MoveSpeed)+1f)/2;
    }

    private void Attack()
    {
        shotTimer += Time.deltaTime;
        if (shotTimer > FullShotTimer)
        {
            canAttack = true;
        }

        if ((Input.GetKey(KeyCode.K) || CrossPlatformInputManager.GetButton("Shot"))&& !needFlyToCenter && canAttack && CurrentAmmo > 0)
        {
            UseMainWeapon();
        }
    }

    public void UseMainWeapon()
    {
        GameObject bulletGO = Instantiate(RifleBulletPR, AttackPoint.position, AttackPoint.rotation);
        //GameObject bulletGO = objectPooler.SpawnFromPool("player_bullet", AttackPoint.position, transform.rotation);
        DistAttackThing bulletSC = bulletGO.GetComponent<DistAttackThing>();
        bulletSC.DamageAmount = ShotDamage;
        bulletSC.TargetTeam = TargetTeam;
        CurrentAmmo--;
        ammoCounterSC.SetInt(CurrentAmmo);

        Instantiate(MuzzlePR, AttackPoint.position, transform.rotation);
        //objectPooler.SpawnFromPool("muzzle", AttackPoint.position, transform.rotation);
        SmokeMuzzleTrailPS.Play();

        //Sound
        ShotAudioSource.pitch += Random.Range(-0.8f, 0.8f);
        ShotAudioSource.PlayOneShot(ShotSound);
        ShotAudioSource.pitch = 1f;

        canAttack = false;
        shotTimer = 0f;
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
}
