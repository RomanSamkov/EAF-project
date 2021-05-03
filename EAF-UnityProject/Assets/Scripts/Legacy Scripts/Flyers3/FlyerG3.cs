using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(TeamSeparatorG3))]
public class FlyerG3 : MonoBehaviour
{
    //Inspector Variables
    #region InspectorVariables
    //[SerializeField] protected Animator anim;

    [Header("Movement")]
    public float AccelerationSpeed;
    public float MinMoveSpeed;
    public float MaxMoveSpeed;
    public float MinRotateSpeed;
    public float MaxRotateSpeed;

    [Header("Border")]
    public Vector3 CentralMapPoint = new Vector3(0, 30, 0);

    [Header("Health parameters")]
    public int Health;
    public bool Alive = true;
    public string Race;

    //[Header("Detection")]
    //public float DetectDistance;
    //public GameObject Arrow;
    #endregion

    #region HidenVariales
    [HideInInspector] public float CurrentSpeed;
    [HideInInspector] public float CurrentGoalSpeed;
    [HideInInspector] public float CurrentRotSpeed;
    [HideInInspector] public float ChangeSpeed;
    [HideInInspector] public float MoveSpeed;
    [HideInInspector] public float RotateSpeed;
    [HideInInspector] public Vector3 RotationAmount;

    protected bool needFlyToCenter;

    [HideInInspector] public float distanceToTarget;

    [HideInInspector] public bool AvailableAsTarget = true;

    protected Vector3 movementVector;

    protected Rigidbody rb;
    protected BoxCollider bcollider;
    protected TeamSeparatorG3 teamSeparator;
    #endregion

    protected virtual void FindAllComponents()
    {
        rb = GetComponent<Rigidbody>();
        bcollider = GetComponent<BoxCollider>();
        teamSeparator = GetComponent<TeamSeparatorG3>();
    }

    //MainFunctions
    protected virtual void Awake()
    {
        MoveSpeed = (MaxMoveSpeed + MinMoveSpeed) / 2;
        FindAllComponents();
        SetProtectedSpeed();
        CanRandomizeMobilityCurrent = CanRandomizeMobilityGlobal;
    }

    protected virtual void Start()
    {
        player = GameObject.Find("Player");
        FindBorders();
    }

    protected virtual void Update()
    {
        //ArrowHost();
        RandomizeDirection();
        BorderControl();
        CurrentSpeed = MoveSpeed;
        CurrentRotSpeed = RotateSpeed;
        if (TargetGO!=null)movementVector = TargetGO.transform.position;
    }

    #region Movement
    [HideInInspector] public float prMoveSpeed;
    [HideInInspector] public float prMinMoveSpeed;
    [HideInInspector] public float prMaxMoveSpeed;
    [HideInInspector] public float prRotateSpeed;
    [HideInInspector] public float prMinRotateSpeed;
    [HideInInspector] public float prMaxRotateSpeed;

    protected float changeAroundDirectionTimer = 4f;
    protected float fullChangeAroundDirectionTimer = 4f;
    protected int rotateDirection;

    protected bool needResetMovement = false;

    protected float fallSpeed = 0f;

    protected virtual void SetProtectedSpeed()
    {
        prMoveSpeed = MoveSpeed;
        prMinMoveSpeed = MinMoveSpeed;
        prMaxMoveSpeed = MaxMoveSpeed;

        prRotateSpeed = RotateSpeed;
        prMinRotateSpeed = MinRotateSpeed;
        prMaxRotateSpeed = MaxRotateSpeed;
    }

    public virtual void SetCurMinMaxMoveSpeed(float movSpeed, float minSpeed, float maxSpeed)
    {
        MoveSpeed = movSpeed;
        MinMoveSpeed = minSpeed;
        MaxMoveSpeed = maxSpeed;
    }

    public virtual void SetCurMinMaxRotateSpeed(float curSpeed, float minSpeed, float maxSpeed)
    {
        RotateSpeed = curSpeed;
        MinRotateSpeed = minSpeed;
        MaxRotateSpeed = maxSpeed;
    }

