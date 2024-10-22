using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdraftTrigger : MonoBehaviour
{
    public float force;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {  
            other.GetComponent<PlayerController>().Updraft(force);
        }
    }
}
