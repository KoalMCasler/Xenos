using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// Holds current player equipment
/// </summary>
public class EquipmentSlot : MonoBehaviour
{
    public enum SlotType{Material, Engine, Fuel, Booster}
    public SlotType type;
    public Equipment currentEquipment;
    public TextMeshProUGUI currentStats;
    public Image currentImage;
    public Sprite defultIcon;
    // Update is called once per frame
    void Update()
    {
        CheckCurrentEquipment();
    }

    /// <summary>
    /// Checks what is currently equip and updates UI.
    /// </summary>
    void CheckCurrentEquipment()
    {
        if(currentEquipment == null)
        {
            switch(type)
            {
                case SlotType.Material:
                    currentStats.text = "Trash\nMass = 100kg";
                    break;
                case SlotType.Engine:
                    currentStats.text = "Old Car Engine\nThrust = 10N";
                    break;
                case SlotType.Fuel:
                    currentStats.text = "Jerry Can\nFuel = 5L";
                    break;
                case SlotType.Booster:
                    currentStats.text = "None\nStarting Boost = 0N";
                    break;
            }
            currentImage.sprite = defultIcon;
        }
        else
        {
            switch(type)
            {
                case SlotType.Material:
                    currentStats.text = string.Format("{0}\nMass = {1}kg",currentEquipment.equipmentName,currentEquipment.modValue);
                    break;
                case SlotType.Engine:
                    currentStats.text = string.Format("{0}\nThrust = {1}N",currentEquipment.equipmentName,currentEquipment.modValue);
                    break;
                case SlotType.Fuel:
                    currentStats.text = string.Format("{0}\nFuel = {1}L",currentEquipment.equipmentName,currentEquipment.modValue);
                    break;
                case SlotType.Booster:
                    currentStats.text = string.Format("{0}\nStarting Boost = {1}N",currentEquipment.equipmentName,currentEquipment.modValue);
                    break;
            }
            currentImage.sprite = currentEquipment.icon;
        }
    }
}
