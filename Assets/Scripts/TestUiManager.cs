using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class TestUiManager : MonoBehaviour
{
    [SerializeField] private GameObject testFight;
    [SerializeField] private GameObject testChooseCharacter;
    [SerializeField] private GameObject testMap;

    [SerializeField] private GameObject gameManager;

    [Header("Characterselection")]
    [SerializeField] private Button Fighter;
    [SerializeField] private Button Thief;
    [SerializeField] private Button Sorcerer;



    void Start()
    {
        testFight.SetActive(false);
        testMap.SetActive(false);
        testChooseCharacter.SetActive(true);
    }

    public void OnClickGoFight()
    {
        testChooseCharacter.SetActive(false);
        testFight.SetActive(true);
    }

    public void OnClickGoAdventure()
    {
        testChooseCharacter.SetActive(false);
        testMap.SetActive(true);
        //testFight.SetActive(false);
    }

    public void Fight()
    {
        testMap.SetActive(false);
        testFight.SetActive(true);
    }

    public void CallBackToMap()
    {
        testMap.SetActive(true);
        testFight.SetActive(false);
    }




}
