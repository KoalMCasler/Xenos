using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handels basic gameplay and state control
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Object Referances")]
    [SerializeField]
    public UIManager uIManager;
    [SerializeField]
    public UpgradeManager upgradeManager;
    [SerializeField]
    public SoundManager soundManager;
    public PlayerController player;
    public GameObject playerCam;
    public Transform camStartPosition;
    public Transform camMenuPosition;
    public float cameraOffset;
    public static GameManager gameManager;
    public enum GameState{MainMenu, Gameplay, Upgrades, Results}
    public GameState gameState;
    private Stats loadedStats;
    public List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    [Header("Run stats")]
    public float runDistance;
    public float collectedMoney;
    void Awake()
    {
        if(gameManager != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            gameManager = this;
        }
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerCam = GameObject.FindWithTag("MainCamera");
        camStartPosition = GameObject.FindWithTag("CamStart").transform;
        camMenuPosition = GameObject.FindWithTag("CamMenu").transform;
        loadedStats = ScriptableObject.CreateInstance<Stats>();
    }
    
    void Start()
    {
        if(gameState == GameState.MainMenu)
        {
            ChangeGameState();
            SetPlayerToSpawn();
        }
    }

    void Update()
    {

    }
    
    /// <summary>
    /// Used to change game state at specific points, runing methods only once instead of every frame.
    /// </summary>
    public void ChangeGameState()
    {
        Debug.Log("Changing to " + gameState + " Gamestate");
        switch(gameState)
        {
            case GameState.MainMenu:
                MainMenu();
                SetCameraPosition();
                break;
            case GameState.Gameplay:
                Gameplay();
                SetCameraPosition();
                break;
            case GameState.Upgrades:
                Upgrades(); 
                break;
            case GameState.Results: 
                Results(); 
                break;
        }
    }

    void MainMenu()
    {
        uIManager.SetUIMainMenu();
        soundManager.PlayMusic(0); //Fist in music list is menu music.
    }

    void Gameplay()
    {
        player.ResetForNewRun();
        GameObject runMarker = GameObject.FindWithTag("BestRun");
        runMarker.transform.position = player.playerStats.bestRunPositon;
        if(soundManager.musicSource.clip != soundManager.music[1])
        {
            soundManager.PlayMusic(1); //2nd in music list is gameplay music
        }
    }

    void Upgrades()
    {
        ReloadGame();
    }

    void Results()
    {
        StartCoroutine(LandingExplosion());
    }

    void ReloadGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadScene("MainLevel");
    }
    /// <summary>
    /// Quits Entire Game. 
    /// </summary>
    public void QuitGame()
    {
        //Debug line to test quit function in editor
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    /// <summary>
    /// Saves all owned upgrades and player stats
    /// </summary>
    public void SaveGame()
    {
        if(CheckforSave())
        {
            Debug.Log("Overwriting save");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.json", FileMode.Open);
            
            Stats playerSave = player.playerStats;
            string json = JsonUtility.ToJson(playerSave);
            bf.Serialize(file, json);
            file.Close(); 
        }
        else
        {
            Debug.Log("Creating save");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.json");

            Stats playerSave = player.playerStats;
            string json = JsonUtility.ToJson(playerSave);

            bf.Serialize(file, json);
            file.Close();   
        }
    }
    /// <summary>
    /// Checks to see if save exists, return true or false
    /// </summary>
    /// <returns></returns>
    public bool CheckforSave()
    {
        bool doseSaveExisit = File.Exists(Application.persistentDataPath + "/playerInfo.json");
        return doseSaveExisit;
    }
    /// <summary>
    /// Loads player save and brings you right to upgrade menu
    /// </summary>
    public void LoadGame()
    {
        if(CheckforSave())
        {
             BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.json", FileMode.Open);

            string json = (string)bf.Deserialize(file);
            
            file.Close();
            JsonUtility.FromJsonOverwrite(json, loadedStats);
            player.playerStats = loadedStats;
        }
        gameState = GameState.Upgrades;
        upgradeManager.CheckEquptment();
        ChangeGameState();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.gameObject.SetActive(true);
        uIManager.distanceTracker = GameObject.FindWithTag("Marker").GetComponent<DistanceTracker>();
        camMenuPosition = GameObject.FindWithTag("CamMenu").transform;
        player.spawnPoint = GameObject.FindWithTag("Start").transform;
        SetPlayerToSpawn();
        player.ResetForNewRun();
        uIManager.distanceTracker = GameObject.FindWithTag("Marker").GetComponent<DistanceTracker>();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetCameraPosition()
    {
        if(gameState == GameState.MainMenu)
        {
            playerCam.transform.SetParent(this.gameObject.transform);
            playerCam.transform.position = camMenuPosition.position;
            playerCam.transform.rotation = camMenuPosition.rotation;
            //Debug.Log("Moveing Camera to Menu point, " + playerCam.transform.position);
        }
        else
        {
            //playerCam.transform.SetParent(this.gameObject.transform);
            playerCam.transform.SetParent(player.playerTransform);
            playerCam.transform.position = camStartPosition.position;
            playerCam.transform.rotation = camStartPosition.rotation;
            //Debug.Log("Moveing Camera to Gameplay point, " + playerCam.transform.position);
        }
    }

    void SetPlayerToSpawn()
    {
        player.playerTransform.position = player.spawnPoint.position;
        player.playerTransform.rotation = player.spawnPoint.rotation;
        //Debug.Log("Moveing Player to start point, " + player.playerTransform.position);
    }

    public void LoadScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainLevel":
                uIManager.UILoadingScreen(uIManager.upgradeMenu);
                break;
        }
        StartCoroutine(WaitForScreenLoad(sceneName));   
    }

    /// <summary>
    /// Waits for screen to load before starting operation. 
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator WaitForScreenLoad(string sceneName)
    {
        yield return new WaitForSeconds(uIManager.fadeTime);
        //Debug.Log("Loading Scene Starting");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.completed += OperationCompleted;
        scenesToLoad.Add(operation);
    }
    /// <summary>
    /// Gets average progress for Loading bar. 
    /// </summary>
    /// <returns></returns>
    public float GetLoadingProgress()
    {
        float totalprogress = 0;

        foreach (AsyncOperation operation in scenesToLoad)
        {
            totalprogress += operation.progress;
        }

        return totalprogress / scenesToLoad.Count;
    }
    /// <summary>
    /// Event for when load operation is finished. 
    /// </summary>
    /// <param name="operation"></param>
    private void OperationCompleted(AsyncOperation operation)
    {
        scenesToLoad.Remove(operation);
        operation.completed -= OperationCompleted;
    }

    /// <summary>
    /// Used to clear player stats for a fresh game. 
    /// </summary>
    public void ResetPlayerStats()
    {
        player.playerStats.ResetStats();
    }

    /// <summary>
    /// Resets run stats for each fresh run. 
    /// </summary>
    public void StartRun()
    {
        runDistance = 0;
        collectedMoney = 0;
        player.playerStats.fuel = player.playerStats.maxFuel;
        uIManager.SetUIGameplay();
    }

    public IEnumerator LandingExplosion()
    {
        playerCam.transform.SetParent(this.gameObject.transform, true);
        playerCam.transform.position.Set(playerCam.transform.position.x,playerCam.transform.position.y+cameraOffset,playerCam.transform.position.z);
        player.PlaceBestRunMarker();
        if(player.hitWater)
        {
            player.hitWater = false;
            player.Splash();
            Debug.Log("Water Death");
        }
        else
        {
            player.Explode();
            Debug.Log("Standard Death");
        }
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(player.explodeTime);
        uIManager.SetUIResults();
    }
}
