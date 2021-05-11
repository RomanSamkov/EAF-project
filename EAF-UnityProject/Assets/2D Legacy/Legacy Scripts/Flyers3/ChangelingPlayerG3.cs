using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ChangelingPlayerG3 : ChangelingG3
{
    [Space]
    [Header("Player Settings")]
    [SerializeField] private GameObject arrowPF;
    //Health
    public int EasyDifHealth;
    public int MiddleDifHealth;
    public int HardDifHealth;

    //UI Objects
    private GameObject DeathPanelUIGO;
    private GameObject HealthBarUIGO;
    private HealthBarG3 HealthBarSC;

    private GameObject RamEnergyBarGO;
    private EnergyBar RamEnergyBarSC;

    private GameObject ShotEnergyBarGO;
    private EnergyBar ShotEnergyBarSC;

    private PlayerG3 playerSC;

    protected override void Awake()
    {
        base.Awake();
        DeathPanelUIGO = GameObject.Find("Death panel");
        ChangeSpeed = (MaxMoveSpeed - MinMoveSpeed) / 2f;
    }

    protected override void Start()
    {
        base.Start();
        

        if (DifficultyLevels.DifficultyLevel < 3)
        {
            HealthBarUIGO = GameObject.Find("Health bar");
            HealthBarSC = HealthBarUIGO.GetComponent<HealthBarG3>();
        }

        if (DifficultyLevels.DifficultyLevel == 1) { Health = EasyDifHealth; HealthBarSC.SetMaxHealth(Health); }
        if (DifficultyLevels.DifficultyLevel == 2) { Health = MiddleDifHealth; HealthBarSC.SetMaxHealth(Health); }
        if (DifficultyLevels.DifficultyLevel == 3) { Health = HardDifHealth; }

        RamEnergyBarGO = GameObject.Find("Ram Energy bar");
        RamEnergyBarSC = RamEnergyBarGO.GetComponent<EnergyBar>();
        RamEnergyBarSC.SetMaxEnergy(RamEnergyMax);

        ShotEnergyBarGO = GameObject.Find("Shot Energy bar");
        ShotEnergyBarSC = ShotEnergyBarGO.GetComponent<EnergyBar>();
        ShotEnergyBarSC.SetMaxEnergy(DistantAttackMaxEnergy);

        playerSC = gameObject.AddComponent<PlayerG3>();
        playerSC.StartInstantiateArrows(teamSeparator, arrowPF);
    }

    protected override void Update()
    {
        ramManager();
        distantAttackManager();

        CurrentSpeed = MoveSpeed + ChangeSpeed * CrossPlatformInputManager.GetAxis("Vertical");
        CurrentRotSpeed = RotateSpeed * Mathf.Abs(CrossPlatformInputManager.GetAxis("Horizontal"));
        AttackManager();
        //CurrentFlyAnimationSpeed();
        BorderControl();

        if (needFlyToCenter)
        {
            FlyToVector(CentralMapPoint);
        }

        if (!needFlyToCenter)
        {
            PlayerControl();
        }
    }

    private void PlayerControl()
    {
        Vector3 direction = (transform.position + transform.right * CrossPlatformInputManager.GetAxis("Horizontal")) - rb.position;
        direction.Normalize();
        RotationAmount = Vector3.Cross(transform.up, direction);

        rb.angularVelocity = RotationAmount * CurrentRotSpeed;

        rb.velocity = transform.up * CurrentSpeed;
    }

    private void AttackManager()
    {
        if ((Input.GetKey(KeyCode.K) || CrossPlatformInputManager.GetButton("Shot")) && canDistantAttack)
        {
            UseDistantAttack();
        }
        else if((Input.GetKey(KeyCode.I) || CrossPlatformInputManager.GetButton("Ram")) && canRam)
        {
            ToggleRam(true);
        }

        if(!(Input.GetKey(KeyCode.I) || CrossPlatformInputManager.GetButton("Ram")) || !canRam){
            ToggleRam(false);
        }
    }

    protected override void ramManager()
    {
        if (RamActive && (ramEnergyCurrent - Time.deltaTime > 0f))
        {
            ramEnergyCurrent -= Time.deltaTime;
            RamEnergyBarSC.SetEnergy(ramEnergyCurrent);
            ramRegenTimer = 0f;
        }
        else if (RamActive && ramEnergyCurrent - Time.deltaTime <= 0)
        {
            RamEnergyBarSC.SetEnergy(ramEnergyCurrent);
            ramEnergyCurrent = 0f;
            ToggleRam(false);
        }
        else if (!RamActive && ramRegenTimer < 2f)
            ramRegenTimer += Time.deltaTime;

        if (ramRegenTimer >= 2f && ramEnergyCurrent < RamEnergyMax)
        {
            ramEnergyCurrent += 0.5f * Time.deltaTime;
            RamEnergyBarSC.SetEnergy(ramEnergyCurrent);
        }

        canRam = ramEnergyCurrent > 0 && !needFlyToCenter;
    }

    protected override void distantAttackManager()
    {
        if (distantAttackRegenTimer < distantAttackRegenTimerEnd)
        {
            distantAttackRegenTimer += Time.deltaTime;
        }
        else if (distantAttackEnergyCurrent < DistantAttackMaxEnergy && distantAttackRegenTimer >= distantAttackRegenTimerEnd)
        {
            distantAttackEnergyCurrent += Time.deltaTime;
            ShotEnergyBarSC.SetEnergy(distantAttackEnergyCurrent);
            if (distantAttackEnergyCurrent > DistantAttackMaxEnergy) distantAttackEnergyCurrent = DistantAttackMaxEnergy;
        }
        if (distantAttackTimer < DistantAttackTimerEnd) distantAttackTimer += Time.deltaTime;
        canDistantAttack = distantAttackTimer >= DistantAttackTimerEnd && distantAttackEnergyCurrent >= 1f && !needFlyToCenter && !RamActive;
    }

    protected override void UseDistantAttack()
    {
        base.UseDistantAttack();
        ShotEnergyBarSC.SetEnergy(distantAttackEnergyCurrent);
    }

    public override void SetDamage(int damage, string Type, Vector3 damagePoint)
    {
        base.SetDamage(damage, Type, damagePoint);
        if (DifficultyLevels.DifficultyLevel < 3) { HealthBarSC.SetHealth(Health); }
    }

    public override void SetDeathSettings()
    {
        Alive = false;
        AvailableAsTarget = false;
        bcollider.enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        Invoke("DeactivateGameObject", 3f);
        DeathPanelUIGO.SetActive(true);
        Time.timeScale = 0f;
    }
}
