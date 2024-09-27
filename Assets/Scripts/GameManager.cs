using System.Collections;
using System.Collections.Generic;
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
    public static GameManager gameManager;
    public enum GameState{MainMenu, Gameplay, Upgrades, Results}
    public GameState gameState;
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
    }
    
    void Start()
    {
        gameState = GameState.Gameplay;
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

    }

    void Gameplay()
    {

    }

    void Upgrades()
    {

    }

    void Results()
    {
        ReloadGame();
    }

    void ReloadGame()
    {
        gameState = GameState.Gameplay;
        SceneManager.LoadScene("MainLevel");
    }
}
