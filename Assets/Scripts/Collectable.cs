using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void CollectMoney()
    {
        soundManager.PlaySFX(4); //See list in editor for index. 
        gameManager.collectedMoney += gainValue;
        Destroy(this.gameObject);
    }
}
