using UnityEngine;

public class PegasusG3 : FlyerG3
{
    [Header("Distant Attack")]
    [Space]
    public Transform DistantAttackPoint;
    public GameObject BulletPR;
    public GameObject MuzzlePR;
    public AudioClip DistantAttackAudioClip;
    public AudioSource DistantAttackAudioSource;
    public ParticleSystem SmokeMuzzleTrailPS;

    public float ProjectileSpeed;
    public float ProjectileLifeTime;
    public int ProjectileDamage;

    public int DistantAttackAmmoMax;
    protected int distantAttackAmmoCurrent;

    protected bool canDistantAttack;
    protected float distantAttackTimer;
    public float DistantAttackTimerEnd;

    [Space]
    [Header("Other Visuals")]
    public ParticleSystem SmokeTrailPS;

    protected override void Awake()
    {
        base.Awake();
        distantAttackAmmoCurrent = DistantAttackAmmoMax;
        distantAttackTimer = DistantAttackTimerEnd;
    }

    protected override void Update()
    {
        base.Update();
        distantAttackManager();
       // CurrentFlyAnimationSpeed();
    }

    //Race Fuctions

    //ƒобавить фишку с защитными свойствами тарана.
    #region AttackManagment
    protected virtual void UseDistantAttack()
    {
        SmokeMuzzleTrailPS.Play();

        GameObject projectileGO = Instantiate(BulletPR, DistantAttackPoint.position, DistantAttackPoint.rotation);
        ProjectileG3 projectileSC = projectileGO.GetComponent<ProjectileG3>();
        projectileSC.DamageAmount = ProjectileDamage;
        projectileSC.TargetTeamList = teamSeparator.EnemyTeamList;
        projectileSC.Speed = ProjectileSpeed;
        projectileSC.DeactivateTimer = ProjectileLifeTime;
        projectileSC.Team = teamSeparator.Team;

        distantAttackAmmoCurrent -= 1;

        //DistantAttackAudioSource.Play();

        Instantiate(MuzzlePR, DistantAttackPoint.position, DistantAttackPoint.rotation);

        DistantAttackAudioSource.pitch = Random.Range(0.95f, 1.05f);
        DistantAttackAudioSource.PlayOneShot(DistantAttackAudioClip);

        distantAttackTimer = 0f;
    }

    protected virtual void distantAttackManager()
    {
        if (distantAttackTimer < DistantAttackTimerEnd) distantAttackTimer += Time.deltaTime;
        canDistantAttack = distantAttackTimer >= DistantAttackTimerEnd && distantAttackAmmoCurrent >= 1f && !needFlyToCenter;
    }

    #endregion

    #region Damage Reacion

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

    #endregion
}
