using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;

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
    public GameObject mainMenu;
    public Button loadButton;
    public GameObject optionsMenu;
    public GameObject upgradeMenu;
    public GameObject resultsMenu;
    public GameObject storyMenu;
    [Header("Loading Screen UI Elements")]
    public GameObject loadingScreen;
    public CanvasGroup loadingScreenCanvasGroup;
    public Image loadingBar;
    public float fadeTime;
    [Header("UpgradeButtons")]
    public Button[] boostButtons;
    public Button[] rampButtons;
    [Header("Upgrade Menu UI")]
    public TextMeshProUGUI currentMoneyText;
    [Header("Run Results")]
    public DistanceTracker distanceTracker;
    public TextMeshProUGUI distanceResultsText;
    public TextMeshProUGUI moneyResultsText;
    public TextMeshProUGUI totalResultsText;
    public float runTotal;

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
        if(gameManager.gameState == GameManager.GameState.Upgrades)
        {
            UpdateUpgrades();
        }
        UpdateResults();
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
            if(gameManager.gameState == GameManager.GameState.Results)
            {
                EndResults();
            }
            ResetAllMenus();
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
        if(gameManager.gameState == GameManager.GameState.Results || gameManager.gameState == GameManager.GameState.Gameplay)
        {
            gameManager.runDistance = distanceTracker.returnDistance();
            distanceResultsText.text = string.Format("Distance = {0:0.0}m",gameManager.runDistance);
            moneyResultsText.text = string.Format("Distance = {0}$",gameManager.collectedMoney);
            runTotal = gameManager.runDistance + gameManager.collectedMoney;
            totalResultsText.text = string.Format("Total = {0:0.0}$",runTotal);
        }
    }

    public void EndResults()
    {
        gameManager.player.playerStats.money += runTotal;
    }

    void UpdateUpgrades()
    {
        currentMoneyText.text = string.Format("{0:0.0}$",gameManager.player.playerStats.money);
        ManageBoostButtons();
        ManageRampButtons();
    }

    void ManageBoostButtons()
    {
        
        for(int i = 0; i < upgradeManager.playerStats.boostLevel.Count(); i++)
        {
            if(boostButtons[i].GetComponent<BuffUpgradeButton>().canAford)
            {
                if(!upgradeManager.playerStats.boostLevel[i])
                {
                    boostButtons[i].interactable = true;
                }
                else
                {
                    boostButtons[i].interactable = false;
                }
            }
            else
            {
                boostButtons[i].interactable = false;
            }
        }
    }

    void ManageRampButtons()
    {
        for(int i = 0; i < upgradeManager.playerStats.rampScale.Count(); i++)
        {
            if(rampButtons[i].GetComponent<BuffUpgradeButton>().canAford)
            {
                if(!upgradeManager.playerStats.rampScale[i])
                {
                    rampButtons[i].interactable = true;
                }
                else
                {
                    rampButtons[i].interactable = false;
                }
                
            }
            else
            {
                rampButtons[i].interactable = false;
            }
        }
    }

    /// <summary>
    /// Starts UI loading screen process.
    /// </summary>
    /// <param name="targetPanel"></param>
    public void UILoadingScreen(GameObject targetPanel)
    {
        StartCoroutine(LoadingUIFadeIN());
        StartCoroutine(DelayedSwitchUIPanel(fadeTime, targetPanel));
    }

    /// <summary>
    /// Fades loading scnreen out.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingUIFadeOut()
    {
        Debug.Log("Starting Fadeout");

        float timer = 0;

        while (timer < fadeTime)
        {
            loadingScreenCanvasGroup.alpha = Mathf.Lerp(1, 0, timer/fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        loadingScreenCanvasGroup.alpha = 0;
        loadingScreen.SetActive(false);
        loadingBar.fillAmount = 0;
        Debug.Log("Ending Fadeout");
    }
    /// <summary>
    /// Fades Loading screen in.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingUIFadeIN()
    {
        Debug.Log("Starting Fadein");
        float timer = 0;
        loadingScreen.SetActive(true);

        while (timer < fadeTime)
        {
            loadingScreenCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        loadingScreenCanvasGroup.alpha = 1;

        Debug.Log("Ending Fadein");
        StartCoroutine(LoadingBarProgress());
    }
    /// <summary>
    /// Sets the loading bar progress to average progress of all loading. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingBarProgress()
    {
        Debug.Log("Starting Progress Bar");
        while (gameManager.scenesToLoad.Count <= 0)
        {
            //waiting for loading to begin
            yield return null;
        }
        while (gameManager.scenesToLoad.Count > 0)
        {
            loadingBar.fillAmount = gameManager.GetLoadingProgress();
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        Debug.Log("Ending Progress Bar");
        StartCoroutine(LoadingUIFadeOut());
    }
    /// <summary>
    /// used for fade in fade out for loading screen UI. 
    /// </summary>
    /// <param name="time"></param>
    /// <param name="uiPanel"></param>
    /// <returns></returns>
    private IEnumerator DelayedSwitchUIPanel(float time, GameObject uiPanel)
    {
        yield return new WaitForSeconds(time);
        ResetAllMenus();
        uiPanel.SetActive(true);
    }
}
