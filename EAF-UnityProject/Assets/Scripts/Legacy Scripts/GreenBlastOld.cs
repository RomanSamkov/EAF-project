using UnityEngine;

public class GreenBlastOld : MonoBehaviour, PooledObject
{
    public float speed;
    public float deactivate_timer;

    ObjectPooler objectPooler;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectPooler = ObjectPooler.Instance;
    }

    public void OnObjectSpawn()
    {
        Invoke("DeactivateGameObject", deactivate_timer);
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player" || target.tag == "EAF" || target.tag == "Rifle_Bullet")
        {
            
            objectPooler.SpawnFromPool("GreenExplosionPS", transform.position, transform.rotation);
            
            gameObject.SetActive(false);
        }
    }
}

