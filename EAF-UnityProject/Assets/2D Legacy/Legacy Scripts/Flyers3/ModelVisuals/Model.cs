using UnityEditor;
using UnityEngine;

public class Model : MonoBehaviour
{
    //Этот класс должен отвечать за отрисовку моделей и за их кастомизацию

    public GameObject modelGO;
    protected Animator anim;
    protected FlyerG3 flyer;

    protected float prPercentOfMaxSpeed;
    protected float percentOfRotSpeed;

    protected virtual void Awake()
    {
        anim = modelGO.GetComponent<Animator>();
        flyer = GetComponent<FlyerG3>();
    }

    protected virtual void Update()
    {
        CurrentFlyAnimationSpeed();
        prPercentOfMaxSpeed = (flyer.CurrentSpeed - flyer.prMinMoveSpeed) / (flyer.prMaxMoveSpeed - flyer.prMinMoveSpeed);
        percentOfRotSpeed = (flyer.CurrentRotSpeed * flyer.RotationAmount.y) / flyer.MaxRotateSpeed;
    }

    protected virtual void CurrentFlyAnimationSpeed()
    {
        //Debug.Log($"(flyer.CurrentSpeed({flyer.CurrentSpeed}) - flyer.prMinMoveSpeed({flyer.prMinMoveSpeed}))" +
        //          $" / (flyer.prMaxMoveSpeed({flyer.prMaxMoveSpeed}) - flyer.prMinMoveSpeed({flyer.prMinMoveSpeed})) = {prPercentOfMaxSpeed}");
        if (prPercentOfMaxSpeed > 0.001f)
        {
            anim.speed = prPercentOfMaxSpeed / 2 + 0.75f;
        }
        else
        {
            anim.speed = 0.75f;
        }
    }


}
