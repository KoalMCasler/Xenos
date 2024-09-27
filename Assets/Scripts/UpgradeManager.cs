using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handels all upgrades and checks if player has them unlocked
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    [Header("Object Referances")]
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Stats playerStats;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManager;
        playerStats = gameManager.player.playerStats;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
