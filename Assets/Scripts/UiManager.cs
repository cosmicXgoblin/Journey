using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using Ink.Runtime;
using System.Collections.Generic;

public class UiManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private GameObject _gameManager;
    [SerializeField] private GameObject _playerController;
    [SerializeField] private GameObject _database;
    
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

    [Header("Effects")]
    private Color _tempColor;
    public Color red;
    public Color original;





    #region Init
    void Start()
    {
        _testFight.SetActive(false);
        _testMap.SetActive(false);
        _screenUi.SetActive(true);                       // ScreenUi and _characterPanel need to be in this order!
        _characterPanel.SetActive(false);
        _testChooseCharacter.SetActive(false);
        _pausePanel.SetActive(false);
        _titlePanelNewGame.SetActive(false);
        _inventory.SetActive(false);
        _screenUi.SetActive(false);
        _titlePanelLoadGame.SetActive(false);
        _dialogueAndChoicesPanel.SetActive(false); ;
        _dialoguePanel.SetActive(false); ;


        _canvasTitle.SetActive(true);
        _titlePanel.SetActive(true);
        _titlePanelButtons.SetActive(true);
        _titlePanelOptions.SetActive(false);

        original.a = 0.5f;
    }
    #endregion

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

    public void OnClickSetCharacter(string selectedClass)
    {
        if (selectedClass == "fighter")
        {
            _classImage.sprite = _database.GetComponent<Database>().fighter.classSpriteRound;
            _classMapfigure_Base = _database.GetComponent<Database>().fighter.classMapfigure_Base;
            _classMapfigure_noBase = _database.GetComponent<Database>().fighter.classMapfigure_noBase;
            _className.text = _database.GetComponent<Database>().fighter.className;
            _healthBar.value = _database.GetComponent<Database>().fighter.maxHitPoints;
            _healthBar.maxValue = _database.GetComponent<Database>().fighter.maxHitPoints;
            _manaBarObject.SetActive(false);
        }

        if (selectedClass == "thief")
        {
            _classImage.sprite = _database.GetComponent<Database>().thief.classSpriteRound;
            _classMapfigure_Base = _database.GetComponent<Database>().thief.classMapfigure_Base;
            _classMapfigure_noBase = _database.GetComponent<Database>().thief.classMapfigure_noBase;
            _className.text = _database.GetComponent<Database>().thief.className;
            _healthBar.value = _database.GetComponent<Database>().thief.maxHitPoints;
            _healthBar.maxValue = _database.GetComponent<Database>().thief.maxHitPoints;
            _manaBarObject.SetActive(false);
        }

        if (selectedClass == "sorcerer")
        {
            _classImage.sprite = _database.GetComponent<Database>().sorcerer.classSpriteRound;
            _classMapfigure_Base = _database.GetComponent<Database>().sorcerer.classMapfigure_Base;
            _classMapfigure_noBase = _database.GetComponent<Database>().sorcerer.classMapfigure_noBase;
            _className.text = _database.GetComponent<Database>().sorcerer.className;
            _healthBar.value = _database.GetComponent<Database>().sorcerer.maxHitPoints;
            _healthBar.maxValue = _database.GetComponent<Database>().sorcerer.maxHitPoints;
            _manaBar.value = 100;
            _manaBarObject.SetActive(true);
        }
        _playerMapfigure.GetComponent<SpriteRenderer>().sprite = _classMapfigure_Base;

        UpdateUi(_gameManager.GetComponent<GameManager>().CurrentPlayerHitPoints);

    }

    public void OnClickContinue()
    {
        _pausePanel.SetActive(false);
        _playerController.GetComponent<PlayerController>().playerMap.Enable();
        _playerController.GetComponent<PlayerController>().playerMap.Disable();
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
    public void OnClickPauseMenu()
    {
        CallPause();
    }
    
    public void OnClickNewGame()
    {
        _titlePanel.SetActive(false);
        _titlePanelNewGame.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_titleButtonStartGame);
    }
    public void OnClickStartNewGame()
    {
        //DataPersistenceManager.instance.NewGame();

        DataPersistenceManager.instance.CallSelectFilename(inputFileName);

        _titlePanelNewGame.SetActive(false);
        _testChooseCharacter.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_fighterButton.gameObject);
    }

    public void OnClickLoadGamePart1()
    {
        _titlePanelLoadGame.SetActive(true);
        _titlePanel.SetActive(false);
    }
    public void OnClickLoadGamePart2()
    {
        DataPersistenceManager.instance.LoadGame();
    }

    public void OnClickSavegame()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void OnClickBackToTitle()
    {
        _titlePanelNewGame.SetActive(false);
        _titlePanelOptions.SetActive(false);
        _titlePanel.SetActive(true);
        _titlePanelButtons.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_titlePanelButtons.transform.GetChild(0).gameObject);
    }
    
    public void OnClickChangeMapfigureBase()
    {
        _noBase = !_noBase;

        if (!_noBase)
            _playerMapfigure.GetComponent<SpriteRenderer>().sprite = _classMapfigure_Base;
        else _playerMapfigure.GetComponent<SpriteRenderer>().sprite = _classMapfigure_noBase;
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

            _playerController.GetComponent<PlayerController>().playerMap.Disable();
            _playerController.GetComponent<PlayerController>().uiMap.Enable();

            EventSystem.current.SetSelectedGameObject(_pausePanelMain.transform.GetChild(0).gameObject);
        }
        else
        {
            _pausePanel.SetActive(false);
            _pausePanelOptions.SetActive(false);
            _pausePanelMain.SetActive(true);

            _playerController.GetComponent<PlayerController>().playerMap.Enable();
            _playerController.GetComponent<PlayerController>().uiMap.Disable();
        }
    }
    
    public void CallDialogueUI()
    {
        _playerController.SetActive(false);

        _dialogueAndChoicesPanel.SetActive(true);
        _dialoguePanel.SetActive(true);
        _choicesPanel.SetActive(false);
    }
    #endregion

    public void ReadStringInput(string s)
    {
        inputFileName = s + ".journey";
        //DataPersistenceManager.instance.CallSelectFilename(inputFileName);
    }

    #region Fight
    public void ShowFightUI()
    {
        _testMap.SetActive(false);
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
            if (currentEnemy == _database.GetComponent<Database>().rat)
            {
                enemyAttackText.text = _database.GetComponent<Database>().rat.attack.ToString();
                enemyImage.sprite = _database.GetComponent<Database>().rat.enemySprite;
                enemyHitPointsText.text = _database.GetComponent<Database>().rat.hitPoints.ToString();
            }
            if (currentEnemy == _database.GetComponent<Database>().rat1)
            {
                enemyAttackText.text = _database.GetComponent<Database>().rat1.attack.ToString();
                enemyImage.sprite = _database.GetComponent<Database>().rat1.enemySprite;
                enemyHitPointsText.text = _database.GetComponent<Database>().rat1.hitPoints.ToString();
            }
            if (currentEnemy == _database.GetComponent<Database>().rat2)
            {
                enemyAttackText.text = _database.GetComponent<Database>().rat2.attack.ToString();
                enemyImage.sprite = _database.GetComponent<Database>().rat2.enemySprite;
                enemyHitPointsText.text = _database.GetComponent<Database>().rat2.hitPoints.ToString();
            }
        }
        if (currentClass != null)
        {
            if (currentClass == _database.GetComponent<Database>().fighter)
            {
                classAttackText.text = _database.GetComponent<Database>().fighter.attack.ToString();
                classImage.sprite = _database.GetComponent<Database>().fighter.classSprite;
                classHitPointsText.text = _database.GetComponent<Database>().fighter.maxHitPoints.ToString();
            }
            if (currentClass == _database.GetComponent<Database>().thief)
            {
                classAttackText.text = _database.GetComponent<Database>().thief.attack.ToString();
                classImage.sprite = _database.GetComponent<Database>().thief.classSprite;
            }
            if (currentClass == _database.GetComponent<Database>().sorcerer)
            {
                classAttackText.text = _database.GetComponent<Database>().sorcerer.attack.ToString();
                classImage.sprite = _database.GetComponent<Database>().thief.classSprite;
            }

            classAttackModifierText.text = _gameManager.GetComponent<GameManager>().classAttackModifier.ToString();
        }
    }

    public IEnumerator ImageEffect(bool enemy, float delay, string color)
    {
        if (enemy)
            _target = enemyImage.GetComponent<Image>();
        else if (!enemy)
            _target = classImage.GetComponent<Image>();


        if (color == "red")
        {
            red.a = 1f;
            _tempColor = red;
            _target.color = _tempColor;
            Debug.Log("Image should be red for a moment");
        }
        yield return new WaitForSeconds(delay);
        _target.color = original;

        //_target.color = new Color(0f, 0f, 0f, 1f);
        //yield return new WaitForSeconds(delay);
        //_target.color = new Color(1f, 1f, 1f, 1f);
    }

    #endregion

    #region Dialogue
    
    

    #endregion


    public void UpdateUi(int currentPlayerHitPoints)        
    {
        _healthBar.value = currentPlayerHitPoints;
        _healthBarText.text = currentPlayerHitPoints.ToString() + " / " +  _gameManager.GetComponent<GameManager>().PlayerData.maxHitPoints.ToString();

        enemyHitPointsText.text = _gameManager.GetComponent<GameManager>().CurrentEnemyHitPoints.ToString();
        classHitPointsText.text = _gameManager.GetComponent<GameManager>().CurrentPlayerHitPoints.ToString();

        whichRoundText.text = _gameManager.GetComponent<GameManager>().Round.ToString();

        //_gameManager.GetComponent<GameManger>().playerData;
    }

}
