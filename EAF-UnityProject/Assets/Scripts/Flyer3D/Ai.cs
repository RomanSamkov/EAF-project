using UnityEngine;

public class Ai : MonoBehaviour
{
    //[SerializeField] protected Animator anim;
    public Transform Vertical;
    public Transform Body;

    [Header("Movement")]
    //Forward
    public float AccelerationSpeed;
    public float MoveSpeed;
    [HideInInspector] public float CurrentSpeed;
    public float MinMoveSpeed;
    public float MaxMoveSpeed;

    //Rotation
    public float AccelerationRotateSpeed;
    [HideInInspector] public float CurrentHorRotSpeed;
    [HideInInspector] public float GoalHorRotSpeed;
    [HideInInspector] public float CurrentVerRotSpeed;
    [HideInInspector] public float GoalVerRotSpeed;
    public float MaxRotSpeed;

    [HideInInspector] public float VerticalAngle = 0f;

    protected float gravitySpeedInfluence = 0f;
    protected float gravitySpeedInfluenceChangePerSec = 9f;
    protected float maxGravityInfluence = 20f;

    [Header("Distant Attack")]
    [Space]
    public Transform DistantAttackPoint;
    public GameObject BulletPR;
    public GameObject MuzzlePR;
    public AudioClip DistantAttackAudioClip;
    public AudioSource DistantAttackAudioSource;
    public ParticleSystem SmokeMuzzleTrailPS;

    public float RandomSpreadAngle;
    public float ProjectileSpeed;
    public float ProjectileLifeTime;
    public int ProjectileDamage;

    public int DistantAttackAmmoMax;
    protected int distantAttackAmmoCurrent = 10000;

    protected bool canDistantAttack;
    protected float distantAttackTimer;
    public float DistantAttackTimerEnd;

    public int BurstFireCount;
    private int burstFireCurrentNum;
    public float BurstFireSpaceTime;
    private float burstFireSpaceTimer;
    bool burstReady;

    [Space]
    [Header("Other Visuals")]
    public ParticleSystem SmokeTrailPS;
    public GameObject ForwardGroundHitPR;
    public GameObject DownGroundHitPR;

    public Transform FlyDestinationPoint;

    public GameObject ShotSoundPR;
    protected int layerLevelMask;
    // Start is called before the first frame update
    void Start()
    {
        layerLevelMask = LayerMask.GetMask("Level");
        burstReady = true;
        burstFireSpaceTimer = BurstFireSpaceTime;
        burstFireCurrentNum = BurstFireCount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        changeFlySpeed();
        rotateHorizontally();
        moveVertical();
        flyForward();
        changeBodyRotation();
        rayCastCollision();
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        distantAttackTimer += Time.deltaTime;

        burstReady = burstFireCurrentNum > 0;

        if (burstReady) burstFireSpaceTimer = BurstFireSpaceTime;
        else burstFireSpaceTimer -= Time.deltaTime;

        if (burstFireSpaceTimer <= 0)
        {
            burstFireCurrentNum = BurstFireCount;
            burstReady = true;
        }

        canDistantAttack = distantAttackTimer >= DistantAttackTimerEnd && distantAttackAmmoCurrent > 0;

        if (canDistantAttack && burstReady)
        {
            UseDistantAttack();
            burstFireCurrentNum--;
            burstReady = burstFireCurrentNum > 0;
            if (!burstReady)
            {
                BurstFireCount = Random.Range(30, 60);
                BurstFireSpaceTime = Random.Range(DistantAttackTimerEnd, 4f);
            }
        }
    }

    //public Transform Sphere;

    protected virtual void UseDistantAttack()
    {
        SmokeMuzzleTrailPS.Play();

        Vector3 randRot = Vertical.eulerAngles;
        randRot += new Vector3(Random.Range(-RandomSpreadAngle, RandomSpreadAngle), Random.Range(-RandomSpreadAngle, RandomSpreadAngle), 0f);

        Instantiate(BulletPR, DistantAttackPoint.position, Quaternion.Euler(randRot));

        distantAttackAmmoCurrent -= 1;

        Instantiate(MuzzlePR, DistantAttackPoint.position, DistantAttackPoint.rotation);

        //DistantAttackAudioSource.pitch = Random.Range(0.95f, 1.05f);
        //DistantAttackAudioSource.PlayOneShot(DistantAttackAudioClip);
        //
        //soundDelay.AddNewSound(DistantAttackAudioSource, DistantAttackAudioClip, transform.position);
        Instantiate(ShotSoundPR, DistantAttackPoint.position, Quaternion.identity);
        distantAttackTimer = 0f;
    }

