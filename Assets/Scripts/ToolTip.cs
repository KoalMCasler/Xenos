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
    public GameManager gameManager;
    public Equipment equipment;
    private ToolTipManager tTM;
    public GameObject toolTipWindow;
    public float toolTipOffset;
    public GameObject ownedMarker;
    public Button shopButton;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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
        if(equipment.cost > gameManager.player.playerStats.money && !equipment.isOwned)
        {
            shopButton.interactable = false;
        }
        else
        {
            shopButton.interactable = true;
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
    }

    void OnEnable()
    {
        toolTipWindow = tTM.toolTipWindow;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //toolTipWindow.SetActive(true);
        //toolTipWindow.GetComponent<RectTransform>().position = new Vector3(equipment.transform.position.x-toolTipOffset,eventData.position.y, 0);    
        tTM.SetToolTip(equipment);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //toolTipWindow.SetActive(false);
        tTM.ClearToolTip();
    }
}