using UnityEditor.Build.Content;
using UnityEngine;

public class TestChooseCharacter : MonoBehaviour
{
    //public bool fighterSelected;
    //public bool thiefSelected;
    //public bool sorcererSelected;

    //private GameObject testFight;
    //private Enemy currentEnemy;
    private ScriptableObject currentClass;
    private ScriptableObject currentEnemy;

    [SerializeField] private GameObject uiManager;

    private void Awake()
    {
        //fighterSelected = false;
        //thiefSelected = false;
        //sorcererSelected = false;

        //currentClass = GetComponent<GameManager>().currentClass;
        //currentEnemy = GetComponent<GameManager>().currentEnemy;

    }

    //// make this one
    //public void OnClickFighterChosen()
    //{
    //    //GetComponent<GameManager>().currentClass = GetComponent<GameManager>().fighter;
    //    //uiManager.GetComponent<TestUiManager>().OnClickSetCharacter("fighter");
    //    CallSetPlayerData("fighter"); ;
    //}

    //public void OnClickThiefChosen()
    //{
    //    GetComponent<GameManager>().currentClass = GetComponent<GameManager>().thief;
    //    uiManager.GetComponent<TestUiManager>().OnClickSetCharacter("thief");
    //}

    //public void OnClickSorcererChosen()
    //{
    //    GetComponent<GameManager>().currentClass = GetComponent<GameManager>().sorcerer;
    //    uiManager.GetComponent<TestUiManager>().OnClickSetCharacter("sorcerer");
    //}
}
