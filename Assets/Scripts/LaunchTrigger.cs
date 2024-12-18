using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Used to launch player at end of ramp
/// </summary>
public class LaunchTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        //Used to set off starting boost based on aqiered upgrades
        if(other.tag == "Player")
        {
            //Debug.Log(other.gameObject.name + " has been boosted");
            other.GetComponent<PlayerController>().LaunchBoost();
        }
    }
}
