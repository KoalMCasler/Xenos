using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

/// <summary>
/// Controls all UI elements
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Object Referances")]
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private UpgradeManager upgradeManager;
    [Header("Menus")]
    [SerializeField]
    private GameObject mainMenu;
    public Button loadButton;
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private GameObject upgradeMenu;
    [SerializeField]
    private GameObject resultsMenu;
    [SerializeField]
    private GameObject storyMenu;
    public DistanceTracker distanceTracker;
    [Header("UpgradeButtons")]
    public Button[] boostButtons = new Button[10];
    public Button[] rampButtons = new Button[10];

    void Awake()
    {
        gameManager = GameManager.gameManager;
        upgradeManager = gameManager.upgradeManager;
    }
    // Start is called before the first frame update
    void Start()
    {
        distanceTracker = GameObject.FindWithTag("Marker").GetComponent<DistanceTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mainMenu.activeSelf)
        {
            if(gameManager.CheckforSave())
            {
                loadButton.interactable = true;
            }
            else
            {
                loadButton.interactable = false;
            }
        }
        if(optionsMenu.activeSelf)
        {

        }
        if(upgradeMenu.activeSelf)
        {
            UpdateUpgrades();
        }
        if(resultsMenu.activeSelf)
        {
            UpdateResults();
        }
    }
    /// <summary>
    /// Used to clear all active UI to set up for activating a specific menu. See SetUI... methods.
    /// </summary>
    void ResetAllMenus()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        resultsMenu.SetActive(false);
        storyMenu.SetActive(false);
    }
    /// <summary>
    /// Sets UI to main menu
    /// </summary>
    public void SetUIMainMenu()
    {
        ResetAllMenus();
        if(gameManager.gameState != GameManager.GameState.MainMenu)
        {
            gameManager.gameState = GameManager.GameState.MainMenu;
            gameManager.ChangeGameState();
        }
        mainMenu.SetActive(true);
    }
    /// <summary>
    /// Sets UI to Options menu
    /// </summary>
    public void SetUIOptions()
    {
        ResetAllMenus();
        optionsMenu.SetActive(true);
    }
    /// <summary>
    /// Sets UI to Upgrades menu
    /// </summary>
    public void SetUIUpgrades()
    {
        if(gameManager.gameState != GameManager.GameState.Upgrades)
        {
            ResetAllMenus();
            upgradeMenu.SetActive(true);
            gameManager.gameState = GameManager.GameState.Upgrades;
            gameManager.ChangeGameState();
        }
    }
    /// <summary>
    /// Sets UI to Results menu 
    /// </summary>
    public void SetUIResults()
    {
        ResetAllMenus();
        resultsMenu.SetActive(true);
    }
    /// <summary>
    /// Sets UI to Story menu
    /// </summary>
    public void SetUIStoryMenu()
    {
        ResetAllMenus();
        storyMenu.SetActive(true);
    }
    /// <summary>
    /// Sets UI to be ready for gameplay
    /// </summary>
    public void SetUIGameplay()
    {
        ResetAllMenus();
        if(gameManager.gameState != GameManager.GameState.Gameplay)
        {
            gameManager.gameState = GameManager.GameState.Gameplay;
            gameManager.ChangeGameState();
        }
    }

    void UpdateResults()
    {

    }

    void UpdateUpgrades()
    {

    }
}
