using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _choiceText;

    private int _choiceIndex = -1;

    public void SetChoiceText(string choiceTextString)
    {
        _choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this._choiceIndex = choiceIndex;
    }

    public void SelectButton()
    {
        _button.Select();
    }

    public void OnClickSelect()
    {
        DialogueManager.Instance.UpdateChoiceIndex(_choiceIndex);
    }
}
