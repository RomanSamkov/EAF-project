using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class TestPlayer3D : MonoBehaviour
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

    public float ProjectileSpeed;
    public float ProjectileLifeTime;
    public int ProjectileDamage;

    public int DistantAttackAmmoMax;
    protected int distantAttackAmmoCurrent = 10000;

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
        rotateHorizontally();
        moveVertical();
        flyForward();
        ChangeBodyRotation();
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        distantAttackTimer += Time.deltaTime;

        canDistantAttack = distantAttackTimer >= DistantAttackTimerEnd && distantAttackAmmoCurrent > 0;

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

    bool forwardAcceleration;
    bool stopAcceleration;
    void changeFlySpeed()
    {
        forwardAcceleration = Input.GetKey(KeyCode.W) || CrossPlatformInputManager.GetButton("Move Forward");
        stopAcceleration = Input.GetKey(KeyCode.S) || CrossPlatformInputManager.GetButton("Stop");

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
        float sinus = Mathf.Sin(VerticalAngle * 0.0174533f) * -1;

        if (sinus < -0.5f && !stopAcceleration)
        {
            gravitySpeedInfluence -= gravitySpeedInfluenceChangePerSec * (sinus + 0.5f) * Time.deltaTime * Time.timeScale;
        }
        else if (sinus < -0.5f && stopAcceleration)
        {
            gravitySpeedInfluence -= gravitySpeedInfluenceChangePerSec * (sinus + 1f) * Time.deltaTime * Time.timeScale;
        }
        else if (sinus >= -0.5f && gravitySpeedInfluence > 0 && !forwardAcceleration)
        {
            gravitySpeedInfluence -= gravitySpeedInfluenceChangePerSec * (sinus + 0.5f) * Time.deltaTime * Time.timeScale;
        }
        else if (sinus > 0 && gravitySpeedInfluence <= 0)
        {
            gravitySpeedInfluence = -1 * (MoveSpeed * (sinus/2)); 
        }

        if (gravitySpeedInfluence < 0 && sinus < 0) gravitySpeedInfluence = 0;
        if (gravitySpeedInfluence > maxGravityInfluence) gravitySpeedInfluence = maxGravityInfluence;

        CurrentSpeed = MoveSpeed + gravitySpeedInfluence;

        //Move Horizontal
        transform.Translate(CurrentSpeed * Mathf.Cos(VerticalAngle * 0.0174533f) * Vector3.up * Time.deltaTime * Time.timeScale);

        //Move Vertical
        transform.Translate(-1 * CurrentSpeed * sinus * Vector3.forward * Time.deltaTime * Time.timeScale);
    }

    void rotateHorizontally()
    {
        GoalHorRotSpeed = MaxRotSpeed * CrossPlatformInputManager.GetAxis("Horizontal")*-1;

        if(CurrentHorRotSpeed > GoalHorRotSpeed)
        {
            CurrentHorRotSpeed -= AccelerationRotateSpeed * Time.deltaTime * Time.timeScale;
            if (CurrentHorRotSpeed - 1f < GoalHorRotSpeed && CurrentHorRotSpeed + 1f > GoalHorRotSpeed) CurrentHorRotSpeed = GoalHorRotSpeed;
        }
        else if (CurrentHorRotSpeed < GoalHorRotSpeed)
        {
            CurrentHorRotSpeed += AccelerationRotateSpeed * Time.deltaTime * Time.timeScale;
            if (CurrentHorRotSpeed - 1f < GoalHorRotSpeed && CurrentHorRotSpeed + 1f > GoalHorRotSpeed) CurrentHorRotSpeed = GoalHorRotSpeed;
        }

        transform.Rotate(CurrentHorRotSpeed * Time.deltaTime * Time.timeScale * Vector3.forward);
    }

    void moveVertical()
    {
        GoalVerRotSpeed = MaxRotSpeed * CrossPlatformInputManager.GetAxis("Vertical");

        if (CurrentVerRotSpeed > GoalVerRotSpeed)
        {
            CurrentVerRotSpeed -= AccelerationRotateSpeed * Time.deltaTime * Time.timeScale;
            if (CurrentVerRotSpeed - 0.1f < GoalVerRotSpeed && CurrentVerRotSpeed + 0.1f > GoalVerRotSpeed) CurrentVerRotSpeed = GoalVerRotSpeed;
        }
        else if (CurrentVerRotSpeed < GoalVerRotSpeed)
        {
            CurrentVerRotSpeed += AccelerationRotateSpeed * Time.deltaTime * Time.timeScale;
            if (CurrentVerRotSpeed - 0.1f < GoalVerRotSpeed && CurrentVerRotSpeed + 0.1f > GoalVerRotSpeed) CurrentVerRotSpeed = GoalVerRotSpeed;
        }

        VerticalAngle -= CurrentVerRotSpeed  * Time.deltaTime * Time.timeScale;
    }

    void ChangeBodyRotation()
    {
        float percentOfMaxSpeed = (MoveSpeed - MinMoveSpeed) / (MaxMoveSpeed - MinMoveSpeed);
        float percentOfRotSpeed = CurrentHorRotSpeed / MaxRotSpeed;

        Vertical.localEulerAngles = new Vector3(
            VerticalAngle,
            0f,
            0f
            );

        Body.transform.localEulerAngles = new Vector3(
            0f,
            90f + 45f * percentOfRotSpeed * percentOfMaxSpeed,
            -90f
            );

        //Debug.Log($"percentOfRotSpeed ({percentOfRotSpeed}) * percentOfMaxSpeed({percentOfMaxSpeed}) = {percentOfRotSpeed * percentOfMaxSpeed}");
    }
}
