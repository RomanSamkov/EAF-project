using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Movement : MonoBehaviour
{
    public float minSpeed = 5f;
    public float moveSpeed = 15f;
    public float angularSpeed = 2f;

    public float xMinBorder;
    public float xMaxBorder;
    public float zMinBorder;
    public float zMaxBorder;

    Rigidbody rb;

    float rotationZ;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if(transform.position.x <= xMinBorder)
        {
            Vector3 temp = transform.position;
            temp.x = xMinBorder;
            transform.position = temp;
        }
        if (transform.position.x >= xMaxBorder)
        {
            Vector3 temp = transform.position;
            temp.x = xMaxBorder;
            transform.position = temp;
        }
        if (transform.position.z <= zMinBorder)
        {
            Vector3 temp = transform.position;
            temp.z = zMinBorder;
            transform.position = temp;
        }
        if (transform.position.z >= zMaxBorder)
        {
            Vector3 temp = transform.position;
            temp.z = zMaxBorder;
            transform.position = temp;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * (minSpeed + moveSpeed * CrossPlatformInputManager.GetAxis("Vertical"));
        rotationZ = CrossPlatformInputManager.GetAxis("Horizontal") * -1;
        transform.Rotate(0, 0, rotationZ * angularSpeed * Time.timeScale);
    }

}