    protected virtual void ResetMovement()
    {
        SetCurMinMaxMoveSpeed(prMoveSpeed, prMinMoveSpeed, prMaxMoveSpeed);
        SetCurMinMaxRotateSpeed(prRotateSpeed, prMinRotateSpeed, prMaxRotateSpeed);
    }

    protected void FlyForward()
    {
        if(CurrentSpeed < CurrentGoalSpeed)
        {
            CurrentSpeed += AccelerationSpeed * Time.deltaTime;
        }
        else if(CurrentSpeed > CurrentGoalSpeed)
        {
            CurrentSpeed -= AccelerationSpeed * Time.deltaTime;
        }
        rb.velocity = transform.up * CurrentSpeed; 
    }

    protected void RotateToVector(Vector3 target)
    {
        Vector3 direction = target - rb.position;
        direction.Normalize();
        RotationAmount = Vector3.Cross(transform.up, direction);
        rb.angularVelocity = RotationAmount * RotateSpeed;
    }

    protected void FlyToVector(Vector3 target)
    {
        RotateToVector(target);
        FlyForward();
    }

    protected virtual void FlyRound(int rotationDirection)
    {
        if (rotationDirection == 1) { rotationDirection = -1; }
        if (rotationDirection == 2) { rotationDirection = 1; }
        FlyToVector(transform.position + transform.right * rotationDirection);
    }

    protected virtual void FlyAround()
    {
        FlyRound(rotateDirection);
    }

    protected virtual void RandomizeDirection()
    {
        changeAroundDirectionTimer += Time.deltaTime;
        if (changeAroundDirectionTimer > fullChangeAroundDirectionTimer)
        {
            rotateDirection = Random.Range(1, 3);
            changeAroundDirectionTimer = 0f;
        }
    }

    [Header("Randomize movement")]
    protected float randomizeTimer = 0f;
    [SerializeField] protected float fullRandomizeTimer;
    public bool CanRandomizeMobilityGlobal;
    protected bool CanRandomizeMobilityCurrent;

