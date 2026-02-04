using TMPro;
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
    [SerializeField] private Button fighter;
    [SerializeField] private Button thief;
    [SerializeField] private Button sorcerer;

    [Header("UI Charactersheet")]
    [SerializeField] private GameObject playerMapfigure;
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private Image classImage;
    [SerializeField] private TextMeshProUGUI className;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider manaBar;

    [SerializeField] private Sprite classMapfigur;

    void Start()
    {
        testFight.SetActive(false);
        testMap.SetActive(false);
        characterPanel.SetActive(false);
        testChooseCharacter.SetActive(true);
    }

    public void OnClickGoFight()
    {
        testChooseCharacter.SetActive(false);
        testFight.SetActive(true);
        characterPanel.SetActive(false);
    }

    public void OnClickGoAdventure()
    {
        testChooseCharacter.SetActive(false);
        testMap.SetActive(true);
        characterPanel.SetActive(true);
    }

    public void OnClickSetCharacter(string selectedClass)
    {
        if (selectedClass == "fighter")
        {
            classImage.sprite = gameManager.GetComponent<TestFight>().fighter.classSpriteRound;
            classMapfigur = gameManager.GetComponent<TestFight>().fighter.classMapfigure_Base;
            className.text = gameManager.GetComponent<TestFight>().fighter.className;
            healthBar.value = gameManager.GetComponent<TestFight>().fighter.hitPoints;
            manaBar.value = 0;

            playerMapfigure.GetComponent<SpriteRenderer>().sprite = classMapfigur;
        }

        if (selectedClass == "sorcerer")
        {
            classImage.sprite = gameManager.GetComponent<TestFight>().sorcerer.classSpriteRound;
            classMapfigur = gameManager.GetComponent<TestFight>().sorcerer.classMapfigure_Base;
            className.text = gameManager.GetComponent<TestFight>().sorcerer.className;
            healthBar.value = gameManager.GetComponent<TestFight>().sorcerer.hitPoints;
            manaBar.value = 100;

            playerMapfigure.GetComponent<SpriteRenderer>().sprite = classMapfigur;
        }

        if (selectedClass == "thief")
        {
            classImage.sprite = gameManager.GetComponent<TestFight>().thief.classSpriteRound;
            classMapfigur = gameManager.GetComponent<TestFight>().thief.classMapfigure_Base;
            className.text = gameManager.GetComponent<TestFight>().thief.className;
            healthBar.value = gameManager.GetComponent<TestFight>().thief.hitPoints;
            manaBar.value = 0;

            playerMapfigure.GetComponent<SpriteRenderer>().sprite = classMapfigur;
        }

    }


    public void Fight()
    {
        testMap.SetActive(false);
        testFight.SetActive(true);
        characterPanel.SetActive(false);
    }

    public void CallBackToMap()
    {
        testMap.SetActive(true);
        testFight.SetActive(false);
        characterPanel.SetActive(true);
    }

    public void UpdateUI()
    {
        //healthBar.value = 0;
        //manaBar.value = 0;
    }
}
