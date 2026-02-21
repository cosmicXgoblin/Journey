using System.Collections;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;

public class GameManager : MonoBehaviour //, IDataPersistence
{
    [Header("Game")]
    GameState currentGameState;

    [Header("Enemy")]
    public ScriptableObject currentEnemy;
    private int enemyAttack;
    [SerializeField]  private int _currentEnemyHitPoints;

    [Header("Player")]
    public Class currentClass;
    public Image classImage;
    private int classAttack;
    [SerializeField]  private int _currentPlayerHitPoints;
    public int classAttackModifier;

    [Header("Fight")]
    BattleState currentBattleState;
    private int _round = 0;
    int randomNumber;
    int randomNumberOdd;
    bool playerTurn;
    bool playerTurnDone;
    bool enemyTurnDone;
    bool playerFirst;

    [Header("PlayerData")]
    [SerializeField] private PlayerData _playerData;

    [Header("Inventory")]
    [SerializeField] private Item _tempItem;
    [SerializeField] private InventorySlot _tempInvSlot;
    [SerializeField] private List<InventorySlot> invSlots;

    [SerializeField] private bool _tutorial = false;
    private int _diceroll;

    public PlayerData PlayerData => _playerData;
    public int CurrentPlayerHitPoints => _currentPlayerHitPoints;
    public int CurrentEnemyHitPoints => _currentEnemyHitPoints;
    public int Round => _round;
    public int diceroll => _diceroll;
    public bool tutorial => _tutorial;
    public static GameManager Instance { get; private set; }

