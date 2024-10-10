using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats", order = 0)]
public class Stats : ScriptableObject
{
    public float money;
    public float startBoost;
    public float maxFuel;
    public float fuel;
    public float boostSpeed;
    public float lookSensitivity;
    public bool[] ownedEquipment = new bool[24];

    public void ResetStats()
    {
        money = 0;
        lookSensitivity = .15f;
        ownedEquipment = new bool[24];
    }
}
