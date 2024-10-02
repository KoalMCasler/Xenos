using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUpgradeButton : MonoBehaviour
{
    public UIManager uIManager;
    public float upgradeCost;
    public bool canAford;
    public int upgradeSlot;
    private GameManager gameManager;
    public Button thisButton;
    public string boostType;
    public Color cantAfordColor;
    public Color canAfordColor;
    public bool isUnlocked;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManager;
        uIManager = gameManager.uIManager;
        thisButton = this.GetComponent<Button>();
        isUnlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfUnlocked();
        if(isUnlocked)
        {
            CheckIfCanAford();
            ChangeColor();
        }
    }

    public void BuyUpgrade()
    {
        if(gameManager.player.playerStats.money >= upgradeCost)
        {
            gameManager.player.playerStats.money -= upgradeCost;
            if(boostType == "Launch")
            {
                gameManager.player.playerStats.boostLevel[upgradeSlot] = true;
                isUnlocked = true;
            }
            if(boostType == "Ramp")
            {
                gameManager.player.playerStats.rampScale[upgradeSlot] = true;
                isUnlocked = true;
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

    void CheckIfUnlocked()
    {
        if(boostType == "Launch")
        {
            if(upgradeSlot == 0)
            {
                isUnlocked = true;
            }
            else
            {
                if(uIManager.boostButtons[upgradeSlot-1].GetComponent<BuffUpgradeButton>().isUnlocked && gameManager.player.playerStats.boostLevel[upgradeSlot-1])
                {
                    isUnlocked = true;
                }
                else
                {
                    isUnlocked = false;
                }
            }
            if(isUnlocked == false)
            {
                ColorBlock cb = thisButton.colors;
                cb.disabledColor = cantAfordColor;
                thisButton.colors = cb;
                thisButton.interactable = false;
            }
        }
        if(boostType == "Ramp")
        {
            if(upgradeSlot == 0)
            {
                isUnlocked = true;
            }
            else
            {
                if(uIManager.rampButtons[upgradeSlot-1].GetComponent<BuffUpgradeButton>().isUnlocked && gameManager.player.playerStats.rampScale[upgradeSlot-1])
                {
                    isUnlocked = true;
                }
                else
                {
                    isUnlocked = false;
                }
            }
            if(isUnlocked == false)
            {
                ColorBlock cb = thisButton.colors;
                cb.disabledColor = cantAfordColor;
                thisButton.colors = cb;
                thisButton.interactable = false;
            }
        }
    }


}
