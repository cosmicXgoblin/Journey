using System;
using UnityEngine;

public class InputEvent
{
    public event Action<string> OnSubmitPressed;


    //public void SubmitPressed(string dialogueKnotName)
    //{
    //    //if (!playerIsNear)
    //    //    return;

    //    //if we have a knot, try to start its dialogue
    //    if (!dialogueKnotName.Equals(""))
    //        EventsManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
    //   // if we don't have a knot, start or finish the quest
    //    else
    //        Debug.Log("Quest started / finished");
    //    }
}
