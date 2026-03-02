using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour //, IDataPersistence
{
    [Header("Game")]
    GameState currentGameState;

    [Header("Enemy")]
    public Enemy currentEnemy;
    private int enemyAttack;
    private int _currentEnemyHitPoints;

    [Header("Player")]
    public Class currentClass;
    public Image classImage;
    private int classAttack;
    private int _currentPlayerHitPoints;
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
    bool _freeInvSlot = false;

    [Header("etc")]
    private bool _tutorial = false;
    private int _diceroll;
    [SerializeField] private PlayerController _playerController;
    

    public static GameManager Instance { get; private set; }
    public PlayerData PlayerData => _playerData;
    public int CurrentPlayerHitPoints => _currentPlayerHitPoints;
    public int CurrentEnemyHitPoints => _currentEnemyHitPoints;
    public int Round => _round;
    public int Diceroll => _diceroll;
    public bool Tutorial => _tutorial;


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
        // overview for Debugging
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
    /// <summary>
    /// toggles Tutorial
    /// </summary>
    public void OnClickToggleTutorial()
    {
        if (!_tutorial) _tutorial = true;
        else _tutorial = false;
    }

    /// <summary>
    /// Button: Pausemenu [back to map] #TODO
    /// </summary>
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

    /// <summary>
    /// sets up the PlayerData according to the chosen class #TODO
    /// </summary>
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

        UiManager.Instance.UpdateUiHP(_playerData.currentHitPoints);
        UiManager.Instance.UpdateUiGold(_playerData.gold);
        _currentPlayerHitPoints = _playerData.currentHitPoints;

        currentGameState = GameState.onMap;
    }
    
    /// <summary>
    /// sets up the current Class, either through the Tutorial or the non-implemented cold start
    /// </summary>
    /// <param name="selectedClass"></param>
    public void SetCurrentClass(string selectedClass)
    {   
        if (selectedClass == "fighter") currentClass = Database.Instance.fighter;
        if (selectedClass == "thief") currentClass = Database.Instance.thief;
        if (selectedClass == "sorcerer") currentClass = Database.Instance.sorcerer;
    }
    #endregion

    #region Fight
    /// <summary>
    /// starts everything we need to be battle-ready, also invokes a coinflip to check which entity begins
    /// </summary>
    /// <param name="enemy"></param> enemy selected through dialoguechoices or when entering a tile with one
    public void StartBattle(Enemy enemy)
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

    /// <summary>
    /// starts everything we need to be battle-ready, this time with a bool so we can decide on who has the first turn via dialogue
    /// </summary>
    /// <param name="enemy"></param> enemy selected through dialoguechoices
    public void StartBattle(Enemy enemy, bool playerStarts)
    {
        currentGameState = GameState.transition;
        currentEnemy = enemy;

        SetBattle(enemy);
        UiManager.Instance.CallSetFightUI(currentClass, currentEnemy);

        if (playerStarts)
        {
            playerFirst = true;
            playerTurn = true;
            playerTurnDone = false;
            enemyTurnDone = false;
        }
        if (!playerStarts)
        {
            playerFirst = false;
            playerTurn = false;
            playerTurnDone = true;
            enemyTurnDone = true;
        }

            currentGameState = GameState.activeBattle;
        currentBattleState = BattleState.fight;

        UiManager.Instance.ShowFightUI();
        UiManager.Instance.ToggleGoldDialogue(true);
    }

    /// <summary>
    /// setting up the battle with the currentEnemy and the currentClass | with defensive checks so we can't enter battle without an enemy
    /// </summary>
    /// <param name="enemy"></param> enemy selected through dialoguechoices or when entering a tile with one
    public void SetBattle(ScriptableObject enemy)
    {
        if (currentGameState != GameState.transition)
            return;

        //Debug.Log(currentEnemy + " or " + enemy.name.ToString());
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
            }
            if (currentClass == Database.Instance.thief)
            {
                classAttack = Database.Instance.thief.attack;
            }
            if (currentClass == Database.Instance.sorcerer)
            {
                classAttack = Database.Instance.sorcerer.attack;
            }
            classAttackModifier = _playerData.attackModifier;
        }
    }

    /// <summary>
    /// setting up the battle via enemy-string (used for dialogues)
    /// </summary>
    /// <param name="enemy"></param> enemy selected through dialoguechoices
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
    /// a coinflip decides who has the first turn
    /// </summary>
    /// <param name="min"></param>          minimum range
    /// <param name="max"></param>          max range
    /// <param name="OddOrEven"></param>    are we in a fight and want know who has the first turn?
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
     /// Will only get called if the current GameState is not initiating
     /// </summary>   
    private void CallUpdateUI(int _currentPlayerHitPoints)
    {
        UiManager.Instance.UpdateUiHP(CurrentPlayerHitPoints);
    }

    /// <summary>
    /// used while in battle, will check whose turn it is and set the PlayerAttackButton accordingly
    /// </summary>
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

    /// <summary>
    /// enemy rolls a D20 to check for their attack
    /// with a short delay until the effect (EnemyAttackPart2) so it feels like we have to wait for the Diceroll, also a bit for slowing down
    /// </summary>
    private void EnemyAttackPart1()
    {
        if (playerTurn) return;

        RollTheDice(0, 20);
        Debug.Log("Enemy rolled the dice.");
        StartCoroutine(WaitEnemy(1f));
    }

    /// <summary>
    /// after 'waiting', we calculate if the attack was a hit or miss
    /// then we set the bools to the playerTurn and advance the round counter
    /// </summary>
    private void EnemyAttackPart2()
    {
        if (enemyTurnDone) return;

        if (_diceroll >= classAttack)
        {
            _currentPlayerHitPoints = _currentPlayerHitPoints - 1;
            Debug.Log("classHitPoints " + _currentPlayerHitPoints);
            UiManager.Instance.fightText.text = "The enemy hit you!";

            CheckConditions();
        }
        if (_diceroll < classAttack)
        {
            UiManager.Instance.fightText.text = "The enemy missed you!";
        }              
        enemyTurnDone = true;
        playerTurn = true;

        if (playerFirst) _round++;
    }

    /// <summary>
    /// after rolling a D20 it calculates if the attack was a hit or miss
    /// then we set the bools to the enemyTurn and advance the round counter
    /// </summary>
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

    /// <summary>
    /// will check for win / lose conditions
    /// </summary>
    private void CheckConditions()
    {
        _playerData.currentHitPoints = _currentPlayerHitPoints;

        if (currentEnemy == null) return;

            currentBattleState = BattleState.fightFinished;

        if (_currentEnemyHitPoints <= 0)
        {

            UiManager.Instance.whichRoundText.text = "WIN";
            UiManager.Instance.fightText.text = "You killed the enemy. Good for you.";
            bool battleWon = true;

            EndFight(1f, battleWon);

        }
        if (_currentPlayerHitPoints <= 0)
        {
            UiManager.Instance.whichRoundText.text = "LOSE";
            UiManager.Instance.fightText.text = "The enemy wounded you badly. Are you dying?";
            bool battleWon = false;

            EndFight(1f, battleWon);

            //TeleportToCity();
        }
    }

    private void ClearFight()
    {
        currentBattleState = BattleState.noFight;
        currentGameState = GameState.transition;

        currentEnemy = null;
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
    
    /// <summary>
    /// artifical delay so it feels more like a diceroll
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator WaitEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnemyAttackPart2();
    }

    // not in use currently, #TODO?
    private IEnumerator WaitAdventure(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Waiting for adventure");
        UiManager.Instance.CallBackToMap();
        ClearFight();
    }

