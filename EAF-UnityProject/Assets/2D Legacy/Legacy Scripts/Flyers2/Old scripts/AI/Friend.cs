using UnityEngine;

public class Friend : MonoBehaviour
{
    public bool alive = true;

    public int Health;

    public float moveSpeed = 9f;
    public float rotateSpeed = 1f;
    public float minMoveSpeed = 16f;
    public float maxMoveSpeed = 18f;
    public float minRotateSpeed = 1.6f;
    public float maxRotateSpeed = 1.8f;
    public float ramSpeed = 13f;
    public float ramRotate = 0.4f;

    //Timers to randomize characteristics
    public float randomize_timer = 5f;
    private float current_randomize_time;
    private bool can_randomize = true;

    public float shot_timer = 2f;
    private float current_shot_timer;

    public float detect_distance;

    public Transform target;
    public Transform aim;
    public Transform attack_point;
    public GameObject arrow;
    private GameObject player;

    public ParticleSystem SmokeTrailPS;
    public ParticleSystem SmokeMuzzleTrailPS;

    public AudioSource ShotSound;
    public AudioSource BlastHit;
    public AudioSource RamHit;

    public bool must_survive;

    /*
    public bool tp_respawn_enable;
    public Vector3 tp_respawn_position;
    */
    private float player_distance;

    private float fall_speed = 0f;

    private float distance;
    private float aim_dist;

    ObjectPooler objectPooler;
    BoxCollider bcollider;
    Rigidbody rb;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("EAF").transform;
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    void Start()
    {
        current_randomize_time = randomize_timer;
        current_shot_timer = shot_timer;
        objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody>();
        bcollider = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            distance = Vector3.Distance(target.position, transform.position);
            aim_dist = Vector3.Distance(target.position, aim.position);
            if (can_randomize) { Randomize(); }
            Attack();
        }

        if (!alive) { fall_speed += 10f * Time.deltaTime; }
        if (!alive && moveSpeed > 0f) { moveSpeed -= 1f * Time.deltaTime; }
    }

    //
    void UpdateTarget()
    {
        if (alive)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("CAF");
            float shortestDist = Mathf.Infinity;

            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < shortestDist)
                {
                    shortestDist = distance;
                    nearestEnemy = enemy;
                }
            }

            if (shortestDist > 20f)
            {
                target = nearestEnemy.transform.Find("Trajectory point");
            }
            else
            {
                target = nearestEnemy.transform;
            }
        }
        else
        {
            target = aim;
        }
    }
    //

    private void FixedUpdate()
    {

        Vector3 direction = target.position - rb.position;
        direction.Normalize();
        Vector3 rotationAmount = Vector3.Cross(transform.up, direction);
        rb.angularVelocity = rotationAmount * rotateSpeed;

        if (alive) { rb.velocity = transform.up * moveSpeed; }
        if (!alive) { rb.velocity = transform.up * moveSpeed + transform.forward * fall_speed; }
    }



    void Randomize()
    {
        randomize_timer += Time.deltaTime;
        if (randomize_timer > current_randomize_time)
        {
            moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
            rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
            randomize_timer = 0f;
        }
    }

    void Attack()
    {
        shot_timer += Time.deltaTime;
        if ((distance > 10 && shot_timer > current_shot_timer && aim_dist < 15)||(distance <= 10 && shot_timer > current_shot_timer && aim_dist < 8))
        {
            ShotSound.Play();

            objectPooler.SpawnFromPool("player_bullet", attack_point.position, transform.rotation);
            objectPooler.SpawnFromPool("muzzle", attack_point.position, transform.rotation);
            
            SmokeMuzzleTrailPS.Play();

            shot_timer = 0f;
        }
        
    }

    private void OnTriggerEnter(Collider thing)
    {
        if (thing.tag == "Blast" || thing.tag == "CAF")
        {
            var mainParticleSystem = SmokeTrailPS.main;
            if (target.tag == "Blast")
            {
                Health -= 4;
                BlastHit.Play();

                SmokeTrailPS.Pause();
                mainParticleSystem.duration = 1f;
                SmokeTrailPS.Play();
            }
            if (target.tag == "CAF")
            {
                Health -= 6;
                RamHit.Play();

                SmokeTrailPS.Pause();
                mainParticleSystem.duration = 2f;
                SmokeTrailPS.Play();
            }

            if (Health <= 0)
            {
                alive = false;
                rb.constraints = RigidbodyConstraints.None;
                Invoke("DeactivateGameObject", 3f);
                bcollider.enabled = false;
                if (must_survive) { Mission.MissionFailed = true; }
            }
        }
    }

    void ArrowHost()
    {
        player_distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < detect_distance && alive)
        {
            arrow.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }
    }
    
    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