    float sinus;
    float cosine;
    void flyForward()
    {
        sinus = Mathf.Sin(VerticalAngle * 0.0174533f);
        cosine = Mathf.Cos(VerticalAngle * 0.0174533f);

        CurrentSpeed = MoveSpeed + gravitySpeedInfluence;

        //Move Horizontal
        transform.Translate(CurrentSpeed * cosine * Vector3.forward * Time.deltaTime);

        //Move Vertical
        transform.Translate(-1 * CurrentSpeed * sinus * Vector3.up * Time.deltaTime);
    }

    bool forwardAcceleration;
    bool stopAcceleration;
    void changeFlySpeed()
    {
        //forwardAcceleration = Input.GetKey(KeyCode.W) || CrossPlatformInputManager.GetButton("Move Forward");
        //stopAcceleration = Input.GetKey(KeyCode.S) || CrossPlatformInputManager.GetButton("Stop");

        if (forwardAcceleration) MoveSpeed += AccelerationSpeed * Time.deltaTime;
        else if (stopAcceleration) MoveSpeed -= AccelerationSpeed * Time.deltaTime;

        if (MoveSpeed > MaxMoveSpeed)
        {
            MoveSpeed = MaxMoveSpeed;
        }
        else if (MoveSpeed < MinMoveSpeed)
        {
            MoveSpeed = MinMoveSpeed;
        }
    }


    Vector3 direction;
    float horizontalDistance;
    void rotateHorizontally()
    {
        float distance = Vector3.Distance(FlyDestinationPoint.position, transform.position);

        direction = FlyDestinationPoint.position +
            FlyDestinationPoint.transform.forward * 21 *(distance/100)
            - transform.position;

      //  Sphere.position = FlyDestinationPoint.position +
       //     FlyDestinationPoint.transform.forward * 21 * (distance / 100)+FlyDestinationPoint.transform.up * 3 * (distance / 125);

        direction.Normalize();

        Vector3 horRotationAmount = Vector3.Cross(transform.forward, direction);

        GoalHorRotSpeed = MaxRotSpeed * horRotationAmount.y;

        if (CurrentHorRotSpeed > GoalHorRotSpeed)
        {
            CurrentHorRotSpeed -= AccelerationRotateSpeed * Time.deltaTime;
            if (CurrentHorRotSpeed - 2f < GoalHorRotSpeed && CurrentHorRotSpeed + 2f > GoalHorRotSpeed) CurrentHorRotSpeed = GoalHorRotSpeed;
        }
        else if (CurrentHorRotSpeed < GoalHorRotSpeed)
        {
            CurrentHorRotSpeed += AccelerationRotateSpeed * Time.deltaTime;
            if (CurrentHorRotSpeed - 2f < GoalHorRotSpeed && CurrentHorRotSpeed + 2f > GoalHorRotSpeed) CurrentHorRotSpeed = GoalHorRotSpeed;
        }

        transform.Rotate(CurrentHorRotSpeed * Time.deltaTime * Vector3.up);
    }


    void moveVertical()
    {
        float goalVerticalAngle = direction.y * -60;


        if (VerticalAngle > goalVerticalAngle)
        {
            CurrentVerRotSpeed = -MaxRotSpeed * Mathf.Abs(direction.y);
        }
        else if (VerticalAngle < goalVerticalAngle)
        {
            CurrentVerRotSpeed = MaxRotSpeed * Mathf.Abs(direction.y);
        }
        else
        {
            CurrentVerRotSpeed = 0;
        }

        //Debug.Log($"The goal is {goalVerticalAngle}, current vert angle is {VerticalAngle}, so we have CurrentVerRotSpeed = {CurrentVerRotSpeed} ");

        VerticalAngle += CurrentVerRotSpeed * Time.deltaTime;
    }

