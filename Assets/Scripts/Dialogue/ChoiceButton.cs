using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour, ISelectHandler
{
    [Header("Components")]
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _choiceText;

    private int choiceIndex = -1;

    public void SetChoiceText(string choiceTextString)
    {
        _choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }

    public void SelectButton()
    {
        _button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
       EventsManager.Instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }
}
