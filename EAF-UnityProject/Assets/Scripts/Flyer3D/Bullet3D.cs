using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet3D : MonoBehaviour
{
    //rot.x = 0 is horizontal

    public float AirStopEffectPerSec;
    public float StartSpeed;
    public float CurrentSpeed;
    public float GravityAcceleration;
    protected float CurrentGravityInfluence = 0;

    public float DeactivateTimer;

    float VerticalAngle;

    float sinus;
    float cosine;

    void Start()
    {
        CurrentSpeed = StartSpeed;
        Invoke("DeactivateGameObject", DeactivateTimer);
    }

    void FixedUpdate()
    {
        /*
        transform.Translate(CurrentSpeed * Vector3.forward * Time.deltaTime);
        transform.Translate(CurrentGravityInfluence * Vector3.up*-1 * Time.deltaTime);
        CurrentGravityInfluence += GravityAcceleration * Time.deltaTime;
        CurrentSpeed -= AirStopEffectPerSec * Time.deltaTime;
        */

        //forward
        transform.Translate(CurrentSpeed * Vector3.forward * Time.deltaTime);

        transform.Translate(-CurrentGravityInfluence * Vector3.up * Time.deltaTime, Space.World);

        CurrentGravityInfluence += GravityAcceleration * Time.deltaTime;
        CurrentSpeed -= AirStopEffectPerSec * Time.deltaTime;
        
    }



    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
