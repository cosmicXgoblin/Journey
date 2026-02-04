using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class TestUiManager : MonoBehaviour
{
    [SerializeField] private GameObject testFight;
    [SerializeField] private GameObject testChooseCharacter;
    [SerializeField] private GameObject testMap;
    [SerializeField] private GameObject screenUi;

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
    [SerializeField] private GameObject manaBarObject;

    [SerializeField] private Sprite classMapfigur;

    void Start()
    {
        testFight.SetActive(false);
        testMap.SetActive(false);
        screenUi.SetActive(true);                       // ScreenUi and characterPanel need to be in this order!
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
            classImage.sprite = gameManager.GetComponent<GameManager>().fighter.classSpriteRound;
            classMapfigur = gameManager.GetComponent<GameManager>().fighter.classMapfigure_Base;
            className.text = gameManager.GetComponent<GameManager>().fighter.className;
            healthBar.value = gameManager.GetComponent<GameManager>().fighter.hitPoints;
            manaBarObject.SetActive(false);

            playerMapfigure.GetComponent<SpriteRenderer>().sprite = classMapfigur;
        }

        if (selectedClass == "thief")
        {
            classImage.sprite = gameManager.GetComponent<GameManager>().thief.classSpriteRound;
            classMapfigur = gameManager.GetComponent<GameManager>().thief.classMapfigure_Base;
            className.text = gameManager.GetComponent<GameManager>().thief.className;
            healthBar.value = gameManager.GetComponent<GameManager>().thief.hitPoints;
            manaBarObject.SetActive(false);

            playerMapfigure.GetComponent<SpriteRenderer>().sprite = classMapfigur;
        }

        if (selectedClass == "sorcerer")
        {
            classImage.sprite = gameManager.GetComponent<GameManager>().sorcerer.classSpriteRound;
            classMapfigur = gameManager.GetComponent<GameManager>().sorcerer.classMapfigure_Base;
            className.text = gameManager.GetComponent<GameManager>().sorcerer.className;
            healthBar.value = gameManager.GetComponent<GameManager>().sorcerer.hitPoints;
            manaBar.value = 100;

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
       // gameManager.GetComponent<GameManger>().playerData
    }
}
