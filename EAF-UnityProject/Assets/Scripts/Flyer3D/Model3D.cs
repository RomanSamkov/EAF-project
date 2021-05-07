using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Model3D : MonoBehaviour
{
    //Этот класс должен отвечать за отрисовку моделей и за их кастомизацию

    //public GameObject modelGO;
    protected Animator anim;
    public TestPlayer3D flyer;

    public Transform Neck;

    public Transform LeftHoofRadius;
    public Transform RightHoofRadius;
    
    public Transform BackHoof_Pelvis_L;
    public Transform BackHoof_Pelvis_R;

    public Transform UpperMane1;
    public Transform UpperMane2;
    public Transform BackMane1;
    public Transform BackMane2;

    public Transform Tail1;
    public Transform Tail2;
    public Transform Tail3;
    public Transform Tail4;

    protected float percentOfMaxSpeed;
    protected float percentOfHorRotSpeed;
    protected float percentOfVerRotSpeed;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        flyer = flyer.GetComponent<TestPlayer3D>();
    }

    private void Start()
    {
        
    }

    float animMoveSpeedTime1;
    float animMoveSpeedTime2;
    float animMoveSpeedTime3;
    float animMoveSpeedTime4;

    protected virtual void Update()
    {
        //Set pos
        LeftHoofRadius.localEulerAngles = new Vector3(335.035309f, 9.48493004f, 34.9138718f);
        RightHoofRadius.localEulerAngles = new Vector3(322.307373f, 352.29187f, 341.792572f);

        //Procedural Positions
        percentOfMaxSpeed = flyer.MoveSpeed / flyer.MaxMoveSpeed;
        percentOfHorRotSpeed = flyer.CurrentHorRotSpeed / flyer.MaxRotSpeed;
        percentOfVerRotSpeed = flyer.CurrentVerRotSpeed / flyer.MaxRotSpeed;

        CurrentFlyAnimationSpeed();

        Neck.localEulerAngles = new Vector3(
           25f * CrossPlatformInputManager.GetAxis("Horizontal"),
           25f * CrossPlatformInputManager.GetAxis("Horizontal"),
           -26f - 15f * CrossPlatformInputManager.GetAxis("Vertical") + (-20 * Mathf.Abs(CrossPlatformInputManager.GetAxis("Horizontal")) * percentOfMaxSpeed)
           );

        BackHoof_Pelvis_L.localEulerAngles = new Vector3(
           -16f + 16*(percentOfMaxSpeed-0.8f),
           -25f + 3* percentOfMaxSpeed,
           82f - 8*percentOfMaxSpeed
           );

        BackHoof_Pelvis_R.localEulerAngles = new Vector3(
           -16f + 16 * (percentOfMaxSpeed - 0.8f),
           25f - 3*percentOfMaxSpeed,
           -82f + 8 * percentOfMaxSpeed
           );

        animMoveSpeedTime1 += Time.deltaTime * ((percentOfMaxSpeed + 0.5f) * 2) * 5f;
        animMoveSpeedTime2 += Time.deltaTime * ((percentOfMaxSpeed + 0.5f) * 2) * 7f;
        animMoveSpeedTime3 += Time.deltaTime * ((percentOfMaxSpeed + 0.5f) * 2) * 9f;
        animMoveSpeedTime4 += Time.deltaTime * ((percentOfMaxSpeed + 0.5f) * 2) * 15f;

        //UpperMane
        UpperMane1.localEulerAngles = new Vector3(
           20 * percentOfHorRotSpeed,
           -90f,
           -30f + 20 * Mathf.Sin(animMoveSpeedTime3)
           );

        UpperMane2.localEulerAngles = new Vector3(
           0f,
           0f,
           10 + -12 * Mathf.Sin(animMoveSpeedTime4)
           );

        //BackMane
        BackMane1.localEulerAngles = new Vector3(
           0,
           0,
           10 + -12 * Mathf.Sin(animMoveSpeedTime2) + 20 * -percentOfHorRotSpeed
           );

        BackMane2.localEulerAngles = new Vector3(
           0,
           0,
           20 * Mathf.Sin(animMoveSpeedTime3)
           );

        //Tail
        Tail1.localEulerAngles = new Vector3(
           0f,
           -(percentOfVerRotSpeed*10)+(-10f*percentOfMaxSpeed*Mathf.Abs(percentOfHorRotSpeed)),
           -90f - 3.5f* Mathf.Sin(animMoveSpeedTime1) - percentOfHorRotSpeed * 10f
           );

        Tail2.localEulerAngles = new Vector3(
           Mathf.Sin(animMoveSpeedTime1) * 5,
           0f,
           5f * Mathf.Sin(animMoveSpeedTime2) - percentOfHorRotSpeed * 6f - (5f * Mathf.Sin(animMoveSpeedTime1) - percentOfHorRotSpeed * 10f)
           );

        Tail3.localEulerAngles = new Vector3(
           Mathf.Sin(animMoveSpeedTime2) * 5,
           0f,
           5f * Mathf.Sin(animMoveSpeedTime3)
           );

        Tail4.localEulerAngles = new Vector3(
           0f,
           0f,
           5f * Mathf.Sin(animMoveSpeedTime4)
           );

    }

    protected virtual void CurrentFlyAnimationSpeed()
    {
        //Debug.Log($"(flyer.CurrentSpeed({flyer.CurrentSpeed}) - flyer.prMinMoveSpeed({flyer.prMinMoveSpeed}))" +
        //          $" / (flyer.prMaxMoveSpeed({flyer.prMaxMoveSpeed}) - flyer.prMinMoveSpeed({flyer.prMinMoveSpeed})) = {prPercentOfMaxSpeed}");
        if (percentOfMaxSpeed > 0.001f)
        {
            anim.speed = percentOfMaxSpeed / 2 + 0.75f;
        }
        else
        {
            anim.speed = 0.75f;
        }
    }

    protected virtual void ModelPoseUpdate()
    {
        
    }

}
