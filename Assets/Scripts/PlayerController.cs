using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handels all player movement and holds active stats for gameplay
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Object Referances")]
    public GameManager gameManager;
    [SerializeField]
    private Camera mainCamera;
    public Transform spawnPoint;
    [Header("Compnent Referances")]
    [SerializeField]
    private ConstantForce playerForce;
    public Transform playerTransform;
    public Rigidbody playerBody;
    public InputActionAsset playerInputs;
    public InputAction boostAction;
    public GameObject explosion;
    [Header("Background Stats")]
    public bool hasLaunched;
    public bool hasLanded;
    public bool isOffRamp;
    public float explodeTime;
    [Header("Player Stats")]
    public Stats playerStats;
    [Header("Animation")]
    public Animator propellorAnim;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerBody = this.gameObject.GetComponent<Rigidbody>();
        playerTransform = this.transform;
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        playerForce = this.GetComponent<ConstantForce>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Sets player at spawn position. 
        spawnPoint = GameObject.FindWithTag("Start").transform;
        //playerTransform.position = spawnPoint.position;
        //Makes sure player has not launched. 
        ResetForNewRun();
        boostAction = playerInputs.FindAction("Boost", false);
        if(playerStats.boostSpeed < 2)
        {
            playerStats.boostSpeed = 2;
        }
        if(playerStats.fuel < 5)
        {
            playerStats.fuel = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            CheckForRunEnd();
        }
        if(gameManager.gameState == GameManager.GameState.Gameplay && !hasLanded)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            HoldYRotation();
            if(hasLaunched)
            {
                //Releases player from spawn position when launch it activated. 
                playerBody.isKinematic = false;
            }
            else
            {
                //holds player in spawn position, until launch it activated. 
                playerBody.isKinematic = true;
                playerTransform.rotation = spawnPoint.rotation;
            }
            if(hasLaunched && isOffRamp)
            {
                if(boostAction.IsPressed())
                {
                    Boost();
                }
                else
                {
                    playerForce.relativeForce = new Vector3(0,0,0);
                }
            }
            //Debug.Log(playerBody.velocity);
        }
        else if(hasLanded)
        {
            Cursor.lockState = CursorLockMode.None;
            playerForce.force = new Vector3(0,0,0);
            playerForce.relativeForce = new Vector3(0,0,0);
        }
        else
        {
            //holds player in spawn position, until launch it activated. 
            playerBody.isKinematic = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if(playerBody.velocity.x > 0 )
        {
            propellorAnim.SetBool("isMoving", true);
            propellorAnim.speed = playerBody.velocity.x/2;
        }
        else
        {
            propellorAnim.SetBool("isMoving", false);
            propellorAnim.speed = 1;
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
        if(gameManager.gameState == GameManager.GameState.Gameplay && isOffRamp && !hasLanded)
        {
            Vector2 moveVector2 = movementValue.Get<Vector2>();
            //Aims player towards mouse movement 
            transform.Rotate(moveVector2.x*-playerStats.lookSensitivity/4,moveVector2.x*playerStats.lookSensitivity/2,moveVector2.y*playerStats.lookSensitivity);
        }
    }

    void Boost()
    {
        if(playerStats.fuel > 0)
        {
            //Debug.Log(playerStats.fuel);
            playerStats.fuel -= Time.deltaTime;
            playerForce.relativeForce = new Vector3(playerStats.boostSpeed,playerBody.mass*3,0);
        }
    }
    /// <summary>
    /// Launches player at end of ramp based on upgrades
    /// </summary>
    public void LaunchBoost()
    {
        isOffRamp = true;
        playerBody.AddForce(Vector3.right * playerStats.startBoost);
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;
        }
    }

    /// <summary>
    /// Checks to see if player has landed and has stoped moveing. 
    /// </summary>
    void CheckForRunEnd()
    {
        if(hasLanded)
        {
            gameManager.gameState = GameManager.GameState.Results;
            gameManager.ChangeGameState();
        }
    }

    /// <summary>
    /// Keeps player on track for launch
    /// </summary>
    void HoldYRotation()
    {
            if(!isOffRamp)
            {
                RigidbodyConstraints constraints = RigidbodyConstraints.FreezeRotationY;
                playerBody.constraints = constraints;
            }
            else
            {
                RigidbodyConstraints constraints = RigidbodyConstraints.FreezeRotationX;
                playerBody.constraints = constraints;
            }
    }

    public void ResetForNewRun()
    {
        hasLanded = false;
        hasLaunched = false;
        isOffRamp = false;
        playerStats.fuel = playerStats.maxFuel;

    }

    public float GetAltitude()
    {
        float altitude = 0;
        return altitude;
    }

    public void Explode()
    {
        GameObject playerExplosion = Instantiate(explosion,new Vector3(playerTransform.position.x+5,playerTransform.position.y,playerTransform.position.z),playerTransform.rotation);
        gameManager.playerCam.transform.LookAt(playerExplosion.transform);
        Destroy(playerExplosion,1);
    }

    public void PlaceBestRunMarker()
    {
        if(gameManager.runDistance > playerStats.bestDistance)
        {
            playerStats.bestRunPositon = this.transform.position;
        }
    }

    public float ReturnDistance()
    {
        float distance = Vector3.Distance(GameObject.FindWithTag("Marker").transform.position,playerTransform.position);
        return distance;
    }
}
