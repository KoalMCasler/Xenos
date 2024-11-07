using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Video;

/// <summary>
/// Handels all player movement and holds active stats for gameplay
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Object Referances")]
    public GameManager gameManager;
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private UpgradeManager upgradeManager;
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
    public Transform boosterPosition;
    [Header("Background Stats")]
    public bool hasLaunched;
    public bool hasLanded;
    public bool isOffRamp;
    public float explodeTime;
    public bool hitWater;
    public float altitude;
    private int maxRayDistance = 2000;
    public int altitudeLimit;
    [Range(1,5)] //in seconds
    public float altitudeTimer; 
    public float downForce = 1;
    public bool canBounce;
    public bool willWarp;
    public int fuelGain;
    public float eficency;
    public float warpEffectTime;
    public GameObject warpEffect;
    public float startBoostTime;
    public bool boostIsDone;
    public float endTime;
    public bool hitWall;
    public bool isMoveing;
    [Header("Player Stats")]
    public Stats playerStats;
    [Header("Animation")]
    public Animator propellorAnim;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        upgradeManager = gameManager.upgradeManager;
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
        //spawnPoint = GameObject.FindWithTag("Start").transform;
        playerTransform.position = spawnPoint.position;
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
            if(!isOffRamp)
            {
                HoldYRotation();
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
                if(isMoveing == false)
                {
                    BalanceCraft();
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
        else if(playerBody.velocity.magnitude > 0 )
        {
            propellorAnim.SetBool("isMoving", true);
            propellorAnim.speed = playerBody.velocity.magnitude;
        }
        else
        {
            propellorAnim.SetBool("isMoving", false);
            propellorAnim.speed = 1;
        }
        LimitAltitude(GetAltitude());
    }
    /// <summary>
    /// Lunch input feedback
    /// </summary>
    void OnLaunch()
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            if(!hasLaunched)
            {
                hasLaunched = true;
                soundManager.PlaySFX(3);//See list in editor for index. 
                CheckForSpecialEquip();
            }
        }
    }
    /// <summary>
    /// Move input reader
    /// </summary>
    /// <param name="movementValue"></param>
    void OnMove(InputValue movementValue)
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay && isOffRamp && !hasLanded)
        {
            Vector2 moveVector2 = movementValue.Get<Vector2>();
            if(moveVector2 != Vector2.zero)
            {
                isMoveing = true;
                moveVector2.Normalize();
                //Aims player towards mouse movement 
                transform.Rotate(moveVector2.x*-playerStats.lookSensitivity/3,moveVector2.x*playerStats.lookSensitivity/1.5f,moveVector2.y*playerStats.lookSensitivity);
            }
            else
            {
                isMoveing = false;
            }
        }
    }
    /// <summary>
    /// Boosts player when they use engine.
    /// </summary>
    void Boost()
    {
        //Debug.Log(playerStats.fuel);
        playerStats.fuel -= Time.deltaTime/eficency;
        playerForce.relativeForce = new Vector3(playerStats.boostSpeed,playerStats.boostSpeed/2,0);
    }
    /// <summary>
    /// Launches player at end of ramp based on upgrades
    /// </summary>
    public void LaunchBoost()
    {
        Debug.Log("Start Boost triggered");
        soundManager.PlayContinuesSFX(2); //index 2 is engine sound in list of SFX
        RollForWoo();
        isOffRamp = true;
        if(upgradeManager.boostSlot.currentEquipment != null)
        {
            if(!willWarp)
            {
                soundManager.PlaySFX(8);
                playerBody.AddExplosionForce(playerStats.startBoost*5,boosterPosition.position,5f);
            }
            else
            {
                StartCoroutine(Warp());
            }
        }
        boostIsDone = true;
    }

    void RollForWoo()
    {
        float roll;
        roll = UnityEngine.Random.Range(1, 5);
        Debug.Log("Woo Roll =" + roll);
        if(roll <= 2)
        {
            soundManager.PlaySFX(7);
        }
    }

    public IEnumerator Warp()
    {
        soundManager.PlaySFX(10);
        warpEffect.SetActive(true);
        yield return new WaitForSeconds(warpEffectTime);
        soundManager.PlaySFX(11);
        Transform warpPoint = GameObject.FindWithTag("Warp").transform;
        playerBody.transform.position = warpPoint.position;
    }
    /// <summary>
    /// Collison event
    /// </summary>
    /// <param name="other"></param>
    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Ground") && !canBounce)
        {
            hasLanded = true;
        }
        else if(other.gameObject.CompareTag("Ground") && canBounce)
        {
            soundManager.PlaySFX(9);
            canBounce = false;
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
        if(other.gameObject.CompareTag("Wall"))
        {
           hasLanded = true;
           hitWall = true;
        }
    }
    /// <summary>
    /// Checks to see if player has landed and has stoped moveing. 
    /// </summary>
    void CheckForRunEnd()
    {
        if(hasLanded && hitWall)
        {
            gameManager.gameState = GameManager.GameState.GameEnd;
            gameManager.ChangeGameState();
        }
        else if(hasLanded)
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
    /// <summary>
    /// Resets background stats for new run.
    /// </summary>
    public void ResetForNewRun()
    {
        hasLanded = false;
        hasLaunched = false;
        isOffRamp = false;
        hitWater = false;
        canBounce = false;
        warpEffect.SetActive(false);
        boostIsDone = false;
        hitWall = false;
        playerStats.fuel = playerStats.maxFuel;

    }
    /// <summary>
    /// Gets current Altitude
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Spawns explosion effect
    /// </summary>
    public void Explode()
    {
        Quaternion explodeRotation = new Quaternion();
        explodeRotation.Set(0,0,0,1);
        GameObject playerExplosion = Instantiate(explosion,new Vector3(playerTransform.position.x+5,playerTransform.position.y,playerTransform.position.z),explodeRotation);
        soundManager.PlaySFX(0); //first in sfx list is always explosion
        gameManager.playerCam.transform.LookAt(playerExplosion.transform);
        Destroy(playerExplosion,2);
    }
    /// <summary>
    /// Spawn spalsh down effect.
    /// </summary>
    public void Splash()
    {
        Quaternion explodeRotation = new Quaternion();
        explodeRotation.Set(0,0,0,1);
        GameObject playerExplosion = Instantiate(splashDown,new Vector3(playerTransform.position.x+5,playerTransform.position.y,playerTransform.position.z),explodeRotation);
        soundManager.PlaySFX(1); //2nd in sfx list is always water explosion
        gameManager.playerCam.transform.LookAt(playerExplosion.transform);
        Destroy(playerExplosion,2);
    }
    /// <summary>
    /// Adds increaseing force to the player as they are over altitude limit. 
    /// </summary>
    /// <param name="altitude"></param>
    private void LimitAltitude(float altitude)
    {
        if(altitude > altitudeLimit)
        {
            altitudeTimer -= Time.deltaTime;
            if(altitudeTimer <= 0)
            {
                altitudeTimer = 1;
                downForce += 1;
            }
            playerBody.AddForce(Vector3.down*downForce);
        }
        else
        {
            downForce = 1;
        }
    }
    /// <summary>
    /// If best run, moves in world marker to landing position
    /// </summary>
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
        speed = (playerBody.velocity.magnitude * 3600)/1000; //Converts m/s to km/h 
        speedKn = speed * 0.539957f; // converts km/h to knots.
        return speedKn;
    }
    /// <summary>
    /// Sets player to spawn position
    /// </summary>
    public void SetPlayerToSpawn()
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    public void Updraft(float force)
    {
        Debug.Log("Updraft Triggered");
        playerBody.AddForce(transform.up*force, ForceMode.Force);
    }

    void CheckForSpecialEquip()
    {
        if(upgradeManager.matSlot.currentEquipment != null)
        {
            if(upgradeManager.matSlot.currentEquipment.isSpecial)
            {
                canBounce = true;
            }
            else
            {
                canBounce = false;
            }
        }
        if(upgradeManager.engSlot.currentEquipment != null)
        {
            if(upgradeManager.engSlot.currentEquipment.isSpecial)
            {
                eficency = 1.25f;
            }
            else
            {
                eficency = 1;
            }
        }
        if(upgradeManager.fuelSlot.currentEquipment != null)
        {
            if(upgradeManager.fuelSlot.currentEquipment.isSpecial)
            {
                fuelGain = 2;
            }
            else
            {
                fuelGain = 1;
            }
        }
        if(upgradeManager.boostSlot.currentEquipment != null)
        {
            if(upgradeManager.boostSlot.currentEquipment.isSpecial)
            {
                willWarp = true;
            }
            else
            {
                willWarp = false;
            }
        }
    }

    void BalanceCraft()
    {
        if(transform.rotation.x >= 0.05f)
        {
            playerForce.relativeTorque =  new Vector3(-.05f,0,0);
        }
        else if(transform.rotation.x <= -0.05f)
        {
            playerForce.relativeTorque =  new Vector3(.05f,0,0);
        }
        else
        {
            playerForce.relativeTorque =  new Vector3(0,0,0);
        }
    }
}