    protected virtual void RandomizeMobility()
    {
        randomizeTimer += Time.deltaTime;
        if (randomizeTimer > fullRandomizeTimer && CanRandomizeMobilityCurrent)
        {
            MoveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);
            RotateSpeed = Random.Range(MinRotateSpeed, MaxRotateSpeed);
            randomizeTimer = 0f;
        }
    }

    protected virtual void DeathFall()
    {
        RotateToVector(transform.position + transform.up * 10f);
        fallSpeed += 10f * Time.deltaTime;
        if (MoveSpeed > 0f) { MoveSpeed -= 1f * Time.deltaTime; }
        if (MoveSpeed < 0f) { MoveSpeed += 1f * Time.deltaTime; }
        rb.velocity = transform.up * MoveSpeed + transform.forward * fallSpeed;
    }

    #endregion

    #region BorderControl

    protected float xMinBorder;
    protected float xMaxBorder;
    protected float zMinBorder;
    protected float zMaxBorder;
    public virtual void BorderControl()
    {
        if (transform.position.x <= xMinBorder)
        {
            needFlyToCenter = true;
        }
        else if (transform.position.x >= xMaxBorder)
        {
            needFlyToCenter = true;
        }
        else if (transform.position.z <= zMinBorder)
        {
            needFlyToCenter = true;
        }
        else if (transform.position.z >= zMaxBorder)
        {
            needFlyToCenter = true;
        }
        else
        {
            needFlyToCenter = false;
        }
    }
    public virtual void FindBorders()
    {
        GameObject pxBorder = GameObject.Find("Border x+");
        xMaxBorder = pxBorder.transform.position.x;
        GameObject mxBorder = GameObject.Find("Border x-");
        xMinBorder = mxBorder.transform.position.x;
        GameObject pzBorder = GameObject.Find("Border z+");
        zMaxBorder = pzBorder.transform.position.z;
        GameObject mzBorder = GameObject.Find("Border z-");
        zMinBorder = mzBorder.transform.position.z;
    }

    #endregion

    #region DamageReaction

    //Damage
    public virtual void SetDamage(int damage, string Type, Vector3 damagePoint)
    {
        Health -= damage;
        HitVisual(Type);
        if (Health <= 0)
        {
            SetDeathSettings();
            DeathVisual();
        }
    }

    public virtual void SetDeathSettings()
    {
        Alive = false;
        AvailableAsTarget = false;
        bcollider.enabled = false;
        //Arrow.SetActive(false);
        rb.constraints = RigidbodyConstraints.None;
        Invoke("DeactivateGameObject", 3f);
    }

    //Visual
    public virtual void HitVisual(string damageType)
    {
        switch (damageType)
        {
            case ("bullet"):
                {
                    BulletHitVisual();
                    break;
                }
            case ("blast"):
                {
                    BlastHitVisual();
                    break;
                }
            case ("ram"):
                {
                    RamHitVisual();
                    break;
                }
        }
    }

    public virtual void RamHitVisual() { }

    public virtual void BulletHitVisual() { }

    public virtual void BlastHitVisual() { }

    public virtual void DeathVisual()
    {

    }

    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region AI

    [HideInInspector]
    public GameObject TargetGO;

    [HideInInspector]
    public int NearEnemiesNum;

    protected GameObject player;

    protected bool fighting = false;

    protected float actualDistance;
    /*
    protected virtual void InterceptTargetBehavior()
    {
        FlyToVector(movementVector);
        if (needResetMovement)
        {
            ResetMovement();
            needResetMovement = false;
        }
        //
    }
    */
    protected virtual void FlyBackToBorders()
    {
        FlyToVector(CentralMapPoint);
    }

    protected virtual void NoTargetBehavior()
    {
        FlyAround();
        SetCurMinMaxMoveSpeed(7f, 4f, 8f);
        SetCurMinMaxRotateSpeed(0.6f, 0.4f, 1f);
        needResetMovement = true;
    }

    //Fight
    protected virtual void UpdateTarget()
    {
        GameObject[] flyers = GameObject.FindGameObjectsWithTag("Flyers");

        NearEnemiesNum = 0;
        List<GameObject> NearEnemiesList = new List<GameObject>();

        //Find Nearest Enemy
        GameObject nearestEnemy = null;
        float shortestDist = Mathf.Infinity;

        foreach (GameObject flyer in flyers)
        {
            //Debug.Log(flyer.name);
            TeamSeparatorG3 selectedFlyerTeam = flyer.GetComponent<TeamSeparatorG3>();
            FlyerG3 flyerSC = flyer.GetComponent<FlyerG3>();

            //Check if Available
            if (!flyerSC.AvailableAsTarget)
            {
                //Debug.Log("This flyer is not available as target");
                continue;
            }

            //Check if Enemy
            bool ItIsMyEnemy = false;
            foreach (string team in teamSeparator.EnemyTeamList)
            {
                if (selectedFlyerTeam.Team == team) ItIsMyEnemy = true;
            }
            if(!ItIsMyEnemy) continue;

            NearEnemiesNum++;
            NearEnemiesList.Add(flyer);

            //Compare to other Fine Targets
            float distance = Vector3.Distance(transform.position, flyer.transform.position);

            if (distance < shortestDist)
            {
                shortestDist = distance;
                nearestEnemy = flyer;
            }
        }
        //if(TargetGO!=null)
        //Debug.Log($"Update target  = {TargetGO.name}");

        fighting = NearEnemiesNum > 0;

        //Find Optimal Target
        GameObject optimalTarget = null;
        float shortestForwardDist = Mathf.Infinity;

        foreach (GameObject nearEnemy in NearEnemiesList)
        {
            Vector3 forwardPosition = transform.position+transform.up*shortestDist;

            float distance = Vector3.Distance(forwardPosition, nearEnemy.transform.position);

            if (distance < shortestForwardDist)
            {
                shortestForwardDist = distance;
                optimalTarget = nearEnemy;
            }
        }

        if (NearEnemiesNum > 0)
        {
            TargetGO = optimalTarget;
        }
    }

    protected void FindActualDistance()
    {
        actualDistance = Vector3.Distance(TargetGO.transform.position, transform.position);
    }

    #endregion
}
