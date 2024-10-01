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
    public bool[] boostLevel = new bool[10];
    public bool[] rampScale = new bool[10];
    public bool[] ownedUpgrades = new bool[18];

    public void ResetStats()
    {
        money = 0;
        boostValue = 0;
        lookSensitivity = .15f;
        boostLevel = new bool[10];
        rampScale = new bool[10];
        ownedUpgrades = new bool[18];
    }
}
