using UnityEngine;

public class RifleBulletOld : MonoBehaviour, PooledObject
{
    public float speed;
    public float deactivate_timer;

    public string TargetTeam;

    public int DamageAmount;
    public string DamageType;

    ObjectPooler objectPooler;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody>();
        DamageType = "bullet";
    }

    public void OnObjectSpawn()
    {
        Invoke("DeactivateGameObject", deactivate_timer);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter(Collider thing)
    {
        TargetTestAttack(thing);
    }

    void TargetTestAttack(Collider thing)
    {
        TeamSeparator team = thing.GetComponent<TeamSeparator>();
        if (team.Team == TargetTeam)
        {
            Flyer target = thing.GetComponent<ChangelingAI>();
            switch (target.Race)
            {
                case ("chn"):
                    {
                        objectPooler.SpawnFromPool("GreenDirectSplashPS", transform.position, transform.rotation);
                        break;
                    }
                case ("pgs"):
                    {
                        break;
                    }
                case ("grf"):
                    {
                        break;
                    }
                default:
                    {

                        break;
                    }
            }
            target.SetDamage(DamageAmount, DamageType);
            gameObject.SetActive(false);
        }
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
