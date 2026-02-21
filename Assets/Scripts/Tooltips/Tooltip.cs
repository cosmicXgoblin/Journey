using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string msg;
    [SerializeField] private InventorySlot _invSlot;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        msg = _invSlot.GetComponent<InventorySlot>().itemInSlot.itemName.ToString();
        TooltipManager.Instance.SetAndShowTooltip(msg);
    }

    public void OnPointerExit(PointerEventData pointerEventData)

    {
        TooltipManager.Instance.ClearAndHideTooltip();
    }
}
