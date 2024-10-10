using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;

public class ToolTipManager : MonoBehaviour
{
    public TextMeshProUGUI equipmentName;
    public TextMeshProUGUI equipmentType;
    public TextMeshProUGUI statModified;
    public TextMeshProUGUI equipmentCost;
    public TextMeshProUGUI discription;
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
        discription.text = eq.discription;
        switch(eq.type)
        {
            case Equipment.equipType.Matirial:
                equipmentType.text = "Matirial";
                statModified.text = string.Format("Mass = {0}kg",eq.modValue);
                break;
            case Equipment.equipType.Engine:
                equipmentType.text = "Engine";
                statModified.text = string.Format("Thrust = {0}",eq.modValue);
                break;
            case Equipment.equipType.Wing:
                equipmentType.text = "Wing";
                statModified.text = string.Format("Fuel = {0}/s",eq.modValue);
                break;
            case Equipment.equipType.Booster:
                equipmentType.text = "Booster";
                statModified.text = string.Format("Start Boost = {0}",eq.modValue);
                break;
        }
    }
}
