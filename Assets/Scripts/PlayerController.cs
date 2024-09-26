using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Transform spawnPoint;
    public Transform playerTransform;
    private Rigidbody playerBody;
    public bool hasLaunched;
    public bool isOffRamp;
    public int boostValue;
    public float maxRotationX;
    public float maxRotationY;
    public float minRotationX;
    public float minRotationY;

    
    void Awake()
    {
        gameManager = GameManager.gameManager;
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
        if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            if(isOffRamp)
            {
                CheckRotation();
            }
            if(!isOffRamp)
            {
                Quaternion rotation = new Quaternion();
                rotation.Set(playerTransform.rotation.x,0,playerTransform.rotation.z,1);
                playerTransform.rotation = rotation;
            }
            if(hasLaunched)
            {
                //Releases player from spawn position when launch it activated. 
                playerBody.isKinematic = false;
            }
            else
            {
                //holds player in spawn position, until launch it activated. 
                playerBody.isKinematic = true;
            }
        }
        else
        {
            //holds player in spawn position, until launch it activated. 
            playerBody.isKinematic = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnLaunch()
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            if(!hasLaunched)
            {
                hasLaunched = true;
            }
        }
    }
    
    void OnMove(InputValue movementValue)
    {
        Vector2 moveVector = movementValue.Get<Vector2>();
        if(gameManager.gameState == GameManager.GameState.Gameplay && isOffRamp)
        {
            RotatePlayer(moveVector);
        }
    }
    /// <summary>
    /// Launches player at end of ramp based on upgrades
    /// </summary>
    public void LaunchBoost()
    {
        isOffRamp = true;
        playerBody.AddForce(Vector3.forward * boostValue);
    }
    /// <summary>
    /// Should only take in the move vector from OnMove Method, logic for player movement
    /// </summary>
    /// <param name="moveVector"></param>
    public void RotatePlayer(Vector2 moveVector) //
    {
        float step = playerBody.velocity.z * Time.deltaTime;
        if(moveVector.x > 0)
        {
            Quaternion rotation = Quaternion.AngleAxis(30, transform.right);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, step*5);
        }
        if(moveVector.x < 0)
        {
            Quaternion rotation = Quaternion.AngleAxis(-30, transform.right);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, step*5);
        }
        if(moveVector.y > 0)
        {
            Quaternion rotation = Quaternion.AngleAxis(-25, transform.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, step);
        }
        if(moveVector.y < 0)
        {
            Quaternion rotation = Quaternion.AngleAxis(25, transform.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, step);
        }
        else
        {
            Quaternion rotation = new Quaternion();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, step);
        }
    }
    /// <summary>
    /// Keeps player rotation within max/min rotation angle.
    /// </summary>
    void CheckRotation()
    {
        if(playerTransform.rotation.x <= minRotationX)
        {
            Debug.Log("Clamping rotation x =" + playerTransform.rotation.x);
            Quaternion rotation = Quaternion.AngleAxis(-30, transform.right);
            rotation.Set(rotation.x,playerTransform.rotation.y,playerTransform.rotation.z,1);
            playerTransform.rotation = rotation;
        }
        if(playerTransform.rotation.x >= maxRotationX)
        {
            Debug.Log("Clamping rotation x =" + playerTransform.rotation.x );
            Quaternion rotation = Quaternion.AngleAxis(30, transform.right);
            rotation.Set(rotation.x,playerTransform.rotation.y,playerTransform.rotation.z,1);
            playerTransform.rotation = rotation;
        }
        if(playerTransform.rotation.z <= minRotationY)
        {
            Debug.Log("Clamping rotation z =" + playerTransform.rotation.z);
            Quaternion rotation = Quaternion.AngleAxis(-25, transform.forward);
            rotation.Set(playerTransform.rotation.x,playerTransform.rotation.y,rotation.z,1);
            playerTransform.rotation = rotation;
        }
        if(playerTransform.rotation.z >= maxRotationY)
        {
            Debug.Log("Clamping rotation z =" + playerTransform.rotation.z);
            Quaternion rotation = Quaternion.AngleAxis(25, transform.forward);
            rotation.Set(playerTransform.rotation.x,playerTransform.rotation.y,rotation.z,1);
            playerTransform.rotation = rotation;
        }
    }
}
