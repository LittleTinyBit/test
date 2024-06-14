using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFad : MonoBehaviour
{
    public void EndLyraAnim()
    {
        GameObject.Find("Canvas").GetComponent<Animator>().SetBool("isSad", false);
    }
    public void EndBulkAnim()
    {
        GameObject.Find("Canvas").GetComponent<Animator>().SetBool("isYell", false);
    }
}