    void changeBodyRotation()
    {
        float percentOfMaxSpeed = (MoveSpeed - MinMoveSpeed) / (MaxMoveSpeed - MinMoveSpeed);
        float percentOfRotSpeed = CurrentHorRotSpeed / MaxRotSpeed;

        Vertical.localEulerAngles = new Vector3(
            VerticalAngle,
            0f,
            0f
            );

        Body.transform.localEulerAngles = new Vector3(
            45f * percentOfRotSpeed * percentOfMaxSpeed,
            90,
            0
            );

        //Debug.Log($"percentOfRotSpeed ({percentOfRotSpeed}) * percentOfMaxSpeed({percentOfMaxSpeed}) = {percentOfRotSpeed * percentOfMaxSpeed}");
    }

    [Header("RayCastCollisionPoints")]

    public Transform ForwardRayCastPoint;
    public Transform ForwardRayCastPoint2;
    public Transform LeftHorRayCastPoint;
    public Transform RightHorRayCastPoint;
    public Transform ForwardDownRayCastPoint;


    void rayCastCollision()
    {
        //Forward ray
        if (Physics.Linecast(Vertical.transform.position, ForwardRayCastPoint.position, layerLevelMask)
            || Physics.Linecast(Vertical.transform.position, ForwardRayCastPoint2.position, layerLevelMask))
        {
            if (CurrentSpeed > 10)
                Instantiate(ForwardGroundHitPR, ForwardRayCastPoint.position, Vertical.rotation);

            if (CurrentSpeed > 6)
                while (Physics.Linecast(transform.position, ForwardRayCastPoint.position, layerLevelMask))
                {
                    //Move Horizontal
                    transform.Translate(-1 * cosine * Vector3.forward);

                    //Move Vertical
                    transform.Translate(1 * sinus * Vector3.up);
                }

            MoveSpeed = 0;
            gravitySpeedInfluence = 0;

        }

        if (Physics.Linecast(Vertical.transform.position, ForwardDownRayCastPoint.position, layerLevelMask))
        {
            if (CurrentSpeed > 6)
                Instantiate(DownGroundHitPR, ForwardRayCastPoint.position, ForwardDownRayCastPoint.rotation);

            for (int i = 0; i < 360; i++)
            {
                VerticalAngle -= 1;

                Vertical.localEulerAngles = new Vector3(
                    VerticalAngle,
                    0f,
                    0f
                );

                if (MoveSpeed > 0.5f)
                    MoveSpeed -= 0.5f;
                else MoveSpeed = 0f;

                if (!Physics.Linecast(transform.position, ForwardDownRayCastPoint.position, layerLevelMask))
                {
                    break;
                }
            }

        }

        if (Physics.Linecast(Vertical.transform.position, LeftHorRayCastPoint.position, layerLevelMask))
        {
            if (CurrentSpeed > 8)
                Instantiate(DownGroundHitPR, LeftHorRayCastPoint.position, ForwardDownRayCastPoint.rotation);

            for (int i = 0; i < 360; i++)
            {
                transform.Rotate(1 * Vector3.up);

                if (MoveSpeed > 0.5f)
                    MoveSpeed -= 0.5f;
                else MoveSpeed = 0f;

                if (!Physics.Linecast(transform.position, LeftHorRayCastPoint.position, layerLevelMask))
                    break;
            }

        }

        if (Physics.Linecast(Vertical.transform.position, RightHorRayCastPoint.position, layerLevelMask))
        {
            if (CurrentSpeed > 8)
                Instantiate(DownGroundHitPR, RightHorRayCastPoint.position, ForwardDownRayCastPoint.rotation);

            for (int i = 0; i < 360; i++)
            {
                transform.Rotate(-1 * Vector3.up);

                if (MoveSpeed > 0.5f)
                    MoveSpeed -= 0.5f;
                else MoveSpeed = 0f;

                if (!Physics.Linecast(transform.position, RightHorRayCastPoint.position, layerLevelMask))
                    break;
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(DistantAttackPoint.transform.position, DistantAttackPoint.transform.position + DistantAttackPoint.transform.forward * 200f);
    }
}
