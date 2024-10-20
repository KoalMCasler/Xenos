using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Used to pass info into tooltip window.
/// </summary>
public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Equipment equipment;
    private ToolTipManager tTM;
    public GameObject toolTipWindow;
    public Vector3 toolTipOffset;
    public GameObject ownedMarker;
    void Awake()
    {
        tTM =  FindObjectOfType<ToolTipManager>();
        toolTipWindow = tTM.toolTipWindow;
    }
    void Update()
    {
        if(toolTipWindow == null)
        {
            toolTipWindow = tTM.toolTipWindow;
        }
        if(equipment.isOwned)
        {
            ownedMarker.SetActive(true);
        }
        else
        {
            ownedMarker.SetActive(false);
        }
    }

    void OnEnable()
    {
        toolTipWindow = tTM.toolTipWindow;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipWindow.SetActive(true);
        toolTipWindow.transform.position = equipment.transform.position - toolTipOffset;
        tTM.SetToolTip(equipment);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipWindow.SetActive(false);
    }
}