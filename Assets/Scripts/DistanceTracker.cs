using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public GameObject playerObject;
    public float distanceFromPlayer;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerObject.GetComponent<PlayerController>().isOffRamp && gameManager.gameState == GameManager.GameState.Gameplay)
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
