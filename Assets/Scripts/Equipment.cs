using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// holds base info for equipment
/// </summary>
public class Equipment : MonoBehaviour
{
    [Serializable]
    public enum equipType{Material, Engine, Fuel, Booster}
    public equipType type;
    public string equipmentName;
    public float cost;
    public string discription;
    public float modValue;
    public Sprite icon;
    public bool isEquipped;
    public bool isOwned;
    public int upgradeIndex;
    public bool isSpecial;
    public bool isLockout;
    public bool isLocked;
    public Equipment lockoutEquip;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(icon != null)
        {
            this.GetComponent<Image>().sprite = icon;
        }
    }
}
