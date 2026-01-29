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

    private void Awake()
    {
        //fighterSelected = false;
        //thiefSelected = false;
        //sorcererSelected = false;

        //currentClass = GetComponent<TestFight>().currentClass;
        //currentEnemy = GetComponent<TestFight>().currentEnemy;
    }

    public void OnClickFighterChosen()
    {
        GetComponent<TestFight>().currentClass = GetComponent<TestFight>().fighter;
    }

    public void OnClickThiefChosen()
    {
        GetComponent<TestFight>().currentClass = GetComponent<TestFight>().thief;
    }

    public void OnClickSorcererChosen()
    {
        GetComponent<TestFight>().currentClass = GetComponent<TestFight>().sorcerer;
    }
}
