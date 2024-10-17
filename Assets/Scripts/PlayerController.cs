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
    private SoundManager soundManager;
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
    public GameObject splashDown;
    [Header("Background Stats")]
    public bool hasLaunched;
    public bool hasLanded;
    public bool isOffRamp;
    public float explodeTime;
    public bool hitWater;
    public float altitude;
    private int maxRayDistance = 1000;
    [Header("Player Stats")]
    public Stats playerStats;
    [Header("Animation")]
    public Animator propellorAnim;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        soundManager = gameManager.soundManager;
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
        //Debug.Log("player speed = " + playerBody.velocity.x);
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
                if(boostAction.IsPressed() && playerStats.fuel > 0)
                {
                    soundManager.contSFXSource.volume = 0.75f;
                    Boost();
                }
                else
                {
                    soundManager.contSFXSource.volume = 0.25f;
                    playerForce.relativeForce = new Vector3(0,0,0);
                }
                GetAltitude();
            }
            //Debug.Log(playerBody.velocity);
        }
        else if(hasLanded)
        {
            Cursor.lockState = CursorLockMode.None;
            playerForce.force = new Vector3(0,0,0);
            playerForce.relativeForce = new Vector3(0,0,0);
            soundManager.contSFXSource.Stop();
        }
        else
        {
            //holds player in spawn position, until launch it activated. 
            playerBody.isKinematic = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(gameManager.gameState == GameManager.GameState.MainMenu)
        {
            propellorAnim.SetBool("isMoving", true);
            propellorAnim.speed = 0.5f;
        }
        else if(playerBody.velocity.x > 0 )
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
                soundManager.PlaySFX(3);//See list in editor for index. 
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
        //Debug.Log(playerStats.fuel);
        playerStats.fuel -= Time.deltaTime;
        playerForce.relativeForce = new Vector3(playerStats.boostSpeed,playerBody.mass*3,0);
    }
    /// <summary>
    /// Launches player at end of ramp based on upgrades
    /// </summary>
    public void LaunchBoost()
    {
        soundManager.PlayContinuesSFX(2); //index 2 is engine sound in lest of SFX
        isOffRamp = true;
        playerBody.AddForce(Vector3.right * playerStats.startBoost);
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;
        }
        if(other.gameObject.CompareTag("Lake"))
        {
            if(playerBody.velocity.x < 100*playerBody.mass) 
            {
                hitWater = true;
                hasLanded = true;
            }
            else
            {
                soundManager.PlaySFX(6); //See list for index
            }
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
        hitWater = false;
        playerStats.fuel = playerStats.maxFuel;

    }

    public float GetAltitude()
    {
        Debug.DrawLine(playerTransform.position,Vector3.down * maxRayDistance);
        if(Physics.Raycast(playerTransform.position,Vector3.down, out RaycastHit hit, maxRayDistance))
        {
            if(hit.collider.gameObject.CompareTag("Ground") || hit.collider.gameObject.CompareTag("Lake"))
            {
                altitude = hit.distance;
            }
        }

        return altitude;
    }

    public void Explode()
    {
        Quaternion explodeRotation = new Quaternion();
        explodeRotation.Set(0,0,0,1);
        GameObject playerExplosion = Instantiate(explosion,new Vector3(playerTransform.position.x+5,playerTransform.position.y,playerTransform.position.z),explodeRotation);
        soundManager.PlaySFX(0); //first in sfx list is always explosion
        gameManager.playerCam.transform.LookAt(playerExplosion.transform);
        Destroy(playerExplosion,2);
    }

    public void Splash()
    {
        Quaternion explodeRotation = new Quaternion();
        explodeRotation.Set(0,0,0,1);
        GameObject playerExplosion = Instantiate(splashDown,new Vector3(playerTransform.position.x+5,playerTransform.position.y,playerTransform.position.z),explodeRotation);
        soundManager.PlaySFX(1); //2nd in sfx list is always water explosion
        gameManager.playerCam.transform.LookAt(playerExplosion.transform);
        Destroy(playerExplosion,2);
    }

    public void PlaceBestRunMarker()
    {
        if(gameManager.runDistance > playerStats.bestDistance)
        {
            playerStats.bestRunPositon = this.transform.position;
        }
    }
    /// <summary>
    /// Returns the distance from the ramp for each run. 
    /// </summary>
    /// <returns></returns>
    public float ReturnDistance()
    {
        float distance = Vector3.Distance(GameObject.FindWithTag("Marker").transform.position,playerTransform.position);
        return distance;
    }
    /// <summary>
    /// Returns speed, converted from m/s to knots. 
    /// </summary>
    /// <returns></returns>
    public float GetSpeed()
    {
        float speed = 0;
        float speedKn;
        speed = (playerBody.velocity.x * 3600)/1000; //Converts m/s to km/h 
        speedKn = speed * 0.539957f; // converts km/h to knots.
        return speedKn;
    }
}
