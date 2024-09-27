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
        gameState = GameState.MainMenu;
        ChangeGameState();
    }

    void Update()
    {

    }
    
    /// <summary>
    /// Used to change game state at specific points, runing methods only once instead of every frame.
    /// </summary>
    public void ChangeGameState()
    {
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
        SceneManager.LoadScene("MainLevel");
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
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.json");

        Stats playerSave = player.playerStats;
        string json = JsonUtility.ToJson(playerSave);

        bf.Serialize(file, json);
        file.Close();   
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
        if(File.Exists(Application.persistentDataPath + "/playerInfo.json"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.json", FileMode.Open);

            string json = (string)bf.Deserialize(file);
            
            file.Close();
            JsonUtility.FromJsonOverwrite(json, loadedStats);
            player.playerStats = loadedStats;

        }
        uIManager.SetUIUpgrades();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.ResetPlayerBools();
        player.gameObject.transform.position = GameObject.FindWithTag("Start").transform.position;
        player.gameObject.transform.rotation = GameObject.FindWithTag("Start").transform.rotation;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
