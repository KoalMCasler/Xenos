using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats", order = 0)]
public class Stats : ScriptableObject
{
    public float money;
    public int boostValue;
    public float maxFuel;
    public float fuel;
    public int boostSpeed;
    public float lookSensitivity;
    public Equipment[] ownedEquipment = new Equipment[24]; 

    public void ResetStats()
    {
        money = 0;
        lookSensitivity = .15f;
        ownedEquipment = new Equipment[24];
    }
}
