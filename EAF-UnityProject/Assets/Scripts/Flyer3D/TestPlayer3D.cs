using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class TestPlayer3D : MonoBehaviour
{
    #region InspectorVariables
    //[SerializeField] protected Animator anim;

    [Header("Movement")]
    public float AccelerationSpeed;
    public float MoveSpeed;
    public float MinMoveSpeed;
    public float MaxMoveSpeed;
    public float RotateSpeed;

    public Transform Body;

    [Header("Border")]
    public Vector3 CentralMapPoint = new Vector3(0, 30, 0);

    [Header("Health parameters")]
    public int Health;
    public bool Alive = true;
    public string Race;

    //[Header("Detection")]
    //public float DetectDistance;
    //public GameObject Arrow;
    #endregion

    #region HidenVariales
    [HideInInspector] public float CurrentSpeed;
    [HideInInspector] public float CurrentGoalSpeed;
    [HideInInspector] public float CurrentRotSpeed;
    [HideInInspector] public float ChangeSpeed;
    
    [HideInInspector] public float VerticalAngle = 0f;

    protected bool needFlyToCenter;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        flyForward();
        rotateHorizontally();
        moveVertical();
    }

    void flyForward()
    {
        //Move Horizontal
        transform.Translate(MoveSpeed* Mathf.Cos(VerticalAngle * 0.0174533f) * Vector3.up * Time.deltaTime * Time.timeScale);

        Debug.Log($"Cos{Mathf.Cos(VerticalAngle * 0.0174533f)}");

        //Move Vertical
        transform.Translate(MoveSpeed * Mathf.Sin(VerticalAngle * 0.0174533f) * Vector3.forward * Time.deltaTime * Time.timeScale);

        Debug.Log($"Sin{Mathf.Sin(VerticalAngle * 0.0174533f)}");
    }

    void rotateHorizontally()
    {
        transform.Rotate(RotateSpeed * -CrossPlatformInputManager.GetAxis("Horizontal") * Vector3.forward * Time.deltaTime * Time.timeScale);
    }

    void moveVertical()
    {
        VerticalAngle -= RotateSpeed * CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * Time.timeScale;

        if (VerticalAngle < -90) VerticalAngle = -90;
        if (VerticalAngle > 90) VerticalAngle = 90;

        Body.localEulerAngles = new Vector3(
            VerticalAngle,
            0f,
            0f
            );

    }
}
