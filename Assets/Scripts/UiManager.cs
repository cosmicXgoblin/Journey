using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UiManager : MonoBehaviour
{
    #region Inspector
    [Header("Manager")]
    [SerializeField] private GameObject _playerController;
    
    [Header("Title Menu")]
    [SerializeField] private GameObject _canvasTitle;
    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private GameObject _titlePanelButtons;
    [SerializeField] private GameObject _titlePanelOptions;
    [SerializeField] private GameObject _titlePanelNewGame;
    [SerializeField] private GameObject _titleButtonStartGame;
    [SerializeField] private GameObject _titlePanelLoadGame;


    [Header("Screens & Panels")]
    [SerializeField] private GameObject _testFight;
    [SerializeField] private GameObject _testChooseCharacter;
    [SerializeField] private GameObject _testMap;
    [SerializeField] private GameObject _screenUi;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _pausePanelMain;
    [SerializeField] private GameObject _pausePanelOptions;
    [SerializeField] private GameObject _dialogueAndChoicesPanel;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private GameObject _choicesPanel;
    [SerializeField] private GameObject _chooseTutorialPanel;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _consumablePanel;

    [Header("Persistence")]
    public string inputFileName;

    [Header("Characterselection")]
    [SerializeField] private Button _fighterButton;
    [SerializeField] private Button _thiefButton;
    [SerializeField] private Button _sorcererButton;

    [Header("UI Charactersheet")]
    [SerializeField] private GameObject _characterPanel;
    [SerializeField] private GameObject _playerMapfigure;
    [SerializeField] private Image _classImage;
    [SerializeField] private TextMeshProUGUI _className;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private TextMeshProUGUI _healthBarText;
    [SerializeField] private Slider _manaBar;
    [SerializeField] private TextMeshProUGUI _manaBarText;
    [SerializeField] private GameObject _manaBarObject;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private TextMeshProUGUI _goldText;

    [Header("Fight UI")]
    public Image enemyImage;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyAttackText;
    public TextMeshProUGUI enemyHitPointsText;
    public Image classImage;
    public TextMeshProUGUI className;
    public TextMeshProUGUI classAttackText;
    public TextMeshProUGUI classHitPointsText;
    //public int CurrentPlayerHitPoints;
    public TextMeshProUGUI classAttackModifierText;
    public GameObject playerAttackButton;
    public TextMeshProUGUI fightText;
    public TextMeshProUGUI whichRoundText;
    private Image _target;

    [Header("UI Charactapperance (game)")]
    [SerializeField] private Sprite _classMapfigure_Base;
    [SerializeField] private Sprite _classMapfigure_noBase;
    [SerializeField] private bool _noBase;

    [SerializeField] private GameObject _dialogueGold;
    [SerializeField] private TextMeshProUGUI _dialogueGoldText;

    //[Header("Effects")]
    //private Color _tempColor;
    //public Color red;
    //public Color original;
    #endregion

    #region Init
    public static UiManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }
    void Start()
    {
        SetEverythingInactive();

        _canvasTitle.SetActive(true);
        _titlePanel.SetActive(true);
        _titlePanelButtons.SetActive(true);
        _titlePanelOptions.SetActive(false);

        //original.a = 0.5f;
    }
    #endregion

    // will get called when clicking on a button
    #region OnClick
    public void OnClickGoFight()
    {
        _testChooseCharacter.SetActive(false);
        _testFight.SetActive(true);
        _characterPanel.SetActive(false);
    }

    public void OnClickGoAdventure()
    {
        _testChooseCharacter.SetActive(false);
        _testMap.SetActive(true);
        _screenUi.SetActive(true);
        _characterPanel.SetActive(true);
    }

    public void OnClickContinue()
    {
        _pausePanel.SetActive(false);
        EnableUiMap(true);
    }

    public void OnClickOptionsPause()
    {
        _pausePanelOptions.SetActive(true);
        _pausePanelMain.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_pausePanelOptions.transform.GetChild(0).gameObject);
    }

    public void OnClickOptionsMain()
    {
        _titlePanelOptions.SetActive(true);
        _titlePanelButtons.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_titlePanelOptions.transform.GetChild(0).gameObject);
    }

    // not in use
    //public void OnClickPauseMenu()
    //{
    //    CallPause();
    //}
    
    public void OnClickNewGame()
    {
        _titlePanel.SetActive(false);
        _titlePanelNewGame.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_titleButtonStartGame);
    }

    public void OnClickStartNewGame()
    {
        //DataPersistenceManager.Instance.NewGame();

        DataPersistenceManager.Instance.CallSelectFilename(inputFileName);

        _titlePanelNewGame.SetActive(false);
        _chooseTutorialPanel.SetActive(false);
        _testChooseCharacter.SetActive(true);
        _inventoryPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_fighterButton.gameObject);
    }

    public void OnClickOpenTutorialQuestion()
    {
        _titlePanel.SetActive(false);
        _chooseTutorialPanel.SetActive(true);
    }

    public void OnClickStartTutorial()
    {
        SetEverythingInactive();

        _screenUi.SetActive(true);
        _dialoguePanel.SetActive(true);
        _dialogueAndChoicesPanel.SetActive(true);
    }

    public void OnClickLoadGamePart1()
    {
        _titlePanelLoadGame.SetActive(true);
        _titlePanel.SetActive(false);
    }
    public void OnClickLoadGamePart2()
    {
        DataPersistenceManager.Instance.LoadGame();
    }

    public void OnClickSavegame()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void OnClickBackToTitle()
    {
        _titlePanelNewGame.SetActive(false);
        _titlePanelOptions.SetActive(false);
        _chooseTutorialPanel.SetActive(false);
        _titlePanel.SetActive(true);
        _titlePanelButtons.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_titlePanelButtons.transform.GetChild(0).gameObject);
    }
    
    public void OnClickToggleMapfigureBase()
    {
        _noBase = !_noBase;

        if (!_noBase)
            _playerMapfigure.GetComponent<SpriteRenderer>().sprite = _classMapfigure_Base;
        else _playerMapfigure.GetComponent<SpriteRenderer>().sprite = _classMapfigure_noBase;
    }

    public void OnClickSetCharacter(string selectedClass)
    {
        switch (selectedClass)
        {
            case "fighter":
                _classImage.sprite = Database.Instance.fighter.classSpriteRound;
                _classMapfigure_Base = Database.Instance.GetComponent<Database>().fighter.classMapfigure_Base;
                _classMapfigure_noBase = Database.Instance.GetComponent<Database>().fighter.classMapfigure_noBase;
                _className.text = Database.Instance.GetComponent<Database>().fighter.className;
                _healthBar.value = Database.Instance.GetComponent<Database>().fighter.maxHitPoints;
                _healthBar.maxValue = Database.Instance.GetComponent<Database>().fighter.maxHitPoints;
                _manaBarObject.SetActive(false);
                break;
            case "thief":
                _classImage.sprite = Database.Instance.GetComponent<Database>().thief.classSpriteRound;
                _classMapfigure_Base = Database.Instance.GetComponent<Database>().thief.classMapfigure_Base;
                _classMapfigure_noBase = Database.Instance.GetComponent<Database>().thief.classMapfigure_noBase;
                _className.text = Database.Instance.GetComponent<Database>().thief.className;
                _healthBar.value = Database.Instance.GetComponent<Database>().thief.maxHitPoints;
                _healthBar.maxValue = Database.Instance.GetComponent<Database>().thief.maxHitPoints;
                _manaBarObject.SetActive(false);
                break;
            case "sorcerer":
                _classImage.sprite = Database.Instance.GetComponent<Database>().sorcerer.classSpriteRound;
                _classMapfigure_Base = Database.Instance.GetComponent<Database>().sorcerer.classMapfigure_Base;
                _classMapfigure_noBase = Database.Instance.GetComponent<Database>().sorcerer.classMapfigure_noBase;
                _className.text = Database.Instance.GetComponent<Database>().sorcerer.className;
                _healthBar.value = Database.Instance.GetComponent<Database>().sorcerer.maxHitPoints;
                _healthBar.maxValue = Database.Instance.GetComponent<Database>().sorcerer.maxHitPoints;
                _manaBar.value = 100;
                _manaBarObject.SetActive(true);
                break;
        }
        _playerMapfigure.GetComponent<SpriteRenderer>().sprite = _classMapfigure_Base;

        UpdateUi(GameManager.Instance.CurrentPlayerHitPoints);
    }

    public void EnableUiMap(bool enable)
    {
        _playerController.GetComponent<PlayerController>().uiMap.Enable();
        _playerController.GetComponent<PlayerController>().playerMap.Disable();
    }
    #endregion

    #region Calls
    public void CallBackToMap()
    {
        _testMap.SetActive(true);
        _testFight.SetActive(false);
        _characterPanel.SetActive(true);
    }

    public void CallPause()
    {
        if (!_pausePanel.activeInHierarchy)
        {
            _pausePanel.SetActive(true);
            _pausePanelOptions.SetActive(false);
            _pausePanelMain.SetActive(true);

            EnableUiMap(true);

            EventSystem.current.SetSelectedGameObject(_pausePanelMain.transform.GetChild(0).gameObject);
        }
        else
        {
            _pausePanel.SetActive(false);
            _pausePanelOptions.SetActive(false);
            _pausePanelMain.SetActive(true);

            EnableUiMap(false);
        }
    }
    
    public void CallDialogueUI()
    {
        _playerController.SetActive(false);

        _dialogueAndChoicesPanel.SetActive(true);
        _dialoguePanel.SetActive(true);
        _dialogueGold.SetActive(false);

        EnableUiMap(true);
    }
    #endregion

    #region Fight
    public void ShowFightUI()
    {
        _testMap.SetActive(false);
        _dialogueAndChoicesPanel.SetActive(false);
        _characterPanel.SetActive(false);
        _testFight.SetActive(true);
    }

    public void ClearFightUI()
    {
        className.text = "Name";
        enemyName.text = "Name";
        classAttackText.text = "Attack";
        enemyAttackText.text = "Attack";
        classHitPointsText.text = "Hitpoints";
        enemyHitPointsText.text = "Hitpoints";
        fightText.text = "";
        whichRoundText.text = "";
}

    public void CallSetFightUI(ScriptableObject currentClass, ScriptableObject currentEnemy)
    {
        if (currentEnemy != null)
        {
            if (currentEnemy == Database.Instance.rat)
            {
                enemyAttackText.text = Database.Instance.rat.attack.ToString();
                enemyImage.sprite = Database.Instance.rat.enemySprite;
                enemyHitPointsText.text = Database.Instance.rat.hitPoints.ToString();
            }
            if (currentEnemy == Database.Instance.rat1)
            {
                enemyAttackText.text = Database.Instance.rat1.attack.ToString();
                enemyImage.sprite = Database.Instance.rat1.enemySprite;
                enemyHitPointsText.text = Database.Instance.rat1.hitPoints.ToString();
            }
            if (currentEnemy == Database.Instance.rat2)
            { 
                enemyAttackText.text = Database.Instance.rat2.attack.ToString();
                enemyImage.sprite = Database.Instance.rat2.enemySprite;
                enemyHitPointsText.text = Database.Instance.rat2.hitPoints.ToString();
            }
        }
        if (currentClass != null)
        {
            if (currentClass == Database.Instance.fighter)
            {
                classAttackText.text = Database.Instance.fighter.attack.ToString();
                classImage.sprite = Database.Instance.fighter.classSprite;
                classHitPointsText.text = Database.Instance.fighter.maxHitPoints.ToString();
            }
            if (currentClass == Database.Instance.thief)
            {
                classAttackText.text = Database.Instance.thief.attack.ToString();
                classImage.sprite = Database.Instance.thief.classSprite;
            }
            if (currentClass == Database.Instance.sorcerer)
            {
                classAttackText.text = Database.Instance.sorcerer.attack.ToString();
                classImage.sprite = Database.Instance.thief.classSprite;
            }

            classAttackModifierText.text = GameManager.Instance.classAttackModifier.ToString();
        }
    }

    //public IEnumerator ImageEffect(bool enemy, float delay, string color)
    //{
    //    if (enemy)
    //        _target = enemyImage.GetComponent<Image>();
    //    else if (!enemy)
    //        _target = classImage.GetComponent<Image>();


    //    if (color == "red")
    //    {
    //        red.a = 1f;
    //        _tempColor = red;
    //        _target.color = _tempColor;
    //        Debug.Log("Image should be red for a moment");
    //    }
    //    yield return new WaitForSeconds(delay);
    //    _target.color = original;

    //    //_target.color = new Color(0f, 0f, 0f, 1f);
    //    //yield return new WaitForSeconds(delay);
    //    //_target.color = new Color(1f, 1f, 1f, 1f);
    //}

    #endregion

    #region Dialogue

    public void ToggleGoldDialogue(bool goldUiOpen)
    {
        int playerGold = GameManager.Instance.PlayerData.gold;
        UpdateUI(playerGold);

        if (goldUiOpen)
        {
            _dialogueGold.SetActive(true);
            _inventoryPanel.SetActive(true);
        }
        else
        {
            _dialogueGold.SetActive(false);
            _inventoryPanel.SetActive(false);
        }
    }
    #endregion

    #region Items
    public void OpenConsumableUI()
    {
            _consumablePanel.SetActive(true);
    }

    public void OnClickCloseConsumableUI()
    {
        CloseConsumableUI();
    }

    public void CloseConsumableUI()
    {
        _consumablePanel.SetActive(false);
    }

    #endregion


    public void UpdateUi(int currentPlayerHitPoints)        
    {
        _healthBar.value = currentPlayerHitPoints;
        _healthBarText.text = currentPlayerHitPoints.ToString() + " / " +  GameManager.Instance.PlayerData.maxHitPoints.ToString();

        enemyHitPointsText.text = GameManager.Instance.CurrentEnemyHitPoints.ToString();
        classHitPointsText.text = GameManager.Instance.CurrentPlayerHitPoints.ToString();

        whichRoundText.text = GameManager.Instance.Round.ToString();

        //GameManager.Instance..GetComponent<GameManger>().playerData;
    }

    public void UpdateUI (int playerGold)
    {
        _goldText.text = playerGold.ToString() + "G";
        _dialogueGoldText.text = playerGold.ToString() + "G";
    }
    
    public void ReadStringInput(string s)
    {
        inputFileName = s + ".journey";
        //DataPersistenceManager.Instance.CallSelectFilename(inputFileName);
    }

    private void SetEverythingInactive()
    {
        _testFight.SetActive(false);
        _testMap.SetActive(false);
        _screenUi.SetActive(true);
        _characterPanel.SetActive(false);
        _testChooseCharacter.SetActive(false);
        _pausePanel.SetActive(false);
        _titlePanelNewGame.SetActive(false);
        _inventory.SetActive(false);
        _screenUi.SetActive(false);
        _titlePanelLoadGame.SetActive(false);
        _dialogueAndChoicesPanel.SetActive(false); ;
        _dialoguePanel.SetActive(false);
        _chooseTutorialPanel.SetActive(false);
        _consumablePanel.SetActive(false);
        _dialogueGold.SetActive(false);
        _canvasTitle.SetActive(false);
        _titlePanel.SetActive(false);
        _titlePanelButtons.SetActive(false);
        _titlePanelOptions.SetActive(false);
    }
}
