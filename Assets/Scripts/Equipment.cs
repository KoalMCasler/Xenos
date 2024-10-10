using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [Serializable]
    public enum equipType{Matirial, Engine, Fuel, Booster}
    public equipType type;
    public string equipmentName;
    public float cost;
    public string discription;
    public float modValue;
    public Sprite icon;
    public bool isEquipped;
    public bool isOwned;
    public int upgradeIndex;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(icon != null)
        {
            this.GetComponent<Image>().sprite = icon;
        }
    }
}
