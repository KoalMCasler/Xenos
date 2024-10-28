using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Marks wether item in shop slot is owned. 
/// </summary>
public class ShopSlot : MonoBehaviour
{
    public Equipment equip;
    public GameObject isOnwedMarker;
    public Button shopButton;
    // Update is called once per frame
    void Update()
    {
        if(equip.isOwned)
        {
            isOnwedMarker.SetActive(true);
        }
        else
        {
            isOnwedMarker.SetActive(false);
        }
        
    }
}
