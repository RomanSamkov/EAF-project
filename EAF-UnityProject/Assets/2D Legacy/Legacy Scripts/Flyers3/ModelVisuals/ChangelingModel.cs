using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangelingModel : Model
{
    [Header("Changeling Model Settings")]

    public Color BodyColor;
    public Color ManeMainColor;
    public Color ManeSecondaryColor;
    public Color HornColor;
    public Color WingsColor;
    public Color ClothColor;

    public SpriteRenderer BodyRenderer;
    public SpriteRenderer HeadRenderer;
    public SpriteRenderer HornRenderer;
    public SpriteRenderer WingRightRenderer;
    public SpriteRenderer WingRight2Renderer;
    public SpriteRenderer WingLeftRenderer;
    public SpriteRenderer WingLeft2Renderer;

    public SpriteRenderer ManeRenderer;
    public SpriteRenderer TailMainRenderer;
    public SpriteRenderer TailSecondaryRenderer;

    public SpriteRenderer ClothRenderer; 

    protected override void Awake() 
    {
        base.Awake();
        BodyRenderer.color = BodyColor;
        HeadRenderer.color = BodyColor;

        HornRenderer.color = HornColor;

        WingRightRenderer.color = WingsColor;
        WingLeftRenderer.color = WingsColor;

        ManeRenderer.color = ManeMainColor;
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
