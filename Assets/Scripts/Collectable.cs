using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType{Money, Fuel}
    public CollectableType type;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player");
        {
            switch(type)
            {
                case CollectableType.Money:
                    CollectMoney(other.gameObject);
                    break;
                
                case CollectableType.Fuel:
                    CollectFuel(other.gameObject);
                    break;
            }
        }
    }

    void CollectFuel(GameObject player)
    {
        Destroy(this.gameObject);
    }

    void CollectMoney(GameObject player)
    {
        Destroy(this.gameObject);
    }
}
