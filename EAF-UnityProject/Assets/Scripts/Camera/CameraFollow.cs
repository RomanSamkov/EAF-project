using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private bool AutoPlayerSearch;

    public Transform Target;

    public Vector3 OffsetVec;
    [SerializeField]
    private float speedHeightChange;
    [SerializeField]
    private float speedForwardChange;
    public Vector3 RotationVec;
    [SerializeField]
    private float rotationSpeedChange;

    private void Start()
    {
        if(AutoPlayerSearch)Target = GameObject.Find("Player").transform;
    }
    
    private void Update()
    {
        //Take and set rotation froma target
        transform.rotation = Target.rotation;

        Vector3 temporary_rotation = RotationVec;
        temporary_rotation.z = temporary_rotation.y + rotationSpeedChange * CrossPlatformInputManager.GetAxis("Horizontal");
        //temporary_rotation.y = temporary_rotation.y + rotationSpeedChange * CrossPlatformInputManager.GetAxis("Horizontal");
        transform.Rotate(temporary_rotation);

        Vector3 desiredPosition = Target.position
            + Target.up * OffsetVec.x
            + Target.up * speedForwardChange * CrossPlatformInputManager.GetAxis("Vertical")
            + Target.forward * OffsetVec.y*-1
            + Target.forward * speedHeightChange * CrossPlatformInputManager.GetAxis("Vertical")*-1
            + Target.right * rotationSpeedChange * CrossPlatformInputManager.GetAxis("Horizontal");

        transform.position = desiredPosition;
    }
}
