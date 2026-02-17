using UnityEngine;

public class Map_DialogueEvents : MonoBehaviour
{
    [Header("Dialogue (optional)")]
    [SerializeField] private string dialogueKnotName;

    private bool playerIsNear;

    [SerializeField] UiManager uiManager;

    private void SubmitPressed()
    {
        if (!playerIsNear)
            return;

        // if we have a knot, try to start its dialogue
        if (!dialogueKnotName.Equals(""))
            DialogueManager.Instance.EnterDialogue(dialogueKnotName);
        // if we don't have a knot, start or finish the quest
        else
            Debug.Log("Quest started / finished");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            string _tempKnotName = dialogueKnotName;
            uiManager.GetComponent<UiManager>().CallDialogueUI();
            DialogueManager.Instance.GetComponent<DialogueManager>().CallDialogue(dialogueKnotName);
        }

        //EventsManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);


        // if we have a knot, try to start its dialogue
        //if (!dialogueKnotName.Equals(""))

        // if we don't have a knot, start or finish the quest
        // else
        // Debug.Log("Quest started / finished");
    }

    //private void OnTriggerExit2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "Player")
    //        playerIsNear = false;
    //}


}
