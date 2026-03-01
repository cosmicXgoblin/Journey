using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private GameObject _startImage1;
    [SerializeField] private GameObject _startImage2; 
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
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject _cantAddPanel;
    [SerializeField] private TextMeshProUGUI _cantAddText;
    [SerializeField] private GameObject _winLosePanel;

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
    [SerializeField] private GameObject _dialogueGold;
    [SerializeField] private TextMeshProUGUI _dialogueGoldText;

    [Header("Fight UI")]
    public Image enemyImage;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyAttackText;
    public TextMeshProUGUI enemyHitPointsText;
    public Image classImage;
    public TextMeshProUGUI className;
    public TextMeshProUGUI classAttackText;
    public TextMeshProUGUI classHitPointsText;
    public TextMeshProUGUI classAttackModifierText;
    public GameObject playerAttackButton;
    public TextMeshProUGUI fightText;
    public TextMeshProUGUI whichRoundText;
    [SerializeField] TextMeshProUGUI _winLoseText;
    [SerializeField] GameObject _button1;
    [SerializeField] GameObject _button2;
    [SerializeField] GameObject _button3;

    [Header("UI Charactapperance (game)")]
    [SerializeField] private Sprite _classMapfigure_Base;
    [SerializeField] private Sprite _classMapfigure_noBase;
    [SerializeField] private bool _noBase;

    [Header("Persistence")]
    public string inputFileName;

    #endregion 
   
    public static UiManager Instance { get; private set; }

    #region Init

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        SetEverythingInactive();

        StartStartScreen();
    }
    #endregion

    /// <summary>
    /// functions for buttons
    /// </summary>
    #region OnClick
    
    public void OnClickGoTitleMenu()
    {
        SetEverythingInactive();

        _canvasTitle.SetActive(true);
        _titlePanel.SetActive(true);
        _titlePanelButtons.SetActive(true);
        _titlePanelOptions.SetActive(false);
    }

    public void OnClickQuitGame()
    {
        Application.Quit();
    }

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
        _tutorialPanel.SetActive(true);
        Tutorial.Instance.DisableOutlines();
    }

    public void OnClickStartGame()
    {
        SetEverythingInactive();

        _screenUi.SetActive(true);
        _dialoguePanel.SetActive(true);
        _dialogueAndChoicesPanel.SetActive(true);
        _tutorialPanel.SetActive(false);
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
        SetCharacter(selectedClass);
    }

    public void OnClickRestartGame()
    {
        SetEverythingInactive();
        _titlePanel.SetActive(true);
    }

    /// <summary>
    /// depending on the bool, the text and buttons will be set accordingly
    /// </summary>
    /// <param name="battleWon"></param>
    /// <param name="loot"></param>
    public void ShowAndSetWinLoseText(bool battleWon, string loot)
    {
        _winLosePanel.SetActive(true);

        if (battleWon)
        {
            _winLoseText.text = "You won! /n Loot: " + loot;
            _button1.SetActive(false);
            _button2.SetActive(false);
        }
        else
        {
            _winLoseText.text = "You lost! /n";
            _button3.SetActive(false);
            //EndFight();
        }
    }

    /// <summary>
    /// sets up the UI with the sprites according to the choice of class - should really hooked that up with the PlayerData, but it's leftover from test code and rewriting & rerouting...
    /// ... things is taking up too much time now. good lesson for next time tho: rewrite your test code.
    /// </summary>
    /// <param name="selectedClass"></param>
    public void SetCharacter(string selectedClass)
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

        UpdateUiHP(GameManager.Instance.CurrentPlayerHitPoints);
    }

    /// <summary>
    /// enables or diables action maps. couldn't tell you why i thought it would be great in UiManager tho
    /// </summary>
    /// <param name="enable"></param>
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
    public void UpdateUiHP(int currentPlayerHitPoints)        
    {
        _healthBar.value = currentPlayerHitPoints;
        _healthBarText.text = currentPlayerHitPoints.ToString() + " / " +  GameManager.Instance.PlayerData.maxHitPoints.ToString();

        enemyHitPointsText.text = GameManager.Instance.CurrentEnemyHitPoints.ToString();
        classHitPointsText.text = GameManager.Instance.CurrentPlayerHitPoints.ToString();

        whichRoundText.text = GameManager.Instance.Round.ToString();
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

    /// <summary>
    /// will set up the fight UI according to our currentClass and the current Enemy
    /// </summary>
    /// <param name="currentClass"></param>
    /// <param name="currentEnemy"></param>
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
                className.text = Database.Instance.fighter.className;
            }
            if (currentClass == Database.Instance.thief)
            {
                classAttackText.text = Database.Instance.thief.attack.ToString();
                classImage.sprite = Database.Instance.thief.classSprite;
                className.text = Database.Instance.thief.className;
            }
            if (currentClass == Database.Instance.sorcerer)
            {
                classAttackText.text = Database.Instance.sorcerer.attack.ToString();
                classImage.sprite = Database.Instance.sorcerer.classSprite;
                className.text = Database.Instance.sorcerer.className;
            }

            classHitPointsText.text = GameManager.Instance.PlayerData.currentHitPoints.ToString();
            classAttackModifierText.text = GameManager.Instance.classAttackModifier.ToString();
        }
    }

    #endregion

    #region Dialogue

    /// <summary>
    /// will open or close the GoldUI from the Dialogue
    /// </summary>
    /// <param name="goldUiOpen"></param>
    public void ToggleGoldDialogue(bool goldUiOpen)
    {
        int playerGold = GameManager.Instance.PlayerData.gold;
        UpdateUiGold(playerGold);

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

    public void ToggleInventory (bool inventoryOpen)
    {

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
    
    /// <summary>
    /// gives us a reason why we cant add something to the inventory
    /// </summary>
    /// <param name="cantAddReason"></param>
    public void SetCantAddText(string cantAddReason)
    {
        switch (cantAddReason)
        {
            case "notEnoughMoney":
                _cantAddText.text = "You don't have enough money for this.";
                break;
            case "noFreeInvSlot":
                _cantAddText.text = "You don't have a free Inventory Slot for this.";
                break;
        }
        _cantAddPanel.SetActive(true);
    }

    #endregion

    private void StartStartScreen()
    {
        _startScreen.SetActive(true);
        _startImage1.SetActive(true);
        _startImage2.SetActive(false);

        StartCoroutine(WaitStartScreen(5f));
    }

    private IEnumerator WaitStartScreen(float delay)
    {
        Debug.Log("Waiting for 5f");
        yield return new WaitForSeconds(delay);

        _startImage1.SetActive(false);
        _startImage2.SetActive(true);
    }

    public void UpdateUiGold (int playerGold)
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
        _startScreen.SetActive(false);
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
        _cantAddPanel.SetActive(false);
        _winLosePanel.SetActive(false);
        _cantAddPanel.SetActive(false);
    }
}
