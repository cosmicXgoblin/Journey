using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public TextMeshProUGUI TooltipText;

    public static TooltipManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        //transform.position = Input.mousePosition;
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -20);

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
