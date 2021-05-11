using UnityEngine;

public class ChangelingG3 : FlyerG3
{
    [Space]
    [Header("Changeling Settings")]
    [Header("Distant Attack")]
    public GameObject BlastPR;
    public AudioSource DistantAttackSound;
    public ParticleSystem GreenHornMagiclPS;
    public float DistantAttackTimerEnd;
    public float ProjectileSpeed;
    public float ProjectileLifeTime;
    public int ProjectileDamage;

    public float DistantAttackMaxEnergy;
    protected float distantAttackEnergyCurrent;

    protected float distantAttackRegenTimer;
    public float distantAttackRegenTimerEnd;

    protected bool canDistantAttack;
    protected float distantAttackTimer;
    //protected bool canDistantRegen = false;

    [Space]
    [Header("Melee Attack")]
    //Melee Parameters
    public GameObject Ram;
    public GameObject GreenRamExplosionV;
    public ParticleSystem GreenTrailPS;
    public bool RamActive;
    public float RamEnergyMax;
    //protected float ramCurrentEnergy;
    public float ramEnergyCurrent;
    public float RamSpeed;
    public float RamRotation;
    public int RamDamageAmount;
    public float ramRegenTimer;
    protected bool canRam;
    [Space]
    [Header("Disguise")]
    public bool CanDisguise;
    public float DisguiseTimer;
    protected float fullDisguiseTimer;
    public GameObject disguise;
    public float DisguiseMinDistance;
    protected bool disguiseActive;
    [Space]
    [Header("Other Visuals")]
    public ParticleSystem GreenExplosionPS;
    public ParticleSystem SmokeTrailPS;

    protected override void Awake()
    {
        base.Awake();
        Race = "chn";
        distantAttackEnergyCurrent = DistantAttackMaxEnergy;
        ramEnergyCurrent = RamEnergyMax;
    }

    protected override void Update()
    {
        base.Update();

        ramManager();
        distantAttackManager();

       // Debug.Log($"Current ram energy is {ramEnergyCurrent} and current dist attack energy is {distantAttackCurrentEnergy}");
    }

    //Race Fuctions

    protected virtual void UseDistantAttack()
    {
        GreenHornMagiclPS.Play();

        GameObject projectileGO = Instantiate(BlastPR, transform.position, transform.rotation);
        ProjectileG3 projectileSC = projectileGO.GetComponent<ProjectileG3>();
        //Set projectile variables
        projectileSC.DamageAmount = ProjectileDamage;
        projectileSC.TargetTeamList = teamSeparator.EnemyTeamList;
        projectileSC.Speed = ProjectileSpeed;
        projectileSC.DeactivateTimer = ProjectileLifeTime;
        projectileSC.Team = teamSeparator.Team;

        distantAttackEnergyCurrent -= 1f;

        DistantAttackSound.Play();

        distantAttackTimer = 0f;

        distantAttackRegenTimer = 0f;
    }
    
    public void ToggleRam(bool i)
    {
        if (i)
        {
            Ram.SetActive(true);
            RamActive = true;
            GreenTrailPS.Play();
            MoveSpeed = Random.Range(RamSpeed - 2f, RamSpeed + 2f);
            RotateSpeed = Random.Range(RamRotation - 0.2f, RamRotation + 0.2f);
            //CanRandomizeMobility = false;
        }
        else
        {
            Ram.SetActive(false);
            RamActive = false;
            GreenTrailPS.Pause();
            ResetMovement();
        }
    }

    protected virtual void ramManager()
    {
        if (RamActive && (ramEnergyCurrent - Time.deltaTime > 0f))
        {
            ramEnergyCurrent -= Time.deltaTime;
            ramRegenTimer = 0f;
        }
        else if (RamActive && ramEnergyCurrent - Time.deltaTime <= 0)
        {
            ramEnergyCurrent = 0f;
            ToggleRam(false);
        }
        else if (!RamActive && ramRegenTimer < 2f)
            ramRegenTimer += Time.deltaTime;

        if (ramRegenTimer >= 2f && ramEnergyCurrent < RamEnergyMax)
        {
            ramEnergyCurrent += 0.5f * Time.deltaTime;
        }

        canRam = ramEnergyCurrent > 0 && !needFlyToCenter;
    }

