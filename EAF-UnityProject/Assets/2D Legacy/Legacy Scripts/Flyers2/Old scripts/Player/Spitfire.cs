using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Spitfire : MonoBehaviour { 

    public float attack_timer = 0.6f;
    private float current_attack_timer;
    private bool can_attack;

    public static bool Alive = true;
    public int Health;
    public int MaxAmmo;
    public static int CurrentAmmo;

    public Transform attack_point;
    public GameObject muzzle;
    public GameObject DeathPanelUIObject;
    public GameObject HealthBarUIObject;
    public HealthBar HealthBarScript;

    public ParticleSystem SmokeTrailPS;
    public ParticleSystem SmokeMuzzleTrailPS;

    public AudioSource ShotSound;
    public AudioSource BlastHit;
    public AudioSource RamHit;

    ObjectPooler objectPooler;
    private Animator anim;
    // Start is called before the first frame update
    private void Awake()
    {
        DeathPanelUIObject = GameObject.Find("Death panel");
        CurrentAmmo = MaxAmmo;
        anim = GetComponent<Animator>();
        Alive = true;  
    }

    void Start()
    {
        current_attack_timer = attack_timer;
        objectPooler = ObjectPooler.Instance;
        

        if (DifficultyLevels.DifficultyLevel < 3)
        {
            HealthBarUIObject = GameObject.Find("Health bar");
            HealthBarScript = HealthBarUIObject.GetComponent<HealthBar>();
        }

        if (DifficultyLevels.DifficultyLevel == 1) { Health = 20; HealthBarScript.SetMaxHealth(Health); }
        if (DifficultyLevels.DifficultyLevel == 2) { Health = 10; HealthBarScript.SetMaxHealth(Health); }
        if (DifficultyLevels.DifficultyLevel == 3) { Health = 1; }
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        attack_timer += Time.deltaTime;
        if (attack_timer > current_attack_timer)
        {
            can_attack = true;
        }

        if ((Input.GetKey(KeyCode.K) || CrossPlatformInputManager.GetButton("Shot")) && can_attack && CurrentAmmo>0)
        {
             can_attack = false;
             attack_timer = 0f;

             ShotSound.Play();

             GameObject bulletGO = objectPooler.SpawnFromPool("player_bullet", attack_point.position, transform.rotation);
             RifleBullet bulletSC = bulletGO.GetComponent<RifleBullet>();
             bulletSC.DamageAmount = 20;
             bulletSC.TargetTeam = "CAF";

             CurrentAmmo--;
             objectPooler.SpawnFromPool("muzzle", attack_point.position, transform.rotation);
             SmokeMuzzleTrailPS.Play();
        }
    }

    void OnTriggerEnter(Collider target)
    {
        var mainParticleSystem = SmokeTrailPS.main;
        if (target.tag == "Blast")
        {
            Health -= 4;
            BlastHit.Play();
            if (DifficultyLevels.DifficultyLevel < 3) { HealthBarScript.SetHealth(Health); }

            SmokeTrailPS.Pause();
            mainParticleSystem.duration = 1f;
            SmokeTrailPS.Play();

        }
        if (target.tag == "CAF")
        {
            Health -= 6;
            RamHit.Play();
            if (DifficultyLevels.DifficultyLevel < 3) { HealthBarScript.SetHealth(Health); }

            SmokeTrailPS.Pause();
            mainParticleSystem.duration = 2.5f;
            SmokeTrailPS.Play();
        }

        if (Health <= 0)
        {
            Alive = false;
            DeathPanelUIObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}

