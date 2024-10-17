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
    public Vector3 bestRunPositon;
    public float bestDistance;
    /// <summary>
    /// Resets stats for new game. 
    /// </summary>
    public void ResetStats()
    {
        money = 0;
        lookSensitivity = .05f;
        maxFuel = 5;
        startBoost = 0;
        boostSpeed = 10;
        ownedEquipment = new bool[24];
        bestRunPositon = Vector3.zero;
        bestDistance = 0;
    }
}
