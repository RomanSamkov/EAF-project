using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model3D : MonoBehaviour
{
    //Этот класс должен отвечать за отрисовку моделей и за их кастомизацию

    //public GameObject modelGO;
    protected Animator anim;
    public TestPlayer3D flyer;

    protected float PercentOfMaxSpeed;
    protected float percentOfRotSpeed;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        flyer = flyer.GetComponent<TestPlayer3D>();
    }

    protected virtual void Update()
    {
        PercentOfMaxSpeed = (flyer.MoveSpeed - flyer.MinMoveSpeed) / (flyer.MaxMoveSpeed - flyer.MinMoveSpeed);
        percentOfRotSpeed = flyer.CurrentRotSpeed / flyer.RotateSpeed;
        CurrentFlyAnimationSpeed();
        ModelPoseUpdate();
    }

    protected virtual void CurrentFlyAnimationSpeed()
    {
        //Debug.Log($"(flyer.CurrentSpeed({flyer.CurrentSpeed}) - flyer.prMinMoveSpeed({flyer.prMinMoveSpeed}))" +
        //          $" / (flyer.prMaxMoveSpeed({flyer.prMaxMoveSpeed}) - flyer.prMinMoveSpeed({flyer.prMinMoveSpeed})) = {prPercentOfMaxSpeed}");
        if (PercentOfMaxSpeed > 0.001f)
        {
            anim.speed = PercentOfMaxSpeed / 2 + 0.75f;
        }
        else
        {
            anim.speed = 0.75f;
        }
    }

    protected virtual void ModelPoseUpdate()
    {
        //Change body Roll
        transform.localEulerAngles = new Vector3(
            0f,
            90f+45f * percentOfRotSpeed* PercentOfMaxSpeed,
            -90f
            );
    }

}
