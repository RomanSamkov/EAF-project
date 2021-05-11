using System.Collections.Generic;
using UnityEngine;

public class ProjectileG3 : MonoBehaviour
{
    public float Speed;
    public float DeactivateTimer;

    //public string[] TargetTeamList;
    public List<string> TargetTeamList;
    public string Team;

    public int DamageAmount;
    public string DamageType;

    protected Vector3 previousProjectilePos;

    protected Rigidbody rb;
    protected BoxCollider bcollider;
    protected SpriteRenderer srenderer;

    protected virtual void Start()
    {
        previousProjectilePos = transform.position;
        //Debug.Log($"previousProjectilePos is { previousProjectilePos}, num is {i}");
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * Speed;
    }

    protected void Update()
    {
        previousProjectilePos = transform.position;
    }

    public virtual void TargetTestAttack(Collider thing)
    {
        //If collide to flyer
        if (thing.tag == "Flyers")
        {
            TeamSeparatorG3 team = thing.GetComponent<TeamSeparatorG3>();

            //Check if it is enemy
            bool ItIsEnemy = false;

            foreach(string listTeam in TargetTeamList)
            {
                ItIsEnemy = listTeam == team.Team;
                if (ItIsEnemy) break;
            }

            //Do things whith enemy
            if (ItIsEnemy)
            {
                FlyerG3 target = thing.GetComponent<FlyerG3>();

                target.SetDamage(DamageAmount, DamageType, previousProjectilePos);

                switch (target.Race)
                {
                    case ("chn"):
                        {
                            EnemyChnCollide();
                            break;
                        }
                    case ("pgs"):
                        {
                            EnemyPgsCollide();
                            break;
                        }
                    case ("grf"):
                        {
                            EnemyGrfCollide();
                            break;
                        }
                    default:
                        {
                            EnemyDefCollide();
                            break;
                        }
                }
            }
        }

        if (thing.tag == "DistAttack")
        {
            ProjectileG3 projectileSC = thing.GetComponent<ProjectileG3>();

            bool ItIsEnemy = false;

            foreach (string listTeam in TargetTeamList)
            {
                ItIsEnemy = listTeam == projectileSC.Team;
                if (ItIsEnemy) break;
            }

            if (ItIsEnemy)
            {
                switch (projectileSC.DamageType)
                {
                    case ("bullet"):
                        {
                            EnemyBulletCollide();
                            break;
                        }
                    case ("blast"):
                        {
                            EnemyBlastCollide();
                            break;
                        }
                }
            }
        }
    }

    public virtual void EnemyBulletCollide()
    {

    }

    public virtual void EnemyBlastCollide()
    {

    }

    public virtual void EnemyChnCollide()
    {

    }

    public virtual void EnemyPgsCollide()
    {

    }

    public virtual void EnemyGrfCollide()
    {

    }

    public virtual void EnemyDefCollide()
    {

    }

    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    public virtual void SetHalfActive()
    {
        Speed = 0;
        bcollider.enabled = false;
        srenderer.enabled = false;
    }

    public virtual void EndOfFly(float timer, float lastTime)
    {
        Invoke("SetHalfActive", timer);
        Invoke("DeactivateGameObject", timer + lastTime);
    }
}
