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
    public Rigidbody playerBody;
    public Equipment[] equipment; //filled in editor. 
    public EquipmentSlot matSlot;
    public EquipmentSlot engSlot;
    public EquipmentSlot fuelSlot;
    public EquipmentSlot boostSlot;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManager;
        playerStats = gameManager.player.playerStats;
        playerBody = gameManager.player.playerBody;
        for(int i = 0; i < equipment.Count(); i++)
        {
            equipment[i].upgradeIndex = i;
        }
    }
    /// <summary>
    /// Gets stats on game load. 
    /// </summary>
    public void OnLoadGame()
    {
        playerStats = gameManager.player.playerStats;
    }
    /// <summary>
    /// Checks what the player owns.
    /// </summary>
    public void CheckEquptment()
    {
        for(int i = 0; i < equipment.Count();i++)
        {
            if(playerStats.ownedEquipment[i])
            {
                equipment[i].isOwned = true;
                if(equipment[i].isLockout)
                {
                    equipment[i].lockoutEquip.isLocked = true;
                }
            }
        }
    }
    /// <summary>
    /// Used by shop buttons, buys or equips if already owned.
    /// </summary>
    /// <param name="eq"></param>
    public void BuyOrEquip(GameObject eq)
    {
        Equipment equip = eq.GetComponent<Equipment>(); 
        if(!equip.isOwned && playerStats.money >= equip.cost)
        {
            equip.isOwned = true;
            playerStats.ownedEquipment[equip.upgradeIndex] = true; 
            playerStats.money -= equip.cost;
            if(equip.isLockout)
            {
                equip.lockoutEquip.isLocked = true;
            }
            switch(equip.type)
            {
                case Equipment.equipType.Material:
                    matSlot.currentEquipment = equip;
                    break;
                case Equipment.equipType.Engine:
                    engSlot.currentEquipment = equip;
                    break;
                case Equipment.equipType.Fuel:
                    fuelSlot.currentEquipment = equip;
                    break;
                case Equipment.equipType.Booster:
                    boostSlot.currentEquipment = equip;
                    break;
            }
        }
        else if(equip.isOwned)
        {
            switch(equip.type)
            {
                case Equipment.equipType.Material:
                    matSlot.currentEquipment = equip;
                    break;
                case Equipment.equipType.Engine:
                    engSlot.currentEquipment = equip;
                    break;
                case Equipment.equipType.Fuel:
                    fuelSlot.currentEquipment = equip;
                    break;
                case Equipment.equipType.Booster:
                    boostSlot.currentEquipment = equip;
                    break;
            }
        }
    }
    /// <summary>
    /// Sets player stats based on current equipmemnt
    /// </summary>
    public void GetStats()
    {
        if(engSlot.currentEquipment != null)
        {
            playerStats.boostSpeed = engSlot.currentEquipment.modValue;
        }
        if(fuelSlot.currentEquipment != null)
        {
            playerStats.maxFuel = fuelSlot.currentEquipment.modValue;
            playerStats.fuel = playerStats.maxFuel;
        }
        if(matSlot.currentEquipment != null)
        {
            playerBody.mass = matSlot.currentEquipment.modValue/100;
        }
        if(boostSlot.currentEquipment != null)
        {
            playerStats.startBoost = boostSlot.currentEquipment.modValue;
        }
    }

    public void ClearEquipSlots()
    {
        matSlot.currentEquipment = null;
        engSlot.currentEquipment = null; 
        fuelSlot.currentEquipment = null;
        boostSlot.currentEquipment = null;
        for(int i = 0; i < equipment.Count();i++)
        {
            equipment[i].isOwned = false;
            equipment[i].isLocked = false;
        }
    }
}
