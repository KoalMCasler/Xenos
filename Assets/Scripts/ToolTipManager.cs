using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;
/// <summary>
/// Manages tooltip window.
/// </summary>
public class ToolTipManager : MonoBehaviour
{
    public TextMeshProUGUI equipmentName;
    public TextMeshProUGUI equipmentType;
    public TextMeshProUGUI statModified;
    public TextMeshProUGUI equipmentCost;
    public TextMeshProUGUI description;
    public GameObject toolTipWindow;
    void Awake()
    {
        toolTipWindow = GameObject.FindWithTag("ToolTip");
    }
    /// <summary>
    /// Sets the tooltip for current item being moused over.
    /// </summary>
    /// <param name="eq"></param>
    public void SetToolTip(Equipment eq)
    {
        equipmentName.text = eq.equipmentName;
        equipmentCost.text = string.Format("{0}$",eq.cost);
        description.text = eq.discription;
        switch(eq.type)
        {
            case Equipment.equipType.Material:
                equipmentType.text = "Material";
                statModified.text = string.Format("Mass = {0}kg",eq.modValue);
                break;
            case Equipment.equipType.Engine:
                equipmentType.text = "Engine";
                statModified.text = string.Format("Thrust = {0}",eq.modValue);
                break;
            case Equipment.equipType.Fuel:
                equipmentType.text = "Fuel";
                statModified.text = string.Format("Fuel = {0}L",eq.modValue);
                break;
            case Equipment.equipType.Booster:
                equipmentType.text = "Booster";
                statModified.text = string.Format("Start Boost = {0}",eq.modValue);
                break;
        }
    }
    public void ClearToolTip()
    {
        equipmentName.text = "Item Name";
        equipmentCost.text = "Cost";
        description.text = "Description";
        equipmentType.text = "Type";
        statModified.text = "Mod Value";
    }
}
