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
    [SerializeField]
    private SoundManager soundManager;
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
    [Header("Upgrade Menu UI")]
    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI bestRunText;
    public TextMeshProUGUI objectiveText;
    private float distanceFromWall;
    public GameObject materialTab;
    public GameObject engineTab;
    public GameObject fuelTab;
    public GameObject boosterTab;
    [Header("Run Results")]
    public GameObject newBestRunObject;
    public DistanceTracker distanceTracker;
    public TextMeshProUGUI distanceResultsText;
    public TextMeshProUGUI moneyResultsText;
    public TextMeshProUGUI totalResultsText;
    public float runTotal;
    [Header("HUD")]
    public TextMeshProUGUI altitudeText;
    public TextMeshProUGUI speedText;
    public Image fuelBar;
    [Header("Fuel Bar colors")]
    public Color fullFuel;
    public Color halfFuel;
    public Color lowFuel;


    void Awake()
    {
        gameManager = GameManager.gameManager;
        upgradeManager = gameManager.upgradeManager;
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
            UpdateHud();
        }
        if(optionsMenu.activeSelf)
        {

        }
        if(gameManager.gameState == GameManager.GameState.Upgrades)
        {
            UpdateUpgrades();
        }
        if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            UpdateHud();
        }
        UpdateResults();
    }

    /// <summary>
    /// Updates Hud for player when game is active. 
    /// </summary>
    void UpdateHud()
    {
        if(gameManager.player.hasLaunched)
        {
            speedText.text = string.Format("{0:0.0}kn",gameManager.player.GetSpeed());
            altitudeText.text = string.Format("{0:0.00}m",gameManager.player.altitude);
            fuelBar.fillAmount = gameManager.player.playerStats.fuel/gameManager.player.playerStats.maxFuel;
        }
        else
        {
            speedText.text = string.Format("{0:0.0}kn",0);
            altitudeText.text = string.Format("{0:0.00}m",0);
            fuelBar.fillAmount = gameManager.player.playerStats.fuel/gameManager.player.playerStats.maxFuel;
        }
        CheckFuel();
    }
    /// <summary>
    /// Used to set the color of the fuel gage as it lowers.
    /// </summary>
    void CheckFuel()
    {
        if(fuelBar.fillAmount > 0.75f)
        {
            fuelBar.color = fullFuel;
        }
        if(fuelBar.fillAmount < 0.5f && fuelBar.fillAmount > 0.25f)
        {
            fuelBar.color = halfFuel;
        }
        if(fuelBar.fillAmount < 0.25f)
        {
            fuelBar.color = lowFuel;
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
        if(gameManager.runDistance > gameManager.player.playerStats.bestDistance)
        {
            newBestRunObject.SetActive(true);
        }
        else
        {
            newBestRunObject.SetActive(false);
        }
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
        if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            gameManager.runDistance = gameManager.player.ReturnDistance();
            distanceResultsText.text = string.Format("Distance = {0:0.00}m",gameManager.runDistance);
            moneyResultsText.text = string.Format("Money Colected = {0}$",gameManager.collectedMoney);
            runTotal = gameManager.runDistance + gameManager.collectedMoney;
            totalResultsText.text = string.Format("Total = {0:0.00}$",runTotal);
            distanceFromWall = 9000 - gameManager.runDistance;
        }
    }

    public void EndResults()
    {
        gameManager.player.playerStats.money += runTotal;
        if(gameManager.player.playerStats.bestDistance < gameManager.runDistance)
        {
            gameManager.player.playerStats.bestDistance = gameManager.runDistance;
        }
    }

    void UpdateUpgrades()
    {
        currentMoneyText.text = string.Format("{0:0.00}$",gameManager.player.playerStats.money);
        bestRunText.text = string.Format("{0:0.00}M",gameManager.player.playerStats.bestDistance);
        objectiveText.text = string.Format("You were {0:0.00}M away from THE WALL",distanceFromWall);
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
        //Debug.Log("Starting Fadeout");

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
        //Debug.Log("Ending Fadeout");
    }
    /// <summary>
    /// Fades Loading screen in.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingUIFadeIN()
    {
        //Debug.Log("Starting Fadein");
        float timer = 0;
        loadingScreen.SetActive(true);

        while (timer < fadeTime)
        {
            loadingScreenCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        loadingScreenCanvasGroup.alpha = 1;

        //Debug.Log("Ending Fadein");
        StartCoroutine(LoadingBarProgress());
    }
    /// <summary>
    /// Sets the loading bar progress to average progress of all loading. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingBarProgress()
    {
        //Debug.Log("Starting Progress Bar");
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
        //Debug.Log("Ending Progress Bar");
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

    public void SwapTab(string tab)
    {
        switch (tab)
        {
            case "Material":
                materialTab.transform.SetAsLastSibling();
                break;
            case "Engine":
                engineTab.transform.SetAsLastSibling();
                break;
            case "Fuel":
                fuelTab.transform.SetAsLastSibling();
                break;
            case "Booster":
                boosterTab.transform.SetAsLastSibling();
                break;
            default:
                materialTab.transform.SetAsLastSibling();
                break;
        }
    }
}
