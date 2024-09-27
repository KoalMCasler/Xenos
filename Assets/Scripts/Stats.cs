using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats", order = 0)]
public class Stats : ScriptableObject
{
    public float money;
    public int boostValue;
    public float lookSensitivity;
    public bool[] boostLevel = new bool[10];
    public bool[] rampScale = new bool[10];
    public bool[] ownedUpgrades = new bool[18];
}
