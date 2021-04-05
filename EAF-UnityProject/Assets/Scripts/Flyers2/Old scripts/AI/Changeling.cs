using UnityEngine;

public class Changeling : MonoBehaviour
{
    /*
    public bool alive = true;
    public bool can_disguise = false;
    public float moveSpeed = 9f;
    public float rotateSpeed = 1f;
    public float minMoveSpeed = 5f;
    public float maxMoveSpeed = 14f;
    public float minRotateSpeed = 0.8f;
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

    public float disguise_distance;

    public Vector3 target;
    public Transform AimPoint;
    public Transform ram_point;
    public GameObject ram;
    public GameObject disguise;
    public GameObject arrow;
    private GameObject player;
    
    public bool tp_respawn_enable;
    public Vector3 tp_respawn_position;
    

    private bool disguise_active = false;
    private float player_distance;

    private float distance;
    private float aim_dist;

    private float fall_speed = 0f;
    //private float max_fall_speed = 100f;

    public ParticleSystem GreenTrailPS;
    public ParticleSystem GreenExplosionPS;
    public ParticleSystem GreenRamExplosionPS;
    public ParticleSystem SmokeTrailPS;

    public AudioSource[] HitSounds;

    ObjectPooler objectPooler;
    BoxCollider bcollider;
    Rigidbody rb;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Flyers").transform.position;
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        if (can_disguise) { InvokeRepeating("Disguise", 0f, 1f); }
    }

    void Start()
    {
        current_randomize_time = randomize_timer;
        current_shot_timer = shot_timer;
        objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody>();
        bcollider = GetComponent<BoxCollider>();
        player = GameObject.Find("Player");
        Mission.changelings_number++;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            distance = Vector3.Distance(target, transform.position);
            aim_dist = Vector3.Distance(target, AimPoint.position);

            if (!disguise_active)
            {
                if (can_randomize) { Randomize(); }
                Attack();
            }
        }

        ArrowHost();

        if (!alive) { fall_speed += 10f * Time.deltaTime; }
        if (!alive && moveSpeed>0f) { moveSpeed -= 1f * Time.deltaTime; }
    }
    
    void UpdateTarget()
    {
        if (alive)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Flyers");
            float shortestDist = Mathf.Infinity;

            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                var team = enemy.GetComponent<TeamSeparator>();
                if (team.Team == "EAF")
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < shortestDist)
                    {
                        shortestDist = distance;
                        nearestEnemy = enemy;
                    }
                }
            }

            distance = shortestDist;


            if (shortestDist > 20f)
            {
                target = nearestEnemy.transform.position + nearestEnemy.transform.up * 20f;
            }
            else
            {
                target = nearestEnemy.transform.position;
            }
        }
        else
        {
            target = transform.position + transform.up * 10f;
        }
        
    }
    //

    private void FixedUpdate() {

        Vector3 direction = target - rb.position;
        direction.Normalize();
        Vector3 rotationAmount = Vector3.Cross(transform.up, direction);
        rb.angularVelocity = rotationAmount * rotateSpeed;

        if (disguise_active) { rb.velocity = transform.up * 0f; }
        if (!disguise_active) { rb.velocity = transform.up * moveSpeed; }
        if (!alive) { rb.velocity = transform.up * moveSpeed + transform.forward *fall_speed; }
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
            if (distance > 10 && shot_timer > current_shot_timer)
            {
                ram.SetActive(false);
                GreenTrailPS.Pause();
            if (aim_dist < 15)
                {
                    objectPooler.SpawnFromPool("blast", transform.position, transform.rotation);
                    shot_timer = 0f;
                }
            can_randomize = true;
            }
            if (distance <= 10)
            {
                if(Vector3.Distance(target, ram_point.position) < 5)
                {
                    ram.SetActive(true);
                    GreenTrailPS.Play();
                    moveSpeed = Random.Range(ramSpeed-2f, ramSpeed+2f);
                    rotateSpeed = Random.Range(ramRotate-0.2f, ramRotate+0.2f);
                }
                else
                {
                    ram.SetActive(false);
                    GreenTrailPS.Pause();
                }
                randomize_timer = current_randomize_time;
                can_randomize = false;
            }
    }

    private void OnTriggerEnter(Collider thing)
    {
        if(thing.tag == "Rifle_Bullet")
        {
            if (disguise_active) { moveSpeed = 0f; }

            alive = false;

            rb.constraints = RigidbodyConstraints.None;

            bcollider.enabled = false;

            ram.SetActive(false);
            GreenTrailPS.Pause();

            disguise.SetActive(false);
            disguise_active = false;

            int i = Random.Range(0, 1);
            HitSounds[i].Play();

            GreenExplosionPS.Play();
            SmokeTrailPS.Play();

            Invoke("DeactivateGameObject", 3f);

            Mission.kill_counter++;
        }
        if((thing.tag == "Player" || thing.tag == "EAF")&& ram.activeSelf)
        {
            GreenRamExplosionPS.Play();
        }
    }

    void Disguise() {
        int i = Random.Range(0, 5);
        if(distance > disguise_distance && distance < detect_distance+10f && aim_dist > 5f && alive && i<3)
        {
            disguise.SetActive(true);
            ram.SetActive(false);
            GreenTrailPS.Pause();
            disguise_active = true;
        }
        else
        {
            disguise.SetActive(false);
            disguise_active = false;
        }
    }

    void ArrowHost()
    {
        player_distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < detect_distance && alive && !disguise_active)
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
    */
}
