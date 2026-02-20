
using Ink.Runtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Ink Story")]
    [SerializeField] private TextAsset _inkJson;
    private Story story;
    private int currentChoiceIndex = -1;
    private bool dialoguePlaying = false;
    //[SerializeField] private string _tempKnotName;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject _submitButton;
    [SerializeField] private TextMeshProUGUI _dialogBoxText;
    [SerializeField] private ChoiceButton[] choiceButtons;
    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private Image _speakerPortrait;
    [SerializeField] private Image _background;

    [Header("Story Tags")]
    private const string _SPEAKER_TAG = "speaker";
    private const string _PORTRAIT_TAG = "portrait";
    private const string _BG_TAG = "background";
    private const string _DO = "do";

    public Image background => _background;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        story = new Story(_inkJson.text);
    }

    #region Dialogue

    public void CallDialogue(string knotName)
    {
        EnterDialogue(knotName);
    }
   
    public void EnterDialogue(string knotName)
    {
        if (dialoguePlaying) return;

        dialoguePlaying = true;

        story.BindExternalFunction("toggleGoldDialogue", (bool goldUiOpen) =>
        {
            Debug.Log("Should the Gold UI be open right now? " + goldUiOpen);
            UiManager.Instance.ToggleGoldDialogue(goldUiOpen);
        });
        story.BindExternalFunction("buyItem", (string item, string goldValue)  =>
        {
            Debug.Log(item + " was bought for " + goldValue + "G.");
            int goldValueINT = Convert.ToInt32(goldValue);
            GameManager.Instance.BuyItem(item, goldValueINT);
        });
        story.BindExternalFunction("startFight", (string enemy) =>
        {
            Debug.Log("Starting a fight with" +  enemy);
            GameManager.Instance.SetBattle(enemy);
        });
        story.BindExternalFunction("setClass", (string currenClass) =>
        {
            Debug.Log("Selecting class: " + currenClass);
            GameManager.Instance.SetCurrentClass(currenClass);
            GameManager.Instance.SetPlayerData();
        });
        if (!knotName.Equals(""))
            story.ChoosePathString(knotName);
        else Debug.LogWarning("Knot name was empty when entering dialogue.");

        //start story
        ContinueOrExitStory();
    }
    
    private void ContinueOrExitStory()
    {
        // make a choice if you have to
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            // reset for future
            currentChoiceIndex = -1;
        }

        if (story.canContinue)
        {
            string dialogueLine = story.Continue();

            //Debug.Log(dialogueLine);
            //_dialogBoxText.text = dialogueLine;

            //EventsManager.Instance.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices);
            DisplayDialogue(dialogueLine, story.currentChoices);
            HandleTags(story.currentTags);
        }
        else if (story.currentChoices.Count == 0)
            ExitDialogue();
    }
   
    private void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
    {
        _dialogBoxText.text = dialogueLine;

        // defensive checks
        if (dialogueChoices.Count > choiceButtons.Length)
        {
            Debug.LogError("More dialogue choices ("
                + dialogueChoices.Count + ") came through than are supported ("
                + choiceButtons.Length + ").");
        }

        // start with all buttons hidden
        foreach (ChoiceButton choiceButton in choiceButtons)
            choiceButton.gameObject.SetActive(false);

        // enable & set info for buttons depending on ink choice information
        int choiceButtonIndex = dialogueChoices.Count - 1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            ChoiceButton choiceButton = choiceButtons[choiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if (inkChoiceIndex == 0)
            {
                choiceButton.SelectButton();
                DialogueManager.Instance.UpdateChoiceIndex(1);
            }
            choiceButtonIndex--;
        }
    }
   
    private void ExitDialogue()
    {
        Debug.Log("Exiting Dialogue");

        story.UnbindExternalFunction("toggleGoldDialogue");
        story.UnbindExternalFunction("buyItem");
        story.UnbindExternalFunction("startFight");
        story.UnbindExternalFunction("setClass");

    dialoguePlaying = false;

        // clear state for future dialogues
        story.ResetState();
    }    
   
    #endregion

    #region Choices
    public void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }

    private void SubmitPressed(string knotName)
    {
        if (!dialoguePlaying)
            return;
        else ContinueOrExitStory();
        
    }
    
    public void OnClickSubmit()
    {
        Submit();
    }
   
    public void CallSubmit()
    {
        Submit();
    }

    private void Submit()
    {
        if (!dialoguePlaying)
            return;
        else
        {
            ContinueOrExitStory();
        }
    }
    #endregion

    #region Tags & Variables

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            // parse it
            string[] splitTag = tag.Split(":");
            if (splitTag.Length != 2)
                Debug.LogError("This tag is not correctly set up and could not be appropriately parsed: " + tag);
            string tagKey = splitTag[0].Trim();     // Trim() is cleaning up whitespeace around it
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case _SPEAKER_TAG:
                    _speakerNameText.text = tagValue;
                    break;
                case _PORTRAIT_TAG:
                    SetSpeakerPortrait(tagValue);
                    break;
                case _BG_TAG:
                    SetBackground(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag could not be handled: " + tag);
                    break;
            }
        }
    }

    private void SetSpeakerPortrait(string tagValue)
    {
        switch (tagValue)
        {
            case "Narrator":
                _speakerPortrait.sprite = Database.Instance.Narrator;
                break;
            case "Shopkeeper":
                _speakerPortrait.sprite = Database.Instance.Shopkeeper;
                break;
            case "Tavernkeeper":
                _speakerPortrait.sprite = Database.Instance.Tavernkeeper;
                break;
            case "Fighter":
                _speakerPortrait.sprite = Database.Instance.Fighter;
                break;
            case "Thief":
                _speakerPortrait.sprite = Database.Instance.Thief;
                break;
            case "Sorcerer":
                _speakerPortrait.sprite = Database.Instance.Sorcerer;
                break;
            default:
                Debug.LogWarning("TagValue could not be handled: " + tagValue);
                break;
        }
    }

    private void SetBackground(string tagValue)
    {
        switch (tagValue)
        {
            case "Shop":
                _background.sprite = Database.Instance.Shop;
                break;
            case "Tavern":
                _background.sprite = Database.Instance.None;
                break;
            case "Village":
                _background.sprite = Database.Instance.Village;
                break;
            case "None":
                _background.sprite = Database.Instance.None;
                break;
            default:
                Debug.LogWarning("TagValue could not be handled: " + tagValue);
                break;
        }
    }

    #endregion

}
