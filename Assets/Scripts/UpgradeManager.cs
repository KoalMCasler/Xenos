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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManager;
        playerStats = gameManager.player.playerStats;
    }

    void OnLoadGame()
    {
        playerStats = gameManager.player.playerStats;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckEquptment()
    {

    }

    public void BuyEquipment(Equipment eq)
    {

    }
}
