using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            this.GetComponentInParent<Stranger> ().AvoidPlayer(other.transform.position);
        }
    }
}
