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

        //currentClass = GetComponent<TestFight>().currentClass;
        //currentEnemy = GetComponent<TestFight>().currentEnemy;

    }

    // make this one
    public void OnClickFighterChosen()
    {
        GetComponent<TestFight>().currentClass = GetComponent<TestFight>().fighter;
        uiManager.GetComponent<TestUiManager>().OnClickSetCharacter("fighter");
    }

    public void OnClickThiefChosen()
    {
        GetComponent<TestFight>().currentClass = GetComponent<TestFight>().thief;
        uiManager.GetComponent<TestUiManager>().OnClickSetCharacter("thief");
    }

    public void OnClickSorcererChosen()
    {
        GetComponent<TestFight>().currentClass = GetComponent<TestFight>().sorcerer;
        uiManager.GetComponent<TestUiManager>().OnClickSetCharacter("sorcerer");
    }
}
