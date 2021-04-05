using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float RotationSpeedX;
    public float RotationSpeedY;
    public float RotationSpeedZ;



    void Update()
    {
        Randomize();
        transform.Rotate(RotationSpeedX*Time.deltaTime, RotationSpeedY * Time.deltaTime, RotationSpeedZ * Time.deltaTime);
    }

    private float currentChangeTimer = 2.5f;
    private float fullShangeTimer = 2.5f;

    void Randomize()
    {
        currentChangeTimer += Time.deltaTime;
        if(currentChangeTimer>= fullShangeTimer)
        {
            RotationSpeedX = Random.Range(-180, 180);
            RotationSpeedY = Random.Range(-180, 180);
            RotationSpeedZ = Random.Range(-180, 180);
            currentChangeTimer = 0f;
        }
    }
}
