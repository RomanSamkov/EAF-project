using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalWaveSpawner : MonoBehaviour
{
    public static int wave = 1;
    public int wave_ch_count;
    public int wave_multiplication;

    public float spawn_timer;
    private float current_spawn_timer;

    public GameObject changeling;

    public Transform spawner1;
    public Transform spawner2;
    public Transform spawner3;
    public Transform spawner4;
    public Transform spawner5;
    public Transform spawner6;
    public Transform spawner7;
    public Transform spawner8;

    public Transform spawner;

    public static float timer = 0f;

    ObjectPooler objectPooler;
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        current_spawn_timer = 10/3;
    }

    private void Awake()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        spawn_timer += Time.deltaTime;
        if(current_spawn_timer <= spawn_timer)
        {
            RandomizeSpawn();
            Instantiate(changeling, spawner.position, spawner.rotation);
            spawn_timer = 0f;
        }

        current_spawn_timer = 10 / (3 + timer/60);

    }

    void RandomizeSpawn()
    {
        int i = Random.Range(1, 8);
        if (i == 1) { spawner = spawner1; }
        if (i == 2) { spawner = spawner2; }
        if (i == 3) { spawner = spawner3; }
        if (i == 4) { spawner = spawner4; }
        if (i == 5) { spawner = spawner5; }
        if (i == 6) { spawner = spawner6; }
        if (i == 7) { spawner = spawner7; }
        if (i == 8) { spawner = spawner8; }
    }
}
