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
        loadedStats = ScriptableObject.CreateInstance<Stats>();
    }
    
    void Start()
    {
        if(gameState == GameState.MainMenu)
        {
            ChangeGameState();
        }
    }

    void Update()
    {
        if(gameState != GameState.Gameplay)
        {
            player.gameObject.transform.position = GameObject.FindWithTag("Start").transform.position;
            player.gameObject.transform.rotation = GameObject.FindWithTag("Start").transform.rotation;
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
            case GameState.MainMenu: MainMenu(); break;
            case GameState.Gameplay: Gameplay(); break;
            case GameState.Upgrades: Upgrades(); break;
            case GameState.Results: Results(); break;
        }
    }

    void MainMenu()
    {
        uIManager.SetUIMainMenu();
    }

    void Gameplay()
    {
        
    }

    void Upgrades()
    {
        ReloadGame();
    }

    void Results()
    {
        uIManager.SetUIResults();
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
        ChangeGameState();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.ResetPlayerBools();
        player.gameObject.transform.position = GameObject.FindWithTag("Start").transform.position;
        player.gameObject.transform.rotation = GameObject.FindWithTag("Start").transform.rotation;
        uIManager.distanceTracker = GameObject.FindWithTag("Marker").GetComponent<DistanceTracker>();
        upgradeManager.ramp = GameObject.FindWithTag("Ramp");
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        Debug.Log("Loading Scene Starting");

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
    }
}
