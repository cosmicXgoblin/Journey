using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class TestUiManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameManager;
    [SerializeField] private GameObject _playerController;

    [Header("Title Menu")]
    [SerializeField] private GameObject _canvasTitle;
    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private GameObject _titlePanelNewGame;
    [SerializeField] private GameObject _titlePanelLoadGame;

    [Header("New Game")]
    //[SerializeField] private InputField _inputField;
    public string inputFileName;

    [Header("Screens & Panels")]
    [SerializeField] private GameObject _testFight;
    [SerializeField] private GameObject _testChooseCharacter;
    [SerializeField] private GameObject _testMap;
    [SerializeField] private GameObject _screenUi;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _pausePanelMain;
    [SerializeField] private GameObject _pausePanelOptions;


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
    [SerializeField] private Slider _manaBar;
    [SerializeField] private GameObject _manaBarObject;
    [SerializeField] private GameObject _inventory;

    [SerializeField] private Sprite _classMapfigure_Base;
    [SerializeField] private Sprite _classMapfigure_noBase;
    [SerializeField] private bool _noBase;

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

        _canvasTitle.SetActive(true);
        _titlePanel.SetActive(true);
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
            _classImage.sprite = _gameManager.GetComponent<GameManager>().fighter.classSpriteRound;
            _classMapfigure_Base = _gameManager.GetComponent<GameManager>().fighter.classMapfigure_Base;
            _classMapfigure_noBase = _gameManager.GetComponent<GameManager>().fighter.classMapfigure_noBase;
            _className.text = _gameManager.GetComponent<GameManager>().fighter.className;
            _healthBar.value = _gameManager.GetComponent<GameManager>().fighter.hitPoints;
            _healthBar.maxValue = _gameManager.GetComponent<GameManager>().fighter.hitPoints;
            _manaBarObject.SetActive(false);
        }

        if (selectedClass == "thief")
        {
            _classImage.sprite = _gameManager.GetComponent<GameManager>().thief.classSpriteRound;
            _classMapfigure_Base = _gameManager.GetComponent<GameManager>().thief.classMapfigure_Base;
            _classMapfigure_noBase = _gameManager.GetComponent<GameManager>().thief.classMapfigure_noBase;
            _className.text = _gameManager.GetComponent<GameManager>().thief.className;
            _healthBar.value = _gameManager.GetComponent<GameManager>().thief.hitPoints;
            _healthBar.maxValue = _gameManager.GetComponent<GameManager>().thief.hitPoints;
            _manaBarObject.SetActive(false);
        }

        if (selectedClass == "sorcerer")
        {
            _classImage.sprite = _gameManager.GetComponent<GameManager>().sorcerer.classSpriteRound;
            _classMapfigure_Base = _gameManager.GetComponent<GameManager>().sorcerer.classMapfigure_Base;
            _classMapfigure_noBase = _gameManager.GetComponent<GameManager>().sorcerer.classMapfigure_noBase;
            _className.text = _gameManager.GetComponent<GameManager>().sorcerer.className;
            _healthBar.value = _gameManager.GetComponent<GameManager>().sorcerer.hitPoints;
            _healthBar.maxValue = _gameManager.GetComponent<GameManager>().sorcerer.hitPoints;
            _manaBar.value = 100;
            _manaBarObject.SetActive(true);
        }
        _playerMapfigure.GetComponent<SpriteRenderer>().sprite = _classMapfigure_Base;

    }

    public void OnClickContinue()
    {
        _pausePanel.SetActive(false);
        _playerController.GetComponent<PlayerController>().playerMap.Enable();
        _playerController.GetComponent<PlayerController>().playerMap.Disable();
    }

    public void OnClickOptions()
    {
        _pausePanelOptions.SetActive(true);
        _pausePanelMain.SetActive(false);
    }
     
    public void OnClickPauseMenu()
    {
        CallPause();
    }
    
    public void OnClickNewGame()
    {
        _titlePanel.SetActive(false);
        _titlePanelNewGame.SetActive(true);
    }
    public void OnClickStartNewGame()
    {
        //DataPersistenceManager.instance.NewGame();

        DataPersistenceManager.instance.CallSelectFilename(inputFileName);

        _titlePanelNewGame.SetActive(false);
        _testChooseCharacter.SetActive(true);
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
        _titlePanel.SetActive(true);
    }
  
    #endregion

    public void ReadStringInput(string s)
    {
        inputFileName = s + ".journey";
        //DataPersistenceManager.instance.CallSelectFilename(inputFileName);
    }

    public void ChangeMapfigureBase()
    {
        _noBase = !_noBase;

        if (!_noBase)
            _playerMapfigure.GetComponent<SpriteRenderer>().sprite = _classMapfigure_Base;
        else _playerMapfigure.GetComponent<SpriteRenderer>().sprite = _classMapfigure_noBase;
    }

    public void Fight()
    {
        _testMap.SetActive(false);
        _testFight.SetActive(true);
        _characterPanel.SetActive(false);
    }
    public void CallBackToMap()
    {
        _testMap.SetActive(true);
        _testFight.SetActive(false);
        _characterPanel.SetActive(true);
    }

    public void UpdateUI()
    {
       // _gameManager.GetComponent<GameManger>().playerData
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

    public void Blabla()
    {
        //_playerController.
    }

}
