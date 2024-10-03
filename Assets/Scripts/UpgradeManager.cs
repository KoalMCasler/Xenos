using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handels all upgrades and checks if player has them unlocked
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    [Header("Object Referances")]
    [SerializeField]
    private GameManager gameManager;
    public Stats playerStats;
    public GameObject ramp;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManager;
        playerStats = gameManager.player.playerStats;
        ramp = GameObject.FindWithTag("Ramp");
    }

    void OnLoadGame()
    {
        playerStats = gameManager.player.playerStats;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBoostUpgrades();
        CheckRampUpgrades();
    }

    public void CheckBoostUpgrades()
    {
        int boostLevelTotal = 0;
        for(int i = 0; i < playerStats.boostLevel.Count(); i++)
        {
            if(playerStats.boostLevel[i] == true)
            {
                boostLevelTotal += 1;
            }
        }
        if(boostLevelTotal > 0)
        {
            playerStats.boostValue = boostLevelTotal * 300;
        }
    }

    void CheckRampUpgrades()
    {
        float rampLevelTotal = 1;
        for(int i = 0; i < playerStats.rampScale.Count(); i++)
        {
            if(playerStats.rampScale[i] == true)
            {
                rampLevelTotal += 0.1f;
            }
        }
        float rampScale = 0.01f;
        if(rampLevelTotal == 1)
        {
            rampScale = 0.01f;
        }
        else
        {
            rampScale = rampLevelTotal/100;
        }
        Vector3 newScale = new Vector3(0.01f,rampLevelTotal/100,rampScale);
        ramp.transform.localScale = newScale;
    }

    public void CheckEquptment()
    {

    }
}
