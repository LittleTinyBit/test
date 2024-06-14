using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    public Vector3 Position;
    public bool isPlayer = false;
    void Start()
    {
        Position = this.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            isPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayer = false;
        }
    }
}
