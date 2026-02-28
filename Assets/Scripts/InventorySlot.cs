using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Slot UI")]
    [SerializeField] private GameObject _invSlotSprite;

    [Header("Slot Attributes")]
    public InventorySlot inventorySlot;
    [SerializeField] private Item _itemInSlot;

    [Header("Item Attributes")]
    public string itemName;
    [SerializeField] private Image _itemImage;
    [SerializeField] private string itemDescription;

    public Item itemInSlot => _itemInSlot;

    #region init
    private void Awake()
    {
        ToggleInventorySlotSprite();
        UpdateInventorySlot();
    }
    #endregion

    public void AddItem(Item itemToAdd)
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
        itemDescription = _itemInSlot.description;

        ToggleInventorySlotSprite();
    }

    public void OnClickInventorySlot()
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
        _itemImage.sprite = null;
        itemName = "";
        itemDescription = "";
        _itemInSlot = null;

        ToggleInventorySlotSprite();
    }

    /// <summary>
    /// only shows us the sprite if there is an item (else we see an ugly white square)
    /// </summary>
    private void ToggleInventorySlotSprite()
    {
        if (_itemInSlot == null)
            _invSlotSprite.SetActive(false);
        else _invSlotSprite.SetActive(true);
    }
}
