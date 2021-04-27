using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class TestPlayer3D : MonoBehaviour
{
    #region InspectorVariables
    //[SerializeField] protected Animator anim;

    [Header("Movement")]
    public float AccelerationSpeed;
    public float MoveSpeed;
    public float MinMoveSpeed;
    public float MaxMoveSpeed;
    public float RotateSpeed;

    public Transform Body;

    [Header("Border")]
    public Vector3 CentralMapPoint = new Vector3(0, 30, 0);

    //[Header("Health parameters")]
    //public int Health;
    //public bool Alive = true;
    //public string Race;

    //[Header("Detection")]
    //public float DetectDistance;
    //public GameObject Arrow;
    #endregion

    #region HidenVariales
    [HideInInspector] public float CurrentSpeed;
    [HideInInspector] public float CurrentGoalSpeed;
    [HideInInspector] public float CurrentRotSpeed;
    [HideInInspector] public float ChangeSpeed;
    
    [HideInInspector] public float VerticalAngle = 0f;
    protected bool needFlyToCenter;

    protected float gravitySpeedInfluence = 0f;
    protected float gravitySpeedInfluenceChangePerSec = 9f;
    protected float maxGravityInfluence = 20f;
    protected float minGravityInfluence = -18f;
    #endregion

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
    protected int distantAttackAmmoCurrent = 100;

    protected bool canDistantAttack;
    protected float distantAttackTimer;
    public float DistantAttackTimerEnd;

    [Space]
    [Header("Other Visuals")]
    public ParticleSystem SmokeTrailPS;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        changeFlySpeed();
        flyForward();
        rotateHorizontally();
        moveVertical();
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        distantAttackTimer += Time.deltaTime;

        canDistantAttack = distantAttackTimer >= DistantAttackTimerEnd && distantAttackAmmoCurrent > 0 && !needFlyToCenter;

        if ((Input.GetKey(KeyCode.F) || CrossPlatformInputManager.GetButton("Shot")) && canDistantAttack)
        {
            UseDistantAttack();
        }
    }

    protected virtual void UseDistantAttack()
    {
        SmokeMuzzleTrailPS.Play();

        GameObject projectileGO = Instantiate(BulletPR, DistantAttackPoint.position, DistantAttackPoint.rotation);
        ProjectileG3 projectileSC = projectileGO.GetComponent<ProjectileG3>();
        projectileSC.DamageAmount = ProjectileDamage;
        //projectileSC.TargetTeamList = teamSeparator.EnemyTeamList;
        projectileSC.Speed = ProjectileSpeed;
        projectileSC.DeactivateTimer = ProjectileLifeTime;
        //projectileSC.Team = teamSeparator.Team;

        distantAttackAmmoCurrent -= 1;

        Instantiate(MuzzlePR, DistantAttackPoint.position, DistantAttackPoint.rotation);

        DistantAttackAudioSource.pitch = Random.Range(0.95f, 1.05f);
        DistantAttackAudioSource.PlayOneShot(DistantAttackAudioClip);

        distantAttackTimer = 0f;
    }

    void changeFlySpeed()
    {
        bool forwardAcceleration = Input.GetKey(KeyCode.W);
        bool stopAcceleration = Input.GetKey(KeyCode.S);

        if (forwardAcceleration) MoveSpeed += AccelerationSpeed * Time.deltaTime * Time.timeScale;
        else if (stopAcceleration) MoveSpeed -= AccelerationSpeed * Time.deltaTime * Time.timeScale;

        if (MoveSpeed > MaxMoveSpeed)
        {
            MoveSpeed = MaxMoveSpeed;
        }
        else if (MoveSpeed < MinMoveSpeed)
        {
            MoveSpeed = MinMoveSpeed;
        }
    }

    void flyForward()
    {
        gravitySpeedInfluence = gravitySpeedInfluenceChangePerSec * Mathf.Sin(VerticalAngle * 0.0174533f);
        /*
        if (gravitySpeedInfluence > maxGravityInfluence)
        {
            gravitySpeedInfluence = maxGravityInfluence;
        }
        else if (gravitySpeedInfluence < minGravityInfluence)
        {
            gravitySpeedInfluence = minGravityInfluence;
        }
        */

        //Debug.Log(gravitySpeedInfluence);


        CurrentSpeed = MoveSpeed + gravitySpeedInfluence;
        //Move Horizontal
        transform.Translate(CurrentSpeed * Mathf.Cos(VerticalAngle * 0.0174533f) * Vector3.up * Time.deltaTime * Time.timeScale);

        //Debug.Log($"Cos{Mathf.Cos(VerticalAngle * 0.0174533f)}");

        //Move Vertical
        transform.Translate(CurrentSpeed * Mathf.Sin(VerticalAngle * 0.0174533f) * Vector3.forward * Time.deltaTime * Time.timeScale);

        //Debug.Log($"Sin{Mathf.Sin(VerticalAngle * 0.0174533f)}");
    }

    void rotateHorizontally()
    {
        CurrentRotSpeed = RotateSpeed * -CrossPlatformInputManager.GetAxis("Horizontal");
        transform.Rotate(CurrentRotSpeed * Time.deltaTime * Time.timeScale * Vector3.forward);
    }

    void moveVertical()
    {
        VerticalAngle -= RotateSpeed * CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * Time.timeScale;

        if (VerticalAngle < -90) VerticalAngle = -90;
        if (VerticalAngle > 90) VerticalAngle = 90;

        Body.localEulerAngles = new Vector3(
            VerticalAngle,
            0f,
            0f
            );

    }
}