/// <summary>
/// will clear the fight, set up the Endscreen for it and add the loot if won
/// </summary>
/// <param name="delay"></param>
/// <param name="battleWon"></param>
    private void EndFight(float delay, bool battleWon)
    {
        UiManager.Instance.playerAttackButton.SetActive(false);
        UiManager.Instance.ToggleGoldDialogue(false);
        _tutorial = false;

        if (battleWon)
        {
            RollTheDice(1, currentEnemy.loot.Count);
            TryToAdd(currentEnemy.loot[Diceroll]);
            Debug.Log("A " + Diceroll + " was rolled & the loot is " + currentEnemy.loot[Diceroll].ToString());

            string loot = currentEnemy.loot[Diceroll].ToString();
            UiManager.Instance.ShowAndSetWinLoseText(battleWon, loot);

        }
        else
        {
            UiManager.Instance.ShowAndSetWinLoseText(battleWon, "");
        }

        ClearFight();
        //StartCoroutine(WaitAdventure(delay));
    }

    private void TeleportToCity()
    {
        _playerController.gameObject.transform.position = _playerController.GetComponent<PlayerController>().camp;
    }

    public void OnClickTeleportToCity()
    {
        TeleportToCity();
    }

    #endregion

    #region Items
    
    /// <summary>
    ///  the item we are currently investigating
    /// </summary>
    /// <param name="itemInSlot"></param> Item in Slot we are investigating
    /// <param name="inventorySlot"></param> Slot we are investigating
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

    /// <summary>
    /// if we have enough gold and a free inventorySlot, the Item will be added to our inventory and the money will be deducted
    /// if we don't have enough money and / or a free inventorySlot, it will open the UI that says o
    /// </summary>
    /// <param name="item"></param>
    /// <param name="goldValue"></param>
    public void BuyItem(string item, int goldValue)
    {
        LookForFreeInvSlot();

        if (PlayerData.gold >= goldValue && _freeInvSlot)
        {
            PlayerData.gold = PlayerData.gold - goldValue;
            UiManager.Instance.UpdateUiGold(PlayerData.gold);

            Debug.Log("Currently at BuyItem. Item to buy: " + item);

            GetItemToAdd(item);
            DialogueManager.Instance.CallToggleVariable(true, "itemBought");
        }
        else if (PlayerData.gold < goldValue)
        {
            Debug.Log("Didn't have enough money");
            UiManager.Instance.SetCantAddText("notEnoughMoney");
            DialogueManager.Instance.CallToggleVariable(false, "itemBought");
        }
        else if (!_freeInvSlot)
        {
            Debug.Log("Didn't have enough space in your inventory");
            UiManager.Instance.SetCantAddText("noFreeInvSlot");
            DialogueManager.Instance.CallToggleVariable(false, "itemBought");
        }
        else Debug.Log("Item could not be added due to undefined error.");

        _freeInvSlot = false;
    }

    private void LookForFreeInvSlot()
    {
        // i really wish i could explain my deep irritation with for loops
        if(invSlots[0].itemInSlot == null || invSlots[1].itemInSlot == null || invSlots[2].itemInSlot == null || invSlots[3].itemInSlot == null ||
            invSlots[5].itemInSlot == null || invSlots[6].itemInSlot == null || invSlots[7].itemInSlot == null || invSlots[8].itemInSlot == null)
        {
            _freeInvSlot = true;
        }   
    }

    public void CallGetItemToAdd(string item)
    {
        GetItemToAdd(item);
    }

    /// <summary>
    /// due to the mechanics of ink, i have to pass along the items we chose via string and compare
    /// this should be moved to Database tho #TODO
    /// </summary>
    /// <param name="item"></param>
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

    /// <summary>
    /// looking for a free slot and then adding the item to it
    /// </summary>
    /// <param name="itemToAdd"></param>
    private void TryToAdd(Item itemToAdd)
    {
        LookForFreeInvSlot();
        Debug.Log("Trying to add: " + itemToAdd.itemName);

        if (_freeInvSlot)
        {
            if (invSlots[0].itemInSlot == null) invSlots[0].AddItem(itemToAdd);
            else if (invSlots[1].itemInSlot == null) invSlots[1].AddItem(itemToAdd);
            else if (invSlots[2].itemInSlot == null) invSlots[2].AddItem(itemToAdd);
            else if (invSlots[3].itemInSlot == null) invSlots[3].AddItem(itemToAdd);
            else if (invSlots[4].itemInSlot == null) invSlots[4].AddItem(itemToAdd);
            else if (invSlots[5].itemInSlot == null) invSlots[5].AddItem(itemToAdd);
            else if (invSlots[6].itemInSlot == null) invSlots[6].AddItem(itemToAdd);
            else if (invSlots[7].itemInSlot == null) invSlots[7].AddItem(itemToAdd);
            else if (invSlots[8].itemInSlot == null) invSlots[8].AddItem(itemToAdd);
        }
        else UiManager.Instance.SetCantAddText("noFreeInvSlot");
            _freeInvSlot = false;
    }

    public void CallTryToAdd(Item itemName)
    {
        TryToAdd(itemName);
    }

    /// <summary>
    /// in some cases, items form the dialogue are floating for a bit while the story is deciding if we get them or not
    /// </summary>
    /// <param name="item"></param>
    public void CallFloatItem(string item)
    {
        CallFloatItem(item);
    }

    /// <summary>
    /// nom nom nom (really thinking if i should make swords consumables just for funsies)
    /// </summary>
    public void OnClickConsumeItem()
    {
        ConsumeItem(_tempItem, _tempInvSlot);
        _tempInvSlot.ClearSlot();
    }

    /// <summary>
    /// checks if our item has an effect and invoke it
    /// </summary>
    /// <param name="item"></param>
    /// <param name="inventorySlot"></param>
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
        if (item.effect == ItemEffect.Coin)
        {
            PlayerData.gold = PlayerData.gold + item.buff;
            UiManager.Instance.UpdateUiGold(PlayerData.gold);
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
    
    /// <summary>
    /// will heal us according to the passed along values
    /// </summary>
    /// <param name="heal"></param> how much does this heal?
    /// <param name="maxHeal"></param> is it healing max?
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

    /// <summary>
    ///  will damage us to the passed along values
    ///  #TODO change CheckConditions bc we should be able to die anywhere #freedom
    /// </summary>
    /// <param name="damage"></param> how much damage does this make?
    /// <param name="maxDamage"></param> will it make maxDamage (aka kill?)
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
    
    /// <summary>
    /// will roll the passed along ability check and pass along the number and the difficulty
    /// </summary>
    /// <param name="ability"></param> what ability should be checked
    /// <param name="difficulty"></param> against what difficulty
    public void RollAbilityCheck(string ability, int difficulty)
    {
        GameManager.Instance.RollTheDice(1, 21);
        switch (ability)
        {
            case "fight":
                _diceroll = _diceroll + GameManager.Instance.PlayerData.fight;
                EvaluateAbilityCheck(_diceroll, difficulty);
                    break;
            case "thinking":
                _diceroll = _diceroll + GameManager.Instance.PlayerData.thinking;
                EvaluateAbilityCheck(_diceroll, difficulty);
                break;
            case "speed":
                _diceroll = _diceroll + GameManager.Instance.PlayerData.speed;
                EvaluateAbilityCheck(_diceroll, difficulty);
                break;
            case "observing":
                _diceroll = _diceroll + GameManager.Instance.PlayerData.observing;
                EvaluateAbilityCheck(_diceroll, difficulty);
                break;
            case "dexterity":
                _diceroll = _diceroll + GameManager.Instance.PlayerData.dexterity;
                EvaluateAbilityCheck(_diceroll, difficulty);
                break;
            case "charme":
                _diceroll = _diceroll + GameManager.Instance.PlayerData.charme;
                EvaluateAbilityCheck(_diceroll, difficulty);
                break;
        }
    }

    /// <summary>
    /// will toggle a bool in ink (success in this case)
    /// </summary>
    /// <param name="_diceroll"></param> the number we rolled + modifiers
    /// <param name="difficulty"></param> the difficulty we rolled against
    private void EvaluateAbilityCheck(int _diceroll, int difficulty)
    {
        if (_diceroll >= difficulty) DialogueManager.Instance.CallToggleVariable(true, "success");
        else DialogueManager.Instance.CallToggleVariable(false, "success");

        Debug.Log("You had a " + Diceroll + " and the difficulty was " + difficulty + ".");
    }

    #endregion

    /// <summary>
    /// generic Diceroll, various uses through the game like ability checks
    /// </summary>
    /// <param name="min"></param> range from min ...
    /// <param name="max"></param> ... to max
    public void RollTheDice(int min, int max)
    {
        _diceroll = Random.Range(min, max);
    }

    //public void OnPause()
    //{
    //    UiManager.Instance.CallPause();
    //}

    // #TODO
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