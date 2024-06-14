using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyesContact : MonoBehaviour
{
    public Transform Eyes;
    public Image Closed;
    public Interacrtable Store;
    Vector3 StartPos;
    GameChanger GC;
    private void Start()
    {
        GC = Camera.main.GetComponent<GameChanger>();//on main camera
        StartPos = Eyes.transform.localPosition;
    }
    private void FixedUpdate()
    {
        if(GC.isDay)
        {
            Closed.enabled = false;
            Store.isInteractable = true;
            Store.tag = "Store";
        }
        else
        {
            Closed.enabled = true;
            Store.isInteractable = false;
            Store.tag = "Untagged";
        }
    }
    //Shrink vector to one lenght
    Vector3 BringUpToShort(Vector3 vecStart, Vector3 vecFinish)
    {
        //Pythagorean theorem
        float k = Mathf.Sqrt(Mathf.Pow(vecFinish.x - vecStart.x, 2) + Mathf.Pow(vecFinish.z - vecStart.z, 2));
        return (vecFinish - vecStart) / k;
    }   
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player"|| other.tag == "Stranger")
        {
            Vector3 vec = BringUpToShort(Eyes.position, other.transform.position);
            //If dot product positive that mean someone in front
            float DotProd = Vector3.Dot(Vector3.back, vec);//seller looks to -z
            if(DotProd>0)
            {
                Debug.DrawLine(Eyes.position, Eyes.position + vec, Color.red);
                float x = other.transform.position.x - Eyes.position.x;
                vec = new Vector3(Mathf.Clamp(StartPos.x + x, StartPos.x - 10f, StartPos.x + 10f), StartPos.y, StartPos.z);
                Eyes.localPosition =vec;
            }
        }
    }
}
