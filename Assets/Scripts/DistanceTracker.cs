using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public GameObject playerObject;
    public float distanceFromPlayer;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerObject.GetComponent<PlayerController>().isOffRamp)
        {
            distanceFromPlayer = Vector3.Distance(this.transform.position,playerObject.transform.position);
        }
        returnDistance();
    }

    public float returnDistance()
    {
        //Debug.Log("Player is " + distanceFromPlayer + "m Away from the ramp");
        return distanceFromPlayer;
    }


}
