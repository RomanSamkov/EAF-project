using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour//, PooledObject
{
    public float LiveTime;
    public AudioSource[] Sounds;

    private void Start()
    {
        Invoke("DeactivateGameObject", LiveTime);
        Sounds[Random.Range(0, Sounds.Length - 1)].Play();
    }

    /*
    public void OnObjectSpawn()
    {
        Invoke("DeactivateGameObject", LiveTime);
        Sounds[Random.Range(0, Sounds.Length - 1)].Play();
    }
    */

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
