using Ink.Runtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Ink Story")]
    [SerializeField] private TextAsset _inkJson;
    private Story story;
    private int currentChoiceIndex = -1;

    [SerializeField] private GameObject _submitButton;
    [SerializeField] private TextMeshProUGUI _dialogBoxText;
    private bool dialoguePlaying = false;
    [SerializeField] private string _tempKnotName;

    [SerializeField] private ChoiceButton[] choiceButtons;

    private void Awake()
    { 
        if (Instance == null) Instance = this;
        else Debug.LogError("Found more than one Event Manager in the scene.");

        story = new Story(_inkJson.text);

    }

    private void OnEnable()
    {
        //EventsManager.Instance.dialogueEvents.OnEnterDialogue += EnterDialogue;
        EventsManager.Instance.inputEvents.OnSubmitPressed += SubmitPressed;
        //EventsManager.Instance.dialogueEvents.OnUpdateChoiceIndex += UpdateChoiceIndex;
    }

    private void OnDisable()
    {
       // EventsManager.Instance.dialogueEvents.OnEnterDialogue -= EnterDialogue;
        EventsManager.Instance.inputEvents.OnSubmitPressed -= SubmitPressed;
       // EventsManager.Instance.dialogueEvents.OnUpdateChoiceIndex += UpdateChoiceIndex;
    }

    public void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }


    private void SubmitPressed(string knotName)
    {
        if (!dialoguePlaying)
            return;
        else ContinueOrExitStory();
        
        _tempKnotName = knotName;
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



    public void CallDialogue(string knotName)
    {
        EnterDialogue(knotName);
        //_tempKnotName = knotName;
    }


    public void EnterDialogue(string knotName)
    {
        if (dialoguePlaying) return;

        dialoguePlaying = true;

        //Debug.Log("Entering dialogue for knotName: " + knotName);
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

        dialoguePlaying = false;

        // clear state for future dialogues
        story.ResetState();
        _tempKnotName = "";
    }

    //public void UpdateChoiceIndex(int _choiceIndex)
    //{
    //    if (OnUpdateChoiceIndex != null)
    //        OnUpdateChoiceIndex(_choiceIndex);
    //}

}