    protected virtual void distantAttackManager()
    {
        if (distantAttackRegenTimer < distantAttackRegenTimerEnd)
        {
            distantAttackRegenTimer += Time.deltaTime;
        }
        else if (distantAttackEnergyCurrent < DistantAttackMaxEnergy && distantAttackRegenTimer >= distantAttackRegenTimerEnd)
        {
            distantAttackEnergyCurrent += Time.deltaTime;
            if (distantAttackEnergyCurrent > DistantAttackMaxEnergy) distantAttackEnergyCurrent = DistantAttackMaxEnergy;
        }
        if(distantAttackTimer<DistantAttackTimerEnd) distantAttackTimer += Time.deltaTime;
        canDistantAttack = distantAttackTimer >= DistantAttackTimerEnd && distantAttackEnergyCurrent >= 1f && !needFlyToCenter && !RamActive;
    }

    //Ram collide
    void OnTriggerEnter(Collider thing)
    {
        if (thing.tag == "Flyers" && RamActive)
        {
            //Check if forward direction
            Vector3 thingPos = thing.transform.position;

            //if central point of changeling is father than forward, then it is ram
            if (Vector3.Distance(thingPos, transform.position)>Vector3.Distance(thingPos, transform.position+transform.up*1f))
            {
                TeamSeparatorG3 selectedFlyerTeam = thing.GetComponent<TeamSeparatorG3>();
                FlyerG3 flyerSC = thing.GetComponent<FlyerG3>();

                //Check if Enemy
                bool ItIsMyEnemy = false;
                foreach (string team in teamSeparator.EnemyTeamList)
                {
                    if (selectedFlyerTeam.Team == team) ItIsMyEnemy = true;
                }

                if (ItIsMyEnemy)
                {
                    Instantiate(GreenRamExplosionV, transform.position, transform.rotation);
                    //GreenRamExplosionPS.Play();
                    flyerSC.SetDamage(RamDamageAmount, "ram", transform.position);
                }
            }
        }
    }



    #region AI
    protected override void NoTargetBehavior()
    {
        base.NoTargetBehavior();
        if (Ram.activeSelf) ToggleRam(false);
    }

    #endregion

    #region DamageReaction

    public override void SetDamage(int damage, string Type, Vector3 damagePoint)
    {
        //if central point of changeling is father than forward, then it is ram
        if (Vector3.Distance(damagePoint, transform.position) > Vector3.Distance(damagePoint, transform.position + transform.up * 1f) && RamActive)
        {
            ramEnergyCurrent = 0f;
            distantAttackEnergyCurrent = 0f;
            Instantiate(GreenRamExplosionV, transform.position, transform.rotation);
        }
        else
        {
            Health -= damage;
            HitVisual(Type);
            if (Health <= 0)
            {
                SetDeathSettings();
                DeathVisual();
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

    public override void BulletHitVisual()
    {

    }

    public override void BlastHitVisual()
    {
        SmokeTrailPS.Stop();
        SmokeTrailPS.Clear();
        var mainParticleSystem = SmokeTrailPS.main;
        mainParticleSystem.duration = 1f;
        SmokeTrailPS.Play();
    }

    public override void SetDeathSettings()
    {
        base.SetDeathSettings();

        if (disguiseActive) { MoveSpeed = 0f; }

        Ram.SetActive(false);
        GreenTrailPS.Pause();

        //Arrow.SetActive(false);

        disguise.SetActive(false);
        disguiseActive = false;
    }

    public override void DeathVisual()
    {
        GreenExplosionPS.Play();
        SmokeTrailPS.Play();
    }

#endregion

}
