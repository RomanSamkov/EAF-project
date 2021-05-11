using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using System;

public class PegasusPlayerG3 : PegasusG3
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
    private GameObject ammoCounterGO;
    private AmmoCounter ammoCounterSC;

    private PlayerG3 playerSC;

    protected int layerLevelMask;

    protected override void Awake()
    {
        base.Awake();
        DeathPanelUIGO = GameObject.Find("Death panel");
        ChangeSpeed = (MaxMoveSpeed - MinMoveSpeed) / 2f;
        layerLevelMask = LayerMask.GetMask("Level");
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

        ammoCounterGO = GameObject.Find("Ammo counter");
        ammoCounterSC = ammoCounterGO.GetComponent<AmmoCounter>();
        ammoCounterSC.SetInt(distantAttackAmmoCurrent);

        playerSC = gameObject.AddComponent<PlayerG3>();
        //playerSC = GetComponent<PlayerG3>();
        playerSC.StartInstantiateArrows(teamSeparator, arrowPF);

        
        //playerSC.DoSmth();

        //StartInstantiateArrows();
    }

    protected override void Update()
    {
        CurrentGoalSpeed = MoveSpeed + ChangeSpeed * CrossPlatformInputManager.GetAxis("Vertical");
        CurrentRotSpeed = RotateSpeed * Mathf.Abs(CrossPlatformInputManager.GetAxis("Horizontal"));
        Attack();
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

        CheckForLevelCollisionDanger();

    }

    private void PlayerControl()
    {
        Vector3 direction = (transform.position + transform.right * CrossPlatformInputManager.GetAxis("Horizontal")) - rb.position;
        direction.Normalize();
        RotationAmount = Vector3.Cross(transform.up, direction);

        rb.angularVelocity = RotationAmount * CurrentRotSpeed;

        FlyForward();
    }

    private void Attack()
    {
        distantAttackTimer += Time.deltaTime;

        canDistantAttack = distantAttackTimer >= DistantAttackTimerEnd && distantAttackAmmoCurrent > 0 && !needFlyToCenter;

        if ((Input.GetKey(KeyCode.K) || CrossPlatformInputManager.GetButton("Shot")) && canDistantAttack)
        {
            UseDistantAttack();
        }
    }

    protected override void UseDistantAttack()
    {
        base.UseDistantAttack();
        distantAttackAmmoCurrent = UnityEngine.Random.Range(12, 999);
        ammoCounterSC.SetInt(distantAttackAmmoCurrent);
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

    [Header("Raycast Vectors")]
    [SerializeField] protected Transform forwardPoint;
    [SerializeField] protected Transform rightPoint;
    [SerializeField] protected Transform leftPoint;
    [SerializeField] protected Transform rightMidPoint;
    [SerializeField] protected Transform leftMidPoint;

    private void CheckForLevelCollisionDanger()
    {
        //Check forward direction
        if (Physics.Linecast(transform.position, forwardPoint.position, layerLevelMask))
        {
            //Debug.Log("forward blocked");
            MaxMoveSpeed = 0;
            if (MinMoveSpeed > 0) MinMoveSpeed = 0;

            playerSC.RecalculateMoveSpeedChangeSpeed();
            int damageAmount = 0;

            if (CurrentSpeed > 12)
                damageAmount = Convert.ToInt32(CurrentSpeed / 2.5f);

            if (damageAmount > 0)
            {
                Debug.Log($"Eat {damageAmount}");
                SetDamage(damageAmount, "ground_hit", gameObject.transform.position);
            }
                
            while(Physics.Linecast(transform.position, forwardPoint.position, layerLevelMask))
            {
                transform.Translate(Vector3.up * -1);
            }
            
            if(CurrentSpeed>0)
            CurrentSpeed *= -0.6f;
        }
        else if (Physics.Linecast(transform.position, rightPoint.position, layerLevelMask))
        {
            Debug.Log("right hit");

            while (Physics.Linecast(transform.position, rightPoint.position, layerLevelMask))
            {
                transform.Rotate(Vector3.forward * .1f);
            }
        }
        else if (Physics.Linecast(transform.position,leftPoint.position, layerLevelMask))
        {
            Debug.Log("left hit");

            while (Physics.Linecast(transform.position, leftPoint.position, layerLevelMask))
            {
                transform.Rotate(Vector3.forward * -.1f);
            }
        }
        else
        {
            ResetMovement();
            playerSC.RecalculateMoveSpeedChangeSpeed();
        }
    }
}
