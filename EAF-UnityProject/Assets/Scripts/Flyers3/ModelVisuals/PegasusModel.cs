using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegasusModel : Model
{
    [Header("Pegasus Model Settings")]

    public Color BodyColor;
    public Color ManeMainColor;
    public Color ManeSecondaryColor;
    public Color ClothMainColor;
    public Color ClothSecondaryColor;

    public SpriteRenderer MainBodyRenderer;
    public SpriteRenderer HeadRenderer;
    public SpriteRenderer WingRightRenderer;
    public SpriteRenderer WingLeftRenderer;

    public SpriteRenderer ManeMainRenderer;
    public SpriteRenderer ManeSecondaryRenderer;
    public SpriteRenderer TailMainRenderer;
    public SpriteRenderer TailSecondaryRenderer;


    public SpriteRenderer ClothMainRenderer;
    public SpriteRenderer ClothSecondaryRenderer;

    protected override void Awake()
    {
        base.Awake();
        MainBodyRenderer.color = BodyColor;
        HeadRenderer.color = BodyColor;
        WingRightRenderer.color = BodyColor;
        WingLeftRenderer.color = BodyColor;

        ManeMainRenderer.color = ManeMainColor;
        ManeSecondaryRenderer.color = ManeSecondaryColor;
        TailMainRenderer.color = ManeMainColor;
        TailSecondaryRenderer.color = ManeSecondaryColor;
    }

    

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        ModelPoseUpdate();
    }

    public Transform FullBody;
    public Transform Head;
    public Transform Tail;
    public Transform Mane;
    
    private float maneHorSinNum = 1.5f;
    private float maneVerSinNum = 1f;

    private float tailHorSinNum = 0f;
    private float tailVerSinNum = 1.1f;
    
    protected virtual void ModelPoseUpdate()
    {
        //Change body Roll
        FullBody.localEulerAngles = new Vector3(
            0f,
            10f * percentOfRotSpeed * -1,
            0f
            );

        //Change head rotation
        Head.localEulerAngles = new Vector3(
            0f,
            0f,
            30f * percentOfRotSpeed * -1
            );

        //Mane swinging and rotation
        Mane.localEulerAngles = new Vector3(
            Mathf.Sin(maneVerSinNum) * 5f,
            0f,
            15f * percentOfRotSpeed + Mathf.Sin(maneHorSinNum) * 5f
            );

        maneVerSinNum += Time.deltaTime * (20 + 10 * (prPercentOfMaxSpeed-0.5f));
        maneHorSinNum += Time.deltaTime * (20 + 15 * (prPercentOfMaxSpeed-0.5f));

        //Tail swinging and rotation
        Tail.localEulerAngles = new Vector3(
            Mathf.Sin(tailVerSinNum) * 10f - (1 - prPercentOfMaxSpeed) * 30f,
            0f,
            15f * percentOfRotSpeed + Mathf.Sin(tailHorSinNum) * 5f
            );

        tailHorSinNum += Time.deltaTime * (10 + 7 * (prPercentOfMaxSpeed - 0.5f));
        tailVerSinNum += Time.deltaTime * (9.5f + 7 * (prPercentOfMaxSpeed - 0.5f));
    }
    
}
