using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloud : MonoBehaviour
{
    public float speed = 3;

    public float bound_X = 125f;
    public float spawn_X = 125f;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 temp = transform.position;
        temp.x += speed * Time.deltaTime;
        transform.position = temp;
        if (temp.x > bound_X)
        {
            temp.x = spawn_X;
            transform.position = temp;
        }
    }
}
