using System.Collections;
using System.Collections.Generic;
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
    public UIManager uIManager;
    public UpgradeManager upgradeManager;
    public SoundManager soundManager;
    public CollectableManager collectableManager;
    public PlayerController player;
    public GameObject playerCam;
    public Transform camStartPosition;
    public Transform camMenuPosition;
    public float cameraOffset;
    public static GameManager gameManager;
    public enum GameState{MainMenu, Gameplay, Upgrades, Results, Options, Paused, GameEnd}
    public bool isPaused;
    public GameState gameState;
    public GameState prevState;
    private Stats loadedStats;
    public List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    public bool autoSaveActive;
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
        isPaused = false;
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
            player.SetPlayerToSpawn();
        }
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
            case GameState.Options:
                break;
            case GameState.Paused:
                Pause();
                break;
            case GameState.GameEnd:
                GameEnd();
                break;
        }
    }
    /// <summary>
    /// Main menu gamestate function
    /// </summary>
    void MainMenu()
    {
        Time.timeScale = 1;
        uIManager.SetUIMainMenu();
        if(player.gameObject.activeSelf == false)
        {
            player.gameObject.SetActive(true);
            player.SetPlayerToSpawn();
        }
        if(soundManager.musicSource.clip != soundManager.music[soundManager.activeSongIndex])
        {
            soundManager.PlayMusic(soundManager.activeSongIndex); 
        }
        if(collectableManager.activeCollectables.Count > 0)
        {
            collectableManager.ClearCollectables();
        }
    }
    /// <summary>
    /// Gameplay gamestate function
    /// </summary>
    void Gameplay()
    {
        Time.timeScale = 1;
        uIManager.SetUIGameplay();
        prevState = GameState.Gameplay;
        GameObject runMarker = GameObject.FindWithTag("BestRun");
        runMarker.transform.position = player.playerStats.bestRunPositon;
        if(soundManager.musicSource.clip != soundManager.music[soundManager.activeSongIndex])
        {
            soundManager.PlayMusic(soundManager.activeSongIndex); 
        }
    }
    /// <summary>
    /// Upgrades gamestate function
    /// </summary>
    void Upgrades()
    {
        player.playerBody.velocity = Vector3.zero;
        isPaused = false;
        Time.timeScale = 1;
        if(prevState != GameState.Upgrades)
        {
            ReloadGame();
        }
    }
    void GameEnd()
    {
        Time.timeScale = 1;
        StartCoroutine(EndGame());
    }
    /// <summary>
    /// Results gamestate function
    /// </summary>
    void Results()
    {
        Time.timeScale = 1;
        StartCoroutine(LandingExplosion());
    }
    void Pause()
    {
        Time.timeScale = 0;
        uIManager.SetUIPause();
    }
    /// <summary>
    /// Reloads game for next run.
    /// </summary>
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
        autoSaveActive = player.playerStats.autoSaveActive;
        gameState = GameState.Upgrades;
        upgradeManager.OnLoadGame();
        upgradeManager.CheckEquptment();
        uIManager.SetUIUpgrades();
        ChangeGameState();
    }
    /// <summary>
    /// OnSceneLoaded event
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.gameObject.SetActive(true);
        camMenuPosition = GameObject.FindWithTag("CamMenu").transform;
        player.spawnPoint = GameObject.FindWithTag("Start").transform;
        player.SetPlayerToSpawn();
        player.ResetForNewRun();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    /// <summary>
    /// Sets camera to correct position depending on gamestate
    /// </summary>
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

    /// <summary>
    /// Load target scene
    /// </summary>
    /// <param name="sceneName"></param>
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
        upgradeManager.ClearEquipSlots();
        player.playerStats.ResetStats();
        player.playerStats.autoSaveActive = autoSaveActive;
    }

    /// <summary>
    /// Resets run stats for each fresh run. 
    /// </summary>
    public void StartRun()
    {
        if(autoSaveActive)
        {
            SaveGame();
        }
        runDistance = 0;
        collectedMoney = 0;
        player.playerStats.fuel = player.playerStats.maxFuel;
        uIManager.SetUIGameplay();
    }
    /// <summary>
    /// Has player explode and waits effect to finish before showing results. 
    /// </summary>
    /// <returns></returns>
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
    public IEnumerator EndGame()
    {
        playerCam.transform.SetParent(gameObject.transform, true);
        playerCam.transform.position.Set(playerCam.transform.position.x,playerCam.transform.position.y+cameraOffset,playerCam.transform.position.z);
        player.Explode();
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(player.endTime);
        uIManager.SetUIGameEnd();
        player.ResetForNewRun();
    }

    public void toggleAutoSave()
    {
        if(autoSaveActive)
        {
            autoSaveActive = false;
            player.playerStats.autoSaveActive = autoSaveActive;
        }
        else
        {
            autoSaveActive = true;
            player.playerStats.autoSaveActive = autoSaveActive;
        }
    }

    public void SetGameState(string state)
    {
        switch(state)
        {
            case "Gameplay":
                gameState = GameState.Gameplay;
                ChangeGameState();
                break;
            case "Menu":
                gameState = GameState.MainMenu;
                ChangeGameState();
                break; 
            case "Upgrades":
                gameState = GameState.Upgrades;
                ChangeGameState();
                break;
        }
    }
}
