using UnityEngine;
using System;
using UnityEngine.UIElements;


public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance {  get; private set; }

    public InputEvent inputEvents;

    public DialogueEvents dialogueEvents;

    // just for working atm
    public UiManager uiManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Found more than one Event Manager in the scene.");
        //    if (Instance != null)
        //        Debug.LogError("Foudn more than one Event Manager in the scene.");
        //    else Instance = this;

        inputEvents = new InputEvent();
        dialogueEvents = new DialogueEvents();
    }
}
