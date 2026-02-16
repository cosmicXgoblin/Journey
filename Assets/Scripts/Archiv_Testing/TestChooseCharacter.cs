using UnityEditor.Build.Content;
using UnityEngine;

public class TestChooseCharacter : MonoBehaviour
{
    //public bool fighterSelected;
    //public bool thiefSelected;
    //public bool sorcererSelected;

    //private GameObject _testFight;
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
    //    //GetComponent<GameManager>().currentClass = GetComponent<GameManager>()._fighterButton;
    //    //_uiManager.GetComponent<UiManager>().OnClickSetCharacter("_fighterButton");
    //    CallSetPlayerData("_fighterButton"); ;
    //}

    //public void OnClickThiefChosen()
    //{
    //    GetComponent<GameManager>().currentClass = GetComponent<GameManager>()._thiefButton;
    //    _uiManager.GetComponent<UiManager>().OnClickSetCharacter("_thiefButton");
    //}

    //public void OnClickSorcererChosen()
    //{
    //    GetComponent<GameManager>().currentClass = GetComponent<GameManager>()._sorcererButton;
    //    _uiManager.GetComponent<UiManager>().OnClickSetCharacter("_sorcererButton");
    //}
}
