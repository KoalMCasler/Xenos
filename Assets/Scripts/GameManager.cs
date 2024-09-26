using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public enum GameState{MainMenu, Gameplay, Upgrades}
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
        
    }

    void Update()
    {

    }
    
    /// <summary>
    /// Used to change game state at specific points, runing methods only once instead of every frame.
    /// </summary>
    void ChangeGameState()
    {
        switch(gameState)
        {
            case GameState.MainMenu: MainMenu(); break;
            case GameState.Gameplay: Gameplay(); break;
            case GameState.Upgrades: Upgrades(); break;
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
}
