using UnityEngine;

public class GreenBigParticle : MonoBehaviour, PooledObject
{
    public float deactivate_timer;

    public float min_speed;
    public float max_speed;

    public float min_side_speed;
    public float max_side_speed;

    private float speed;
    private float side_speed;

    ObjectPooler objectPooler;
    Rigidbody rb;
    public void OnObjectSpawn()
    {
        Invoke("DeactivateGameObject", deactivate_timer);
        speed = Random.Range(min_speed, max_speed);
        side_speed = Random.Range(min_side_speed, max_side_speed);
    }

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed + transform.right * side_speed;
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
