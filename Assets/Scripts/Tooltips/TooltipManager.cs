using UnityEngine;
using TMPro;

/// <summary>
/// responsible for showing tooltips in the inventorySlot on mouseover
/// </summary>
public class TooltipManager : MonoBehaviour
{
    public TextMeshProUGUI TooltipText;

    public static TooltipManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }


    private void Update()
    {
        transform.position = Input.mousePosition;
    }


    public void SetAndShowTooltip(string tooltip)
    {
        TooltipText.text = tooltip;
        if (tooltip != "")
            gameObject.SetActive(true);
        
    }

    public void ClearAndHideTooltip()
    {
        gameObject.SetActive(false);
        TooltipText.text = "";
    }
}
