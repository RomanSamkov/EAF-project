using UnityEngine;

public class PegasusBaseAiG3 : PegasusG3
{
    protected Vector3 posToShoot;
    protected float aimDistanceToAttackVector;
    protected float aimDistanceToTarget;
    protected float movVecDistance;
    protected bool targetForward;

    protected override void Awake()
    {
        base.Awake();
        RandomizeMobility();
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        //if(TargetGO!=null) Debug.Log(TargetGO.name);
        //else Debug.Log("Cant define enemy");
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        RandomizeMobility();

        // Debug.Log(distantAttackCurrentEnergy);

        if (fighting)
            distanceToTarget = Vector3.Distance(TargetGO.transform.position, transform.position);

        if (Alive && fighting && !needFlyToCenter)
        {
            previsionInterceptBehavior();
        }
        else if (Alive && needFlyToCenter)
        {
            FlyBackToBorders();
        }
        else if (Alive && !fighting)
        {
            NoTargetBehavior();
        }
        else if (!Alive) DeathFall();
    }

    #region MainAIFunctions

    protected void FindMovementData()
    {
        Rigidbody TargetRB = TargetGO.GetComponent<Rigidbody>();

        //Find data about direction relative to target
        float forwardDirectionDistance = Vector3.Distance(transform.position + transform.up * 1f, TargetGO.transform.position);
        targetForward = distanceToTarget > forwardDirectionDistance;
        //Check if direction is on target
        if (targetForward)
        {
            movementVector = TargetGO.transform.position +
            TargetRB.velocity * (TargetRB.velocity.magnitude / ProjectileSpeed) * (distanceToTarget / ProjectileSpeed) * TargetRB.velocity.magnitude * .5f;
        }
        else
        {
            movementVector = TargetGO.transform.position;
        }

        movVecDistance = Vector3.Distance(transform.position, movementVector);
        aimDistanceToTarget = Vector3.Distance(TargetGO.transform.position, movementVector);

        aimDistanceToAttackVector = Vector3.Distance(transform.position + transform.up * movVecDistance, movementVector);
    }
    /*
     Точка на которую надо навестить что бы попасть
      В направлении движения прибавить  СкоростьЦели/СкоростьСнаряда*(Расстояние до цели/Скорость пули)
     */

    protected virtual void previsionInterceptAttack()
    {
       if (canDistantAttack && aimDistanceToAttackVector < 10f * (distanceToTarget / 20))
       {
           UseDistantAttack();
       }
    }

    #endregion

    #region Behaviors
    protected void previsionInterceptBehavior()
    {
        FlyToVector(movementVector);

        if (needResetMovement)
        {
            ResetMovement();
            needResetMovement = false;
        }

        FindMovementData();
        previsionInterceptAttack();
    }

    protected void backInterceptBehavior()
    {

    }

    #endregion

    public override void SetDeathSettings()
    {
        base.SetDeathSettings();

        //Arrow.SetActive(false);
    }

    public override void DeathVisual()
    {
        SmokeTrailPS.Play();
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(movementVector, 10f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.up * movVecDistance, 4f);
    }
}
