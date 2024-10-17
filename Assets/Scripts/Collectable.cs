using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Makes item a collectable, ether money or fuel
/// </summary>
public class Collectable : MonoBehaviour
{
    public GameManager gameManager;
    public SoundManager soundManager;
    public enum CollectableType{Money, Fuel}
    public CollectableType type;
    public float gainValue;

    void Start()
    {
        gameManager = GameManager.gameManager;
        soundManager = gameManager.soundManager;
    }

    /// <summary>
    /// Trigger event used for collection
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gameManager = other.GetComponent<PlayerController>().gameManager;
            switch(type)
            {
                case CollectableType.Money:
                    CollectMoney();
                    break;
                
                case CollectableType.Fuel:
                    CollectFuel(other.gameObject);
                    break;
            }
        }
    }
    /// <summary>
    /// If type is fuel, ups player current fuel on collection
    /// </summary>
    /// <param name="player"></param>
    void CollectFuel(GameObject player)
    {
        soundManager.PlaySFX(5); //See list in editor for index. 
        player.GetComponent<PlayerController>().playerStats.fuel += gainValue;
        if(player.GetComponent<PlayerController>().playerStats.fuel > player.GetComponent<PlayerController>().playerStats.maxFuel)
        {
            player.GetComponent<PlayerController>().playerStats.fuel = player.GetComponent<PlayerController>().playerStats.maxFuel;
        }
        Destroy(this.gameObject);
    }
    /// <summary>
    /// If tpye is money, adds money to player end results. 
    /// </summary>
    void CollectMoney()
    {
        soundManager.PlaySFX(4); //See list in editor for index. 
        gameManager.collectedMoney += gainValue;
        Destroy(this.gameObject);
    }
}
