using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handels all player movement and holds active stats for gameplay
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Object Referances")]
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Camera mainCamera;
    private Transform spawnPoint;
    [Header("Compnent Referances")]
    [SerializeField]
    private ConstantForce playerForce;
    public Transform playerTransform;
    private Rigidbody playerBody;
    public InputActionAsset playerInputs;
    public InputAction boostAction;
    [Header("Background Stats")]
    public bool hasLaunched;
    public bool hasLanded;
    public bool isOffRamp;
    [Header("Player Stats")]
    public Stats playerStats;
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
        playerTransform.position = spawnPoint.position;
        //Makes sure player has not launched. 
        ResetPlayerBools();
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
            if(!isOffRamp)
            {
                HoldYRotation();
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
            transform.Rotate(moveVector2.x*-playerStats.lookSensitivity,0,moveVector2.y*playerStats.lookSensitivity);
            //Used to let the player move left/right
            if(moveVector2.x > 0)
            {
                playerForce.force = new Vector3(0,0,-1);
            }
            if(moveVector2.x < 0)
            {
                playerForce.force = new Vector3(0,0,1);
            }
            if(moveVector2.y > 0)
            {
                playerForce.force = new Vector3(0,1,0);
            }
            if(moveVector2.y < 0)
            {
                playerForce.force = new Vector3(0,-1,0);
            }
        }
    }

    void Boost()
    {
        if(playerStats.fuel > 0)
        {
            Debug.Log(playerStats.fuel);
            playerStats.fuel -= Time.deltaTime;
            playerForce.relativeForce = new Vector3(playerStats.boostSpeed,0,0);
        }
    }
    /// <summary>
    /// Launches player at end of ramp based on upgrades
    /// </summary>
    public void LaunchBoost()
    {
        isOffRamp = true;
        playerBody.AddForce(Vector3.right * playerStats.boostValue);
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
        if(hasLanded && playerBody.velocity == Vector3.zero)
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
        transform.rotation.Set(transform.rotation.x,0,transform.rotation.z,1);
    }

    public void ResetPlayerBools()
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
}
