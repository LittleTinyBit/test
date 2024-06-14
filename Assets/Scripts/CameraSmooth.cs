using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour
{
    public Transform CameraPosition;
	float TransformSpeed = 0.15f;
    Vector3 Offset = Vector3.zero;
    void FixedUpdate()
    {
        AvoidObstacle();
        if(CameraPosition.position.z>-10)
        {
            Offset = Vector3.zero;
        }
        transform.position = Vector3.Lerp(transform.position, CameraPosition.position-Offset, TransformSpeed);
    }
    void AvoidObstacle()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.back),out hit,1))
        {
            float z = CameraPosition.position.z - hit.point.z;
            Offset.z = z;
        }
        else
        {
            Offset = Vector3.zero;
        }
        
    }
}
