using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Equipment equipment;
    public ToolTipManager tTM;
    public GameObject toolTipWindow;
    public Vector3 toolTipOffset;
    void Awake()
    {
        tTM =  FindObjectOfType<ToolTipManager>();
        toolTipWindow = GameObject.FindWithTag("ToolTip");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipWindow.SetActive(true);
        toolTipWindow.transform.position = equipment.gameObject.transform.position - toolTipOffset;
        tTM.SetToolTip(equipment);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipWindow.SetActive(false);
    }
}