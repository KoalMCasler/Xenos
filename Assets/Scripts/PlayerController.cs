using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Transform spawnPoint;
    public Transform playerTransform;
    private Rigidbody playerBody;
    public bool hasLaunched;
    public bool isOffRamp;
    public int boostValue;
    
    void Awake()
    {
        playerBody = this.gameObject.GetComponent<Rigidbody>();
        playerTransform = this.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Sets player at spawn position. 
        spawnPoint = GameObject.FindWithTag("Start").transform;
        playerTransform.position = spawnPoint.position;
        //Makes sure player has not launched. 
        hasLaunched = false;
        isOffRamp = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRotation();
        if(hasLaunched)
        {

        }
        else
        {
            //holds player in spawn position, until launch it activated. 
            playerTransform.position = spawnPoint.position;
            playerTransform.rotation = spawnPoint.rotation;
        }
    }

    void OnLaunch()
    {
        if(!hasLaunched)
        {
            hasLaunched = true;
        }
    }

    public void LaunchBoost()
    {
        /// <summary>
        /// Will add force to player based on upgrades the player has aquiered. 
        /// </summary>
        isOffRamp = true;
        playerBody.AddForce(Vector3.forward * boostValue);
    }

    void CheckRotation()
    {
        if(playerTransform.rotation.x < -30)
        {
            Debug.Log("Clamping View");
            Quaternion rotation = Quaternion.AngleAxis(-30, transform.right);
            playerTransform.rotation = rotation;
        }
        if(playerTransform.rotation.x > 30)
        {
            Debug.Log("Clamping View");
            Quaternion rotation = Quaternion.AngleAxis(30, transform.right);
            playerTransform.rotation = rotation;
        }
        if(playerTransform.rotation.z < -25)
        {
            Debug.Log("Clamping View");
            Quaternion rotation = Quaternion.AngleAxis(-25, transform.forward);
            playerTransform.rotation = rotation;
        }
        if(playerTransform.rotation.z > 25)
        {
            Debug.Log("Clamping View");
            Quaternion rotation = Quaternion.AngleAxis(25, transform.forward);
            playerTransform.rotation = rotation;
        }

    }
}
