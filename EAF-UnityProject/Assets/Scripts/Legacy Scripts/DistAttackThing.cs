using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class DistAttackThing : MonoBehaviour
{
    public float Speed;
    public float DeactivateTimer;

    public string TargetTeam;

    public int DamageAmount;
    public string DamageType;

    protected Rigidbody rb;
    protected BoxCollider bcollider;
    protected SpriteRenderer srenderer;

    /*
    public virtual void OnObjectSpawn()
    {
        Invoke("DeactivateGameObject", DeactivateTimer);
    }
    */

    protected virtual void FlyForward()
    {
        rb.velocity = transform.up * Speed;
    }

    public virtual void TargetTestAttack(Collider thing)
    {
        if (thing.tag == "Flyers")
        {
            TeamSeparator team = thing.GetComponent<TeamSeparator>();
            if (team.Team == TargetTeam)
            {
                Flyer target = thing.GetComponent<Flyer>();
                target.SetDamage(DamageAmount, DamageType);
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
            DistAttackThing DistAttackSC = thing.GetComponent<DistAttackThing>();
            if(DistAttackSC.TargetTeam != TargetTeam)
            {
                switch (DistAttackSC.DamageType)
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
        Invoke("DeactivateGameObject", timer+lastTime);
    }
}
