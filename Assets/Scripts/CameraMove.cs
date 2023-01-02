using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(0, speed, 0);
    }
}
