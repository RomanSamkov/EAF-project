using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet3D : MonoBehaviour
{
    //rot.x = 0 is horizontal

    public GameObject GroundHitPR;

    public Transform Body;

    //public Transform ForwardPos;

    public float AirStopEffectPerSec;
    public float StartSpeed;
    public float CurrentSpeed;
    public float GravityAcceleration;
    protected float CurrentGravityInfluence = 0;

    public float DeactivateTimer;

    protected Vector3 PointOfDirection;

    protected int layerLevelMask;

    void Start()
    {
        CurrentSpeed = StartSpeed;
        layerLevelMask = LayerMask.GetMask("Level");
        Invoke("DeactivateGameObject", DeactivateTimer);
    }

    void FixedUpdate()
    {
        RayCastCheck();

        //forward
        transform.Translate(CurrentSpeed * Vector3.forward * Time.deltaTime);

        transform.Translate(-CurrentGravityInfluence * Vector3.up * Time.deltaTime, Space.World);

        CurrentGravityInfluence += GravityAcceleration * Time.deltaTime;
        CurrentSpeed -= AirStopEffectPerSec * Time.deltaTime;

        PointOfDirection = transform.position + transform.forward * CurrentSpeed + Vector3.up * -CurrentGravityInfluence;

        Body.LookAt(PointOfDirection);
    }


    void RayCastCheck()
    {
        if (Physics.Linecast(Body.transform.position + Body.transform.forward * -1.5f, Body.transform.position + Body.transform.forward *1.5f, layerLevelMask)
            || Physics.Linecast(Body.transform.position + Body.transform.up * 0.2f, Body.transform.position + Body.transform.up * -0.2f, layerLevelMask)
            || Physics.Linecast(Body.transform.position + Body.transform.right * 0.2f, Body.transform.position + Body.transform.right * -0.2f, layerLevelMask))

        {
            Instantiate(GroundHitPR, transform.position, Quaternion.identity);
            DeactivateGameObject();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Body.transform.position + Body.transform.forward * -1.5f, Body.transform.position + Body.transform.forward * 1.5f);
        Gizmos.DrawLine(Body.transform.position + Body.transform.up * 0.2f, Body.transform.position + Body.transform.up * -0.2f);
        Gizmos.DrawLine(Body.transform.position + Body.transform.right * 0.2f, Body.transform.position + Body.transform.right * -0.2f);
    }


    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
