using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    public Button shopButton;
    public float toolTipOffscreenOffset;
    void Awake()
    {
        tTM =  FindObjectOfType<ToolTipManager>();
        toolTipWindow = tTM.toolTipWindow;
        shopButton = this.GetComponent<Button>();
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
        if(equipment.isLockout)
        {
            if(equipment.isLocked)
            {
                shopButton.interactable = false;
            }
            else
            {
                shopButton.interactable = true;
            }
        }
    }

    void OnEnable()
    {
        toolTipWindow = tTM.toolTipWindow;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipWindow.SetActive(true);
        Vector3 offset = new Vector3();
        Debug.Log(eventData.position);
        if(eventData.position.x > 1500)
        {
            offset.Set(toolTipOffscreenOffset,toolTipOffset.y,toolTipOffset.z);
        }
        else
        {
            offset = toolTipOffset;
        }
        toolTipWindow.transform.position = equipment.transform.position - offset;
        tTM.SetToolTip(equipment);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipWindow.SetActive(false);
    }
}