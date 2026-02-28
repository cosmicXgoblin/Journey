using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// script attached to the tooltip
/// </summary>
public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// the message we want to display
    /// </summary>
    public string msg;
    /// <summary>
    /// the inventorySlot we are hovering over
    /// </summary>
    [SerializeField] private InventorySlot _invSlot;

    /// <summary>
    /// gives us back the slot we are hovering over and is setting the messsage to its itemName if one is in it
    /// </summary>
    /// <param name="pointerEventData"></param>
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (_invSlot.GetComponent<InventorySlot>().itemInSlot != null)
            msg = _invSlot.GetComponent<InventorySlot>().itemInSlot.itemName.ToString();
        else msg = "";
            TooltipManager.Instance.SetAndShowTooltip(msg);
    }

    /// <summary>
    /// clears the message and stops showing us the tooltip when exiting
    /// </summary>
    /// <param name="pointerEventData"></param>
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        TooltipManager.Instance.ClearAndHideTooltip();
    }
}
