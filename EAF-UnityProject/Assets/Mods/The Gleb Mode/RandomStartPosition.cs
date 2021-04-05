using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStartPosition : MonoBehaviour
{
    public float Radius = 75;

    void Start()
    {
        transform.position = new Vector3(Random.Range(-Radius, Radius), 30f, Random.Range(-Radius, Radius));
        transform.eulerAngles = new Vector3(90, 0, Random.Range(0, 360));
    }
}
