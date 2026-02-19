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
        transform.position = Input.mousePosition;
    }

    public void SetAndShowTooltip(string tooltip)
    {
        gameObject.SetActive(true);
        TooltipText.text = tooltip;
    }

    public void ClearAndHideTooltip()
    {
        gameObject.SetActive(false);
        TooltipText.text = "";
    }
}
