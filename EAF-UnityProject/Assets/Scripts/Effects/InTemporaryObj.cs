using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTemporaryObj : MonoBehaviour
{
    public float LiveTime;

    private void Awake()
    {
        Invoke("DeactivateGameObject", LiveTime);
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