    #region init
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        currentBattleState = BattleState.noFight;
        currentGameState = GameState.init;
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
            Debug.Log(currentBattleState + " | round: " + _round + " | " + "playerTurn: " + playerTurn);
        }

        CallUpdateUI(_currentPlayerHitPoints);
    }
    #endregion

    #region OnClick
    public void OnClickToggleTutorial()
    {
        if (!_tutorial) _tutorial = true;
        else _tutorial = false;
    }

    public void OnClickRestart()
    {
        ClearFight();
        UiManager.Instance.ClearFightUI();
    }

    public void OnClickBackToMap()
    {
        UiManager.Instance.CallBackToMap();
    }

    public void OnClickPlayerAttack()
    {
        PlayerAttack();
    }

    public void OnClickSetCurrentClass(string selectedClass)
    {
        SetCurrentClass(selectedClass);
    }
        
    public void OnClickSetPlayerData()
    {
        SetPlayerData();
    }
    #endregion

    #region Setup
    public void SetPlayerData()
    {
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
        _playerData.gold = 100;

        if (currentClass.className == "Fighter")
            _playerData.attackModifier = _playerData.fight;
        if (currentClass.className == "Thief")
            _playerData.attackModifier = _playerData.dexterity;
        if (currentClass.className == "Sorcerer")
            _playerData.attackModifier = _playerData.thinking;

        //Debug.Log(_playerData.attackModifier);
        UiManager.Instance.UpdateUiHP(_playerData.currentHitPoints);
        UiManager.Instance.UpdateUiGold(_playerData.gold);
        _currentPlayerHitPoints = _playerData.currentHitPoints;

        currentGameState = GameState.onMap;
    }
    public void SetCurrentClass(string selectedClass)
    {   
        if (selectedClass == "fighter") currentClass = Database.Instance.fighter;
        if (selectedClass == "thief") currentClass = Database.Instance.thief;
        if (selectedClass == "sorcerer") currentClass = Database.Instance.sorcerer;
    }
    #endregion

    public void RollTheDice(int min, int max)
    {
        _diceroll = Random.Range(min, max);
    }  


    #region Fight
    public void StartBattle(ScriptableObject enemy)
    {
        currentGameState = GameState.transition;
        currentEnemy = enemy;

        SetBattle(enemy);
        UiManager.Instance.CallSetFightUI(currentClass, currentEnemy);

        Coinflip(0, 100, true);

        currentGameState = GameState.activeBattle;
        currentBattleState = BattleState.fight;

        UiManager.Instance.ShowFightUI();
    }

    public void SetBattle(ScriptableObject enemy)
    {
        if (currentGameState != GameState.transition)
            return;

        Debug.Log(currentEnemy + " or " + enemy.name.ToString());
        if (currentEnemy == null) Debug.Log("There is no current enemy!");

        if (currentEnemy != null)
        {
            if (currentEnemy == Database.Instance.rat)
            {
                enemyAttack = Database.Instance.rat.attack;
                _currentEnemyHitPoints = Database.Instance.rat.hitPoints;
            }
            if (currentEnemy == Database.Instance.rat1)
            {
                enemyAttack = Database.Instance.rat1.attack;
                _currentEnemyHitPoints = Database.Instance.rat1.hitPoints;
            }
            if (currentEnemy == Database.Instance.rat2)
            {
                enemyAttack = Database.Instance.rat2.attack;
                _currentEnemyHitPoints = Database.Instance.rat2.hitPoints;
            }
        }
        if (currentClass != null)
        {
            if (currentClass == Database.Instance.fighter)
            {
                classAttack = Database.Instance.fighter.attack;
                _currentPlayerHitPoints = Database.Instance.fighter.maxHitPoints;
            }
            if (currentClass == Database.Instance.thief)
            {
                classAttack = Database.Instance.thief.attack;
                _currentPlayerHitPoints = Database.Instance.thief.maxHitPoints;
            }
            if (currentClass == Database.Instance.sorcerer)
            {
                classAttack = Database.Instance.sorcerer.attack;
                _currentPlayerHitPoints = Database.Instance.sorcerer.maxHitPoints;
            }

            classAttackModifier = _playerData.attackModifier;
        }
    }

    public void SetBattle(string enemy)
    {
        switch (enemy)
        {
            case "rat":
                currentEnemy = Database.Instance.rat;
                StartBattle(currentEnemy);
                break;
            case "rat1":
                currentEnemy = Database.Instance.rat1;
                StartBattle(currentEnemy);
                break;
            case "rat2":
                currentEnemy = Database.Instance.rat2;
                StartBattle(currentEnemy);
                break;
        }
    }

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
  
     /// <summary>
     /// Will only get called if the current GameState is activeBattle.
     /// Tracking a lot of the related UI, most should be moved. TODO.
     /// </summary>   
    private void CallUpdateUI(int _currentPlayerHitPoints)
    {
        UiManager.Instance.UpdateUiHP(CurrentPlayerHitPoints);
    }

    private void CheckTurn()
    {
        if (playerTurn && !playerTurnDone)
        {
            UiManager.Instance.playerAttackButton.SetActive(true);
        }

        if (!playerTurn && !enemyTurnDone)
        {
            UiManager.Instance.playerAttackButton.SetActive(false);
            EnemyAttackPart1();
        }

        if (playerTurnDone && enemyTurnDone)
        {
            playerTurnDone = false;
            enemyTurnDone = false;
           
            if (playerFirst) playerTurn = true;
            else playerTurn = false;

        }     
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

        if (_diceroll >= classAttack)
        {
            _currentPlayerHitPoints = _currentPlayerHitPoints - 1;
            Debug.Log("classHitPoints " + _currentPlayerHitPoints);
            UiManager.Instance.fightText.text = "The enemy hit you!";
            //StartCoroutine(_uiManager.GetComponent<UiManager>().ImageEffect(true, 0.2f, "red"));

            CheckConditions();
        }
        if (_diceroll < enemyAttack)
        {
            UiManager.Instance.fightText.text = "The enemy missed you!";
        }              
        enemyTurnDone = true;
        playerTurn = true;

        if (playerFirst) _round++;
    }

    private void PlayerAttack()
    {
        if (!playerTurn) return;

        RollTheDice(0, 20);

        if (_diceroll + classAttackModifier >= enemyAttack)
        {
            _currentEnemyHitPoints = _currentEnemyHitPoints - 1;
            Debug.Log("enemyHitPoints " + _currentEnemyHitPoints);
            UiManager.Instance.fightText.text = "You hit the enemy with a" + _diceroll + " and an Attackmodifier of " + classAttackModifier + ".";
            //StartCoroutine(_uiManager.GetComponent<UiManager>().ImageEffect(false, 0.2f, "red"));

            CheckConditions();
        }
        if (_diceroll + classAttackModifier < enemyAttack && _currentEnemyHitPoints != 0 && _currentPlayerHitPoints != 0)
        {
            UiManager.Instance.fightText.text = "You missed the enemy!";
        }

        playerTurn = false;
        playerTurnDone = true;
        if (!playerFirst) _round++;
    }

    private void CheckConditions()
    {
        _playerData.currentHitPoints = _currentPlayerHitPoints;

        if (currentEnemy == null) return;

        if (_currentEnemyHitPoints <= 0)
        {
            currentBattleState = BattleState.fightFinished;
            currentGameState = GameState.onMap;
            UiManager.Instance.whichRoundText.text = "WIN";

            UiManager.Instance.fightText.text = "You killed the enemy. Good for you.";
            EndFight(2f);
            
        }
        if (_currentPlayerHitPoints <= 0)
        {
            currentBattleState = BattleState.fightFinished;
            currentGameState = GameState.onMap;
            UiManager.Instance.whichRoundText.text = "LOSE";

            UiManager.Instance.fightText.text = "The enemy wounded you badly. Are you dying?";
            EndFight(0.5f);
        }
    }

    private void ClearFight()
    {
        currentBattleState = BattleState.noFight;
        currentGameState = GameState.transition;

        currentClass = null;
        currentEnemy = null;
        classAttack = 0;
        enemyAttack = 0;
        _currentEnemyHitPoints = 0;
        playerTurnDone = false;
        enemyTurnDone = false;
        playerFirst = false;
        _round = 0;
        UiManager.Instance.fightText.text = "";
        UiManager.Instance.playerAttackButton.SetActive(false);

        UiManager.Instance.ClearFightUI();
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
        UiManager.Instance.CallBackToMap();
        ClearFight();
    }

    private void EndFight(float delay)
    {
        UiManager.Instance.playerAttackButton.SetActive(false);
        _tutorial = false;
        StartCoroutine(WaitAdventure(delay));
    }

    #endregion

    #region Items
    public void SetTempItem(Item itemInSlot, InventorySlot inventorySlot)
    {
        _tempItem = itemInSlot;
        _tempInvSlot = inventorySlot;
    }

    private void ClearTempItem()
    {
        _tempItem = null;
        _tempInvSlot = null;   
    }

    public void BuyItem(string item, int goldValue)
    {
        PlayerData.gold = PlayerData.gold - goldValue;
        UiManager.Instance.UpdateUiGold(PlayerData.gold);

        Debug.Log("Currently at BuyItem. Item to buy: " + item);

        GetItemToAdd(item);
    }

    private void GetItemToAdd(string item)
    {
        switch (item)
        {
            case "Apple":
                TryToAdd(Database.Instance.Apple);
                break;
            case "cheese":
                TryToAdd(Database.Instance.Cheese);
                break;
            case "Mystery Potion":
                TryToAdd(Database.Instance.MysteryPotion);
                break;
            case "Potion of Healing":
                TryToAdd(Database.Instance.PotionOfHealing);
                break;
            case "Potion of Strength":
                TryToAdd(Database.Instance.PotionOfStrength);
                break;
            case "Sword":
                TryToAdd(Database.Instance.Sword);
                break;
            case "sharp sword":
                TryToAdd(Database.Instance.sharpSword);
                break;
            case "Weird Potion":
                TryToAdd(Database.Instance.WeirdPotion);
                break;
        }
    }

    private void TryToAdd(Item itemToAdd)
    {
        Debug.Log("Trying to add: " + itemToAdd.itemName);

        if (invSlots[0].itemInSlot == null) invSlots[0].AddItem(itemToAdd);
        else if (invSlots[1].itemInSlot == null) invSlots[1].AddItem(itemToAdd);
        else if (invSlots[2].itemInSlot == null) invSlots[2].AddItem(itemToAdd);
        else if (invSlots[3].itemInSlot == null) invSlots[3].AddItem(itemToAdd);
        else if (invSlots[4].itemInSlot == null) invSlots[4].AddItem(itemToAdd);
        else if (invSlots[5].itemInSlot == null) invSlots[5].AddItem(itemToAdd);
        else if (invSlots[6].itemInSlot == null) invSlots[6].AddItem(itemToAdd);
        else if (invSlots[7].itemInSlot == null) invSlots[7].AddItem(itemToAdd);
        else if (invSlots[8].itemInSlot == null) invSlots[8].AddItem(itemToAdd);



        //foreach (InventorySlot invSlot in invSlots)
        //{
        //    if (invSlot == null) invSlot.AddItem(itemToAdd);
        //    break;

        //    //Debug.Log("Going through inventorySlots. Currently at " + invSlots[i].ToString() );
        //    //if (invSlots[i].itemInSlot == null)
        //    //    invSlots[i].AddItem(itemToAdd);
        //    //break;
        //}
    }

    public void CallTryToAdd(Item itemName)
    {
        TryToAdd(itemName);
    }

    public void OnClickConsumeItem()
    {
        ConsumeItem(_tempItem, _tempInvSlot);
    }

    private void ConsumeItem(Item item, InventorySlot inventorySlot)
    {
        if (item.effect == ItemEffect.Heal)
            Heal(item.buff, false);

        if (item.effect == ItemEffect.RandomEvent)
        {
            if (item.randomEvent == RandomEvent.HealOrDamage)
            {
                Debug.Log("Will this heal or damage you?");
                RollTheDice(1, 3);
                Debug.Log(_diceroll);
                if (_diceroll == 1) Heal(item.buff, false);
                else Damage(item.debuff, false);
            }
        }


        //if (item.effect == ItemEffect.Heal) Debug.Log("Heal");
        //else if (item.effect == ItemEffect.Damage) Debug.Log("Damage");
        //else if (item.effect == ItemEffect.RandomEvent) Debug.Log("RandomEvent");

        ClearTempItem();
        inventorySlot.ClearSlot();
        UiManager.Instance.CloseConsumableUI();
    }
    #endregion

    #region Statuschanges
    public void Heal(int heal, bool maxHeal)
    {
        if (maxHeal == true)
        {
            _playerData.currentHitPoints = _playerData.maxHitPoints;
        }
        else
        {
            int healToMax = _playerData.maxHitPoints - _playerData.currentHitPoints;
            Debug.Log("healtToMax: " + healToMax);
            if (heal > healToMax)
                _playerData.currentHitPoints = _playerData.maxHitPoints;
            else _playerData.currentHitPoints += heal;
            Debug.Log("Player got healed for " + heal);
        }
        _currentPlayerHitPoints = _playerData.currentHitPoints;
        UiManager.Instance.UpdateUiHP(_playerData.currentHitPoints);
    }

    public void Damage (int damage, bool maxDamage)
    {
        if (maxDamage)
        {
            Debug.Log("You received maximal Damage.");
        }
        else
        {
            _playerData.currentHitPoints -= damage;
            Debug.Log("Player got damaged for " + damage);
        }
        _currentPlayerHitPoints = _playerData.currentHitPoints;
        UiManager.Instance.UpdateUiHP(_playerData.currentHitPoints);
    }
    
    #endregion

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
    transition,
    onMap,
    inDialogue,
    activeBattle,
    paused
}