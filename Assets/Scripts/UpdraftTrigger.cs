using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdraftTrigger : MonoBehaviour
{
    public float thrustBoost;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Updraft Triggered");
            other.GetComponent<PlayerController>().playerBody.AddForce(other.gameObject.transform.up*thrustBoost, ForceMode.Force);
        }
    }
}
