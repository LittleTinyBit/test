using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interacrtable : MonoBehaviour
{
    public Image img;
    public Material mat;
    public bool isInteractable = true;
    Color DefaultColor= new Color(0.7f, 0.7f, 0.7f);
    private void Start()
    {
        MakeInteractable(true);
    }
    void MakeInteractable(bool No=false)
    {
        if(No)
        {
            if (img == null)
            {
                mat.color = DefaultColor;
                mat.SetColor("_EmissionColor", Color.clear);
            }
            else
            {
                img.color = DefaultColor;
            }   
        }
        else
        {
            if (img == null)
            {
                mat.color = Color.white;
                mat.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
            }
            else
            {
                img.color = Color.white;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(isInteractable&&other.tag=="Player"&&!other.GetComponent<PlayerCtrl>().interact)
        {
            MakeInteractable();
        }
        else if(other.tag=="Player")
        {
            MakeInteractable(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            MakeInteractable(true);
        }
    }
}
