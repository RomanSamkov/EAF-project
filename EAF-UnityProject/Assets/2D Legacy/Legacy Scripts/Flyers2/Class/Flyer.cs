using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Flyer : MonoBehaviour
{
    //Movement
    public float MoveSpeed;
    public float MinMoveSpeed;
    public float MaxMoveSpeed;
    public float FinalSpeed;
    public float ChangeSpeed;

    public float RotateSpeed;
    public float MinRotateSpeed;
    public float MaxRotateSpeed;
    public float FinalRotateSpeed;

    protected float prMoveSpeed;
    protected float prMinMoveSpeed;
    protected float prMaxMoveSpeed;
    protected float prFinalSpeed;

    protected float prRotateSpeed;
    protected float prMinRotateSpeed;
    protected float prMaxRotateSpeed;

    public float RandomizeTimer;
    protected float fullRandomizeTimer;
    public bool CanRandomizeMobility;

    protected float changeAroundDirectionTimer = 4f;
    protected float fullChangeAroundDirectionTimer = 4f;
    protected int rotateDirection;

    protected float fallSpeed = 0f;

    protected float xMinBorder;
    protected float xMaxBorder;
    protected float zMinBorder;
    protected float zMaxBorder;

    public Vector3 CentralMapPoint;
    protected bool needFlyToCenter;

    protected bool needResetSpeed = false;

    //Other
    public bool AvailableAsTarget = true;

    protected Vector3 Target;
    protected Vector3 actualTargetPos;

    protected GameObject player;

    public int Health;
    public bool Alive = true;

    public string Race;

    public float DetectDistance;
    public GameObject Arrow;

    //Fight
    public string TargetTeam;
    public int TargetNumber;

    /*
    public float ShotTimer;
    public float FullShotTimer;

    public int ShotDamage;
    */

    protected float actualDistance;
    protected float targetDistance;
    public float aimDistance;

    //Components
    protected Rigidbody rb;
    protected BoxCollider bcollider;

    //MainFunctions
    protected virtual void Awake()
    {
        SetProtectedSpeed();
        rb = GetComponent<Rigidbody>();
        bcollider = GetComponent<BoxCollider>();
        fullRandomizeTimer = RandomizeTimer;
    }

    protected virtual void Start()
    {
        player = GameObject.Find("Player");
        FindBorders();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        RandomizeDirection();
        BorderControl();
        AutoFly();
        FinalSpeed = MoveSpeed;
    }

    //Movement
    protected virtual void SetProtectedSpeed()
    {
        prMoveSpeed    = MoveSpeed;
        prMinMoveSpeed = MinMoveSpeed;
        prMaxMoveSpeed = MaxMoveSpeed;
        prFinalSpeed   = FinalSpeed;

        prRotateSpeed    = RotateSpeed;
        prMinRotateSpeed = MinRotateSpeed;
        prMaxRotateSpeed = MaxRotateSpeed;
    }

    protected void FlyForward()
    {
        rb.velocity = transform.up * MoveSpeed;
    }

    protected void RotateToVector(Vector3 target)
    {
        Vector3 direction = target - rb.position;
        direction.Normalize();
        Vector3 rotationAmount = Vector3.Cross(transform.up, direction);
        rb.angularVelocity = rotationAmount * RotateSpeed;
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

    protected virtual void RandomizeMobility()
    {
        RandomizeTimer += Time.deltaTime;
        if (RandomizeTimer > fullRandomizeTimer && CanRandomizeMobility)
        {
            MoveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);
            RotateSpeed = Random.Range(MinRotateSpeed, MaxRotateSpeed);
            RandomizeTimer = 0f;
        }
    }

    protected virtual void ArrowHost()
    {
        float playerDistance = Vector3.Distance(player.transform.position, transform.position);
        if (playerDistance < DetectDistance && Alive)
        {
            Arrow.SetActive(true);
        }
        else
        {
            Arrow.SetActive(false);
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

    public virtual void SetCurMinMaxMoveSpeed(float curSpeed, float minSpeed, float maxSpeed)
    {
        MoveSpeed = curSpeed;
        MinMoveSpeed = minSpeed;
        MaxMoveSpeed = maxSpeed;
    }

    public virtual void SetCurMinMaxRotateSpeed(float curSpeed, float minSpeed, float maxSpeed)
    {
        RotateSpeed = curSpeed;
        MinRotateSpeed = minSpeed;
        MaxRotateSpeed = maxSpeed;
    }

    protected virtual void AutoFly()
    {
        if (Alive && TargetNumber > 0 && !needFlyToCenter)
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
        else if (Alive && TargetNumber == 0)
        {
            FlyAround();
            SetCurMinMaxMoveSpeed(7f, 4f, 8f);
            SetCurMinMaxRotateSpeed(0.6f, 0.4f, 1f);
            needResetSpeed = true;
        }
        if (!Alive) DeathFall();
    }

    //Fight
    protected virtual void UpdateTarget()
    {
        GameObject[] flyers = GameObject.FindGameObjectsWithTag("Flyers");
        float shortestDist = Mathf.Infinity;

        GameObject nearestEnemy = null;
        TargetNumber = 0;
        foreach (GameObject flyer in flyers)
        {
            TeamSeparator team = flyer.GetComponent<TeamSeparator>();
            Flyer flyerSC = flyer.GetComponent<Flyer>();
            if (team.Team == TargetTeam && flyerSC.AvailableAsTarget)
            {
                TargetNumber++;
                float distance = Vector3.Distance(transform.position, flyer.transform.position);
                if (distance < shortestDist)
                {
                    shortestDist = distance;
                    nearestEnemy = flyer;
                }
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
            if (shortestDist > 20f)
            {
                Target = nearestEnemy.transform.position + nearestEnemy.transform.up * 20f;
            }
            else
            {
                Target = nearestEnemy.transform.position;
            }
        }

    }

    protected void FindActualDistance()
    {
        actualDistance = Vector3.Distance(actualTargetPos, transform.position);
    }

    protected void FindTargetDistance()
    {
        targetDistance = Vector3.Distance(Target, transform.position);
    }

    protected virtual void FindAimDistance(float forwardDistance)
    {
        aimDistance = Vector3.Distance(Target, transform.position + transform.up * forwardDistance);
    }

    //Damage
    public virtual void SetDamage(int damage, string Type)
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
        Arrow.SetActive(false);
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
}
