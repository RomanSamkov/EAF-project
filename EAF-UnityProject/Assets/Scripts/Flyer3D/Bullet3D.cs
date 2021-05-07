using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet3D : MonoBehaviour
{
    //rot.x = 0 is horizontal

    public GameObject GroundHitPR;

    //public Transform ForwardPos;

    public float AirStopEffectPerSec;
    public float StartSpeed;
    public float CurrentSpeed;
    public float GravityAcceleration;
    protected float CurrentGravityInfluence = 0;

    public float DeactivateTimer;

    float VerticalAngle;

    float sinus;
    float cosine;

    protected int layerLevelMask;

    void Start()
    {
        CurrentSpeed = StartSpeed;
        layerLevelMask = LayerMask.GetMask("Level");
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
        RayCastCheck();
        //forward
        transform.Translate(CurrentSpeed * Vector3.forward * Time.deltaTime);

        transform.Translate(-CurrentGravityInfluence * Vector3.up * Time.deltaTime, Space.World);

        CurrentGravityInfluence += GravityAcceleration * Time.deltaTime;
        CurrentSpeed -= AirStopEffectPerSec * Time.deltaTime;
        
    }

    void RayCastCheck()
    {
        if (Physics.Linecast(transform.position + Vector3.forward * -1.5f, transform.position + Vector3.forward*1.5f, layerLevelMask)
            || Physics.Linecast(transform.position + Vector3.up * 0.5f, transform.position + Vector3.up * -0.5f, layerLevelMask)
            || Physics.Linecast(transform.position + Vector3.right * 0.5f, transform.position + Vector3.right * -0.5f, layerLevelMask))

        {
            Instantiate(GroundHitPR, transform.position, Quaternion.identity);
            DeactivateGameObject();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.forward * -1.5f, transform.position + Vector3.forward * 1.5f);
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, transform.position + Vector3.up * -0.5f);
    }


    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
