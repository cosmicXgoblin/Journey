using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Slot Attributes")]
    public InventorySlot inventorySlot;
    [SerializeField] private Item _itemInSlot;
    [SerializeField] private GameObject _invSlotSprite;
    //public int slotIndex;


    [Header("Item Attributes")]
    public string itemName;
    private string itemDescription;
    [SerializeField] private Image _itemImage;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _itemNameText;

    public Item itemInSlot => _itemInSlot;

    private void Awake()
    {
        ToggleInventorySlotSprite();
        UpdateInventorySlot();
    }

    private void AddItem(Item itemToAdd)
    {
        _itemInSlot = itemToAdd;

        UpdateInventorySlot();
    }

    private void UpdateInventorySlot()
    {
        if (_itemInSlot == null)
            return;

        _itemImage.sprite = _itemInSlot.itemSprite;
        itemName = _itemInSlot.itemName;
        _itemNameText.text = itemName.ToString();
        itemDescription = _itemInSlot.description;

        ToggleInventorySlotSprite();
    }

    public void OnClick()
    {        
        GameManager.Instance.SetTempItem(itemInSlot, inventorySlot);

        Debug.Log(itemName.ToString() + ": " + itemDescription);

        if (_itemInSlot.consumable == true)
        {
            Item item = _itemInSlot;
            UiManager.Instance.OpenConsumableUI();
        }
    }


    public void ClearSlot()
    {
        _itemInSlot = null;
        _itemImage.sprite = null;
        itemName = null;
        _itemNameText.text = "";
        itemDescription = "";
        _itemImage = null;

        ToggleInventorySlotSprite();
    }

    private void ToggleInventorySlotSprite()
    {
        if (_itemInSlot == null)
            _invSlotSprite.SetActive(false);
        else _invSlotSprite.SetActive(true);
    }
}
