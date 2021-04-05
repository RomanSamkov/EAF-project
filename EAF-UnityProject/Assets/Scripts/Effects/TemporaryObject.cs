using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryObject : MonoBehaviour, PooledObject
{
    public float LiveTime;

    public void OnObjectSpawn()
    {
        Invoke("DeactivateGameObject", LiveTime);
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
