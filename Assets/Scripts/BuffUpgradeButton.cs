using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class BuffUpgradeButton : MonoBehaviour
{
    public float upgradeCost;
    public bool canAford;
    public int upgradeSlot;
    private GameManager gameManager;
    public Button thisButton;
    public string boostType;
    public Color cantAfordColor;
    public Color canAfordColor;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManager;
        thisButton = this.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfCanAford();
        ChangeColor();
    }

    public void BuyUpgrade()
    {
        if(gameManager.player.playerStats.money >= upgradeCost)
        {
            gameManager.player.playerStats.money -= upgradeCost;
            if(boostType == "Launch")
            {
                gameManager.player.playerStats.boostLevel[upgradeSlot] = true;
            }
            if(boostType == "Ramp")
            {
                gameManager.player.playerStats.rampScale[upgradeSlot] = true;
            }
        }
    }

    public void ChangeColor()
    {
        if(boostType == "Launch")
        {
            if(canAford == false || gameManager.player.playerStats.boostLevel[upgradeSlot] == false)
            {
                ColorBlock cb = thisButton.colors;
                cb.disabledColor = cantAfordColor;
                thisButton.colors = cb;
                //thisButton.interactable = false;
            }
            if(canAford == true || gameManager.player.playerStats.boostLevel[upgradeSlot] == true)
            {
                ColorBlock cb = thisButton.colors;
                cb.disabledColor = canAfordColor;
                thisButton.colors = cb;
            }
        }
        if(boostType == "Ramp")
        {
                if(canAford == false || gameManager.player.playerStats.rampScale[upgradeSlot] == false)
            {
                ColorBlock cb = thisButton.colors;
                cb.disabledColor = cantAfordColor;
                thisButton.colors = cb;
                //thisButton.interactable = false;
            }
            if(canAford == true || gameManager.player.playerStats.rampScale[upgradeSlot] == true)
            {
                ColorBlock cb = thisButton.colors;
                cb.disabledColor = canAfordColor;
                thisButton.colors = cb;
            }
        }
    }

    void CheckIfCanAford()
    {
        if(upgradeCost <= gameManager.player.playerStats.money)
        {
            canAford = true;
        }
        else
        {
            canAford = false;
        }
    }


}
