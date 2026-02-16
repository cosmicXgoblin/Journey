using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Security.Cryptography;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class GameManager : MonoBehaviour //, IDataPersistence
{
    [Header("Manager")]
    [SerializeField] private GameObject _uiManager;
    [SerializeField] private GameObject _database;

    [Header("Game")]
    GameState currentGameState;

    [Header("Enemy")]
    public ScriptableObject currentEnemy;
    private int enemyAttack;
    private int _currentEnemyHitPoints;
    public int currentEnemyHitPoints;

    [Header("Player")]
    public Class currentClass;
    public Image classImage;
    private int classAttack;
    private int _currentPlayerHitPoints;
    public int currentPlayerHitPoints;
    public int classAttackModifier;

    [Header("UI")]
    public TextMeshProUGUI fightText;
    public TextMeshProUGUI whichRoundText;
    private GameObject _playerAttackButton;

    [Header("Fight")]
    BattleState currentBattleState;
    [SerializeField] private int round = -1;
    int randomNumber;
    int randomNumberOdd;
    bool playerTurn;
    int diceroll;
    [SerializeField] bool playerTurnDone;
    [SerializeField] bool enemyTurnDone;
    [SerializeField] bool playerFirst;


    [Header("PlayerData")]
    [SerializeField] private PlayerData _playerData;

     public PlayerData PlayerData => _playerData;

    #region init
    private void Awake()
    {
        currentBattleState = BattleState.noFight;
        currentGameState = GameState.init;

        _playerAttackButton = _uiManager.GetComponent<UiManager>().playerAttackButton;
    }

    void Update()
    {
        Debug.Log("GameState: " + currentGameState.ToString() + " | BattleState: " + currentBattleState.ToString());

        if (currentGameState == GameState.init)
        {
            return;
        }
        if (currentGameState == GameState.activeBattle)
        {
            CheckTurn();
            Debug.Log(currentBattleState + " | round: " + round + " | " + "playerTurn: " + playerTurn);
        }

        CallUpdateUI(_currentPlayerHitPoints);
    }
    #endregion

    #region OnClick
    public void OnClickRestart()
    {
        ClearFight();
        _uiManager.GetComponent<UiManager>().ClearFightUI();
    }

    public void OnClickBackToMap()
    {
        _uiManager.GetComponent<UiManager>().CallBackToMap();
    }

    public void OnClickPlayerAttack()
    {
        PlayerAttack();
    }

    public void OnClickSetCurrentClass(string selectedClass)
    {
        if (selectedClass == "fighter") currentClass = _database.GetComponent<Database>().fighter;
        if (selectedClass == "thief") currentClass = _database.GetComponent<Database>().thief;
        if (selectedClass == "sorcerer") currentClass = _database.GetComponent<Database>().sorcerer;
    }
        
    public void OnClickSetPlayerData()
    {
        // var currentPlayerData = Instantiate(_playerData);
        _playerData.className = currentClass.className;
        _playerData.classSprite = currentClass.classSprite;
        _playerData.classSpriteRound = currentClass.classSpriteRound;
        _playerData.mapFigure_Base = currentClass.classMapfigure_Base;
        _playerData.mapFigure_noBase = currentClass.classMapfigure_noBase;
        _playerData.fight = currentClass.fight;
        _playerData.thinking = currentClass.thinking;
        _playerData.speed = currentClass.speed;
        _playerData.observing = currentClass.observing;
        _playerData.dexterity = currentClass.dexterity;
        _playerData.charme = currentClass.charme;
        _playerData.attack = currentClass.attack;
        _playerData.maxHitPoints = currentClass.maxHitPoints;
        _playerData.currentHitPoints = currentClass.maxHitPoints;

        if (currentClass.className == "Fighter")
            _playerData.attackModifier = _playerData.fight;
        if (currentClass.className == "Thief")
            _playerData.attackModifier = _playerData.dexterity;
        if (currentClass.className == "Sorcerer")
            _playerData.attackModifier = _playerData.thinking;

        //Debug.Log(_playerData.attackModifier);
        _uiManager.GetComponent<UiManager>().UpdateUi(_playerData.currentHitPoints);
        _currentPlayerHitPoints = _playerData.currentHitPoints;

        currentGameState = GameState.onMap;

        //this are later for loading / saving, just as a reminder
        // currentPlayerData.currenthitPoints;
        // currentPlayerData.Item1
        // currentPlayerData.Item2
        // currentPlayerData.Item3
        // currentPlayerData.Item4
        // currentPlayerData.Item5
        // currentPlayerData.lastLocation
    }
    #endregion

    #region Fight
    public void StartBattle(ScriptableObject enemy)
    {
        currentEnemy = enemy;

        SetBattle();
        _uiManager.GetComponent<UiManager>().CallSetFightUI(currentClass, currentEnemy);

        Coinflip(0, 100, true);

        currentGameState = GameState.activeBattle;
        currentBattleState = BattleState.fight;

        _uiManager.GetComponent<UiManager>().ShowFightUI();
    }

    public void SetBattle()
    {
        if (currentEnemy == null || currentClass == null) return;

        if (currentEnemy != null)
        {
            if (currentEnemy == _database.GetComponent<Database>().rat)
            {
                enemyAttack = _database.GetComponent<Database>().rat.attack;
                _currentEnemyHitPoints = _database.GetComponent<Database>().rat.hitPoints;
            }
            if (currentEnemy == _database.GetComponent<Database>().rat1)
            {
                enemyAttack = _database.GetComponent<Database>().rat1.attack;
                _currentEnemyHitPoints = _database.GetComponent<Database>().rat1.hitPoints;
            }
            if (currentEnemy == _database.GetComponent<Database>().rat2)
            {
                enemyAttack = _database.GetComponent<Database>().rat2.attack;
                _currentEnemyHitPoints = _database.GetComponent<Database>().rat2.hitPoints;
            }
        }
        if (currentClass != null)
        {
            if (currentClass == _database.GetComponent<Database>().fighter)
            {
                classAttack = _database.GetComponent<Database>().fighter.attack;
                _currentPlayerHitPoints = _database.GetComponent<Database>().fighter.maxHitPoints;
                //classAttackModifier = _playerData.attackModifier;
            }
            if (currentClass == _database.GetComponent<Database>().thief)
            {
                classAttack = _database.GetComponent<Database>().thief.attack;
                _currentPlayerHitPoints = _database.GetComponent<Database>().thief.maxHitPoints;
                //classAttackModifier = _database.GetComponent<Database>().thief.attackModifier;
            }
            if (currentClass == _database.GetComponent<Database>().sorcerer)
            {
                classAttack = _database.GetComponent<Database>().sorcerer.attack;
                _currentPlayerHitPoints = _database.GetComponent<Database>().sorcerer.maxHitPoints;
                //classAttackModifier = _database.GetComponent<Database>().sorcerer.attackModifier;
            }

            classAttackModifier = _playerData.attackModifier;

            //classAttackModifierText.text = classAttackModifier.ToString();
        }
    }

        //UiCallSetFight(currentClass.name, currentEnemy.name);
        //className.text = currentClass.name;
        //enemyName.text = currentEnemy.name;

            //CheckTurn(playerTurn);

            //    if (currentEnemy != null)
            //    {
            //        if (currentEnemy == rat)
            //        {
            //            enemyAttackText.text = rat.attack.ToString();
            //            enemyImage.sprite = rat.enemySprite;
            //            enemyAttack = rat.attack;
            //            enemyHitPointsText.text = rat.hitPoints.ToString();
            //            _currentEnemyHitPoints = rat.hitPoints;
            //        }
            //        if (currentEnemy == rat1)
            //        {
            //            enemyAttackText.text = rat1.attack.ToString();
            //            enemyImage.sprite = rat1.enemySprite;
            //            enemyAttack = rat1.attack;
            //            enemyHitPointsText.text = rat1.hitPoints.ToString();
            //            _currentEnemyHitPoints = rat1.hitPoints;
            //        }
            //        if (currentEnemy == rat2)
            //        {
            //            enemyAttackText.text = rat2.attack.ToString();
            //            enemyImage.sprite = rat2.enemySprite;
            //            enemyAttack = rat2.attack;
            //            enemyHitPointsText.text = rat2.hitPoints.ToString();
            //            _currentEnemyHitPoints = rat2.hitPoints;
            //        }
            //    }
            //    if (currentClass != null)
            //    {
            //        if (currentClass == fighter)
            //        {
            //            classAttackText.text = fighter.attack.ToString();
            //            classImage.sprite = fighter.classSprite;
            //            classAttack = fighter.attack;
            //            classHitPointsText.text = fighter.hitPoints.ToString();
            //            _currentPlayerHitPoints = fighter.hitPoints;
            //            classAttackModifier = fighter.fight;
            //        }
            //        if (currentClass == thief)
            //        {
            //            classAttackText.text = thief.attack.ToString();
            //            classImage.sprite = thief.classSprite;
            //            classAttack = thief.attack;
            //            classHitPointsText.text = thief.hitPoints.ToString();
            //            _currentPlayerHitPoints = thief.hitPoints;
            //            classAttackModifier = fighter.dexterity;
            //        }
            //        if (currentClass == sorcerer)
            //        {
            //            classAttackText.text = sorcerer.attack.ToString();
            //            classImage.sprite = thief.classSprite;
            //            classAttack = sorcerer.attack;
            //            classHitPointsText.text = sorcerer.hitPoints.ToString();
            //            _currentPlayerHitPoints = sorcerer.hitPoints;
            //            classAttackModifier = fighter.thinking;
            //        }

            //        classAttackModifierText.text = classAttackModifier.ToString();
            //    }
            //}


            /// <summary>
            /// Flip a coin within the given range.
            /// This method is versatile usable due to the OddOrEven bool, which is just for fights.
            /// </summary>
            /// <param name="min"></param>          minimum range
            /// <param name="max"></param>          max range
            /// <param name="OddOrEven"></param>    are we in a fight and want to who has the first turn?
    public void Coinflip (int min, int max, bool OddOrEven)
    {
        randomNumber = Random.Range(min, max);
        Debug.Log(randomNumber);
        if (OddOrEven)
        {
            OddOrEven = false;

            randomNumberOdd = randomNumber % 2;
            if (randomNumberOdd == 0)
            {
                playerFirst = true;
                playerTurn = true;
                playerTurnDone = false;
                enemyTurnDone = false;

            }
            else
            {
                playerFirst = false;
                playerTurn = false;
                playerTurnDone = true;
                enemyTurnDone = true;

            }
        }
    }
    
    private void CallUpdateUI(int _currentPlayerHitPoints)
    {
        //currentEnemyHitPoints = _currentEnemyHitPoints;
        currentPlayerHitPoints = _currentPlayerHitPoints;

        _uiManager.GetComponent<UiManager>().UpdateUi(currentPlayerHitPoints);
    }

  /// <summary>
  /// Will only get called if the current GameState is activeBattle.
  /// Tracking a lot of the related UI, most should be moved. TODO.
  /// </summary>

    private void CheckTurn()
    {
        if (playerTurn && !playerTurnDone)
        {
            _playerAttackButton.SetActive(true);
        }

        if (!playerTurn && !enemyTurnDone)
        { 
            _playerAttackButton.SetActive(false);
            EnemyAttackPart1();

        }

        if (playerTurnDone && enemyTurnDone)
        {
            playerTurnDone = false;
            enemyTurnDone = false;

            round++;

            if (playerFirst) playerTurn = true;
            else playerTurn = false;

        }     
    }
    
    private void RollTheDice(int min, int max)
    {
        diceroll = Random.Range(min, max);
    }

    private void EnemyAttackPart1()
    {
        if (playerTurn) return;

        RollTheDice(0, 20);
        Debug.Log("Enemy rolled the dice.");
        StartCoroutine(WaitEnemy(1f));
    }

    private void EnemyAttackPart2()
    {
        if (enemyTurnDone) return;

        if (diceroll >= classAttack)
        {
            _currentPlayerHitPoints = _currentPlayerHitPoints - 1;
            Debug.Log("classHitPoints " + _currentPlayerHitPoints);
            fightText.text = "The enemy hit you!";
            CheckConditions();
        }
        if (diceroll < enemyAttack)
        {
            fightText.text = "The enemy missed you!";
        }

        enemyTurnDone = true;
        playerTurn = true;
    }

    private void PlayerAttack()
    {
        if (!playerTurn) return;

        RollTheDice(0, 20);

        if (diceroll + classAttackModifier >= enemyAttack)
        {
            _currentEnemyHitPoints = _currentEnemyHitPoints - 1;
            Debug.Log("enemyHitPoints " + _currentEnemyHitPoints);
            fightText.text = "You hit the enemy with " + diceroll + " " + classAttackModifier + ".";
            CheckConditions();
            //return;
        }
        if (diceroll + classAttackModifier < enemyAttack && _currentEnemyHitPoints != 0 && _currentPlayerHitPoints != 0)
        {
            fightText.text = "You missed the enemy!";
        }

        //StartCoroutine(Wait(5f));

        playerTurn = false;

        // StartCoroutine(Wait(5f));

        playerTurnDone = true;
    }

    private void CheckConditions()
    {
        _playerData.currentHitPoints = _currentPlayerHitPoints;

        if (_currentEnemyHitPoints <= 0)
        {
            currentBattleState = BattleState.fightFinished;
            whichRoundText.text = "-";

            fightText.text = "You killed the enemy. Good for you.";
            StartCoroutine(WaitAdventure(3f));
            ClearFight();
        }
        if (_currentPlayerHitPoints <= 0)
        {
            currentBattleState = BattleState.fightFinished;
            whichRoundText.text = "-";

            fightText.text = "The enemy wounded you badly. Are you dying?";
            StartCoroutine(WaitAdventure(0.5f));
            ClearFight();
        }
    }

    private void ClearFight()
    {
        currentBattleState = BattleState.noFight;
        currentGameState = GameState.onMap;

        currentClass = null;
        currentEnemy = null;
        classAttack = 0;
        enemyAttack = 0;
        _currentEnemyHitPoints = 0;

        fightText.text = "";
        _playerAttackButton.SetActive(false);

        _uiManager.GetComponent<UiManager>().ClearFightUI();
    }
    
    private IEnumerator WaitEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnemyAttackPart2();
    }

    private IEnumerator WaitAdventure(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Waiting for adventure");
        _uiManager.GetComponent<UiManager>().CallBackToMap();
    }

    #endregion

    public void Heal()
    {
        Debug.Log("Your body is healded. Your soul will always remember what you endured. Healed to " + _playerData.maxHitPoints + ".");

        _playerData.currentHitPoints = _playerData.maxHitPoints;
        _currentPlayerHitPoints = _playerData.maxHitPoints;
        _uiManager.GetComponent<UiManager>().UpdateUi(_playerData.maxHitPoints);

    }

    #region IDataPersistence

    //public void LoadData(GameData data)
    //{
    //    this.fightCount = data.fightCount;
    //}

    //public void SaveData(ref GameData data)
    //{
    //    data.fightCount = this.fightCount;
    //    Debug.Log("FightCount: " + fightCount);
    //}

    #endregion
}

enum BattleState
{
    fight,
    noFight,
    fightFinished
}

enum GameState
{
    init,
    onMap,
    activeBattle,
    paused
}