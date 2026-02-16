//using UnityEngine;
//using TMPro;
//using System.Collections;
//using Unity.VisualScripting;
//using UnityEngine.UI;
//using System.Security.Cryptography;

//public class NewMonoBehaviourScript : MonoBehaviour
//{
//    [Header("Enemy")]
//    public ScriptableObject currentEnemy;
//    public TextMeshProUGUI enemyName;
//    public TextMeshProUGUI enemyAttackText;
//    private int enemyAttack;
//    public TextMeshProUGUI enemyHitPointsText;
//    private int _currentEnemyHitPoints;

//    [Header("Player")]
//    public ScriptableObject currentClass;
//    public TextMeshProUGUI _className;
//    public TextMeshProUGUI classAttackText;
//    private int classAttack;
//    public TextMeshProUGUI classHitPointsText;
//    private int _currentPlayerHitPoints;
//    private int classAttackModifier;
//    public TextMeshProUGUI classAttackModifierText;

//    [Header("Database")]
//    public Enemy rat;
//    public Enemy rat1;
//    public Enemy rat2;
//    public Class _fighterButton;
//    public Class _thiefButton;
//    public Class _sorcererButton;

//    [Header("UI")]
//    public TextMeshProUGUI fightText;
//    public TextMeshProUGUI whichRoundText;
//    public GameObject playerAttackButton;

//    [Header("Fight")]
//    BattleState gameState;
//    BattleState currentBattleState;
//    int round;
//    int randomNumber;
//    int randomNumberOdd;
//    bool playerTurn;
//    int diceroll;
//    bool playerTurnDone;
//    bool enemyTurnDone;
//    bool playerFirst;

//    #region init
//    private void Awake()
//    {
//        playerAttackButton.SetActive(false);
//        currentBattleState = BattleState.noFight;
//    }
//    private void Update()
//    {
//        if (currentBattleState == BattleState.fight)
//        {
//            CheckTurn(playerTurn, round);
//            CallUpdateUI();
//        }


//        Debug.Log(currentBattleState + " | round: " + round + " | " + "playerTurn: " + playerTurn);
//    }
//    #endregion

//    #region OnClick
//    public void SetBattle()
//    {
//        if (currentEnemy == null || currentClass == null) return;

//        _className.text = currentClass.name;
//        enemyName.text = currentEnemy.name;

//        if (currentEnemy != null)
//        {
//            if (currentEnemy == rat)
//            {
//                enemyAttackText.text = rat.attack.ToString();
//                enemyAttack = rat.attack;
//                enemyHitPointsText.text = rat.hitPoints.ToString();
//                _currentEnemyHitPoints = rat.hitPoints;
//            }

//            if (currentEnemy == rat1)
//            {
//                enemyAttackText.text = rat1.attack.ToString();
//                enemyAttack = rat1.attack;
//                enemyHitPointsText.text = rat1.hitPoints.ToString();
//                _currentEnemyHitPoints = rat1.hitPoints;
//            }

//            if (currentEnemy == rat2)
//            {
//                enemyAttackText.text = rat2.attack.ToString();
//                enemyAttack = rat2.attack;
//                enemyHitPointsText.text = rat2.hitPoints.ToString();
//                _currentEnemyHitPoints = rat2.hitPoints;
//            }
//        }

//        if (currentClass != null)
//        {
//            if (currentClass == _fighterButton)
//            {
//                classAttackText.text = _fighterButton.attack.ToString();
//                classAttack = _fighterButton.attack;
//                classHitPointsText.text = _fighterButton.hitPoints.ToString();
//                _currentPlayerHitPoints = _fighterButton.hitPoints;
//                classAttackModifier = _fighterButton.fight;
//            }
//            if (currentClass == _thiefButton)
//            {
//                classAttackText.text = _thiefButton.attack.ToString();
//                classAttack = _thiefButton.attack;
//                classHitPointsText.text = _thiefButton.hitPoints.ToString();
//                _currentPlayerHitPoints = _thiefButton.hitPoints;
//                classAttackModifier = _fighterButton.dexterity;
//            }
//            if (currentClass == _sorcererButton)
//            {
//                classAttackText.text = _sorcererButton.attack.ToString();
//                classAttack = _sorcererButton.attack;
//                classHitPointsText.text = _sorcererButton.hitPoints.ToString();
//                _currentPlayerHitPoints = _sorcererButton.hitPoints;
//                classAttackModifier = _fighterButton.thinking;
//            }

//            classAttackModifierText.text = classAttackModifier.ToString();
//        }
//    }
    
//    public void OnClickRestart()
//    {
//        ClearFight();
//    }

//    public void InitBattle()
//    {
//        currentBattleState = BattleState.fight;
//        round = 0;

//        randomNumber = Random.Range(0, 100);
//        randomNumberOdd = randomNumber % 2;

//        OddOrEven(randomNumberOdd);
//    }

//    public void OnClickPlayerAttack()
//    {
//        PlayerAttack();
//    }

//    #endregion

//    #region Fight
     
//    private void CallUpdateUI()
//    {
//        enemyHitPointsText.text = _currentEnemyHitPoints.ToString();
//        classHitPointsText.text = _currentPlayerHitPoints.ToString();

//        whichRoundText.text = round.ToString();
//    }
   
//    private void OddOrEven(int randomNumberOdd)
//    {
//        if (randomNumberOdd == 0)
//        {
//            playerTurn = true;
//            playerTurnDone = false;
//        }

//        else
//        {
//            playerTurn = false;
//            playerTurnDone = true;
//            playerFirst = false;
//        }
//    }

//    private void CheckTurn(bool playerTurn, int round)
//    {
//        //if (!playerFirst && enemyTurnDone && )

//        if (playerTurn & !playerTurnDone)
//        {
//            playerAttackButton.SetActive(true);
//        }

//        if (!playerTurn & !enemyTurnDone)
//        {
//            playerAttackButton.SetActive(false);
//            EnemyAttackPart1();
//        }

//        if (playerTurnDone && enemyTurnDone)
//        {
//            playerTurnDone = false;
//            enemyTurnDone = false;

//            round++;

//            if (playerFirst) playerTurn = true;
//            else playerTurn = false;
//        }      

//    }
    
//    private void RollTheDice()
//    {
//        diceroll = Random.Range(0, 20);
//    }

//    private void EnemyAttackPart1()
//    {
//        if (playerTurn) return;

//        RollTheDice();
//        Debug.Log("Enemy rolled the dice.");
//        StartCoroutine(WaitEnemy(2f));
//    }

//    private void EnemyAttackPart2()
//    {
//        if (enemyTurnDone) return;

//        if (diceroll >= classAttack)
//        {
//            _currentPlayerHitPoints = _currentPlayerHitPoints - 1;
//            Debug.Log("_currentPlayerHitPoints " + _currentPlayerHitPoints);
//            fightText.text = "The enemy hit you!";
//            CheckConditions();
//        }
//        if (diceroll < enemyAttack)
//        {
//            fightText.text = "The enemy missed you!";
//        }

//        enemyTurnDone = true;
//        playerTurn = true;
//    }

//    private void PlayerAttack()
//    {
//        if (!playerTurn) return;

//        RollTheDice();

//        if (diceroll + classAttackModifier >= enemyAttack)
//        {

//            _currentEnemyHitPoints = _currentEnemyHitPoints - 1;
//            Debug.Log("_currentEnemyHitPoints " + _currentEnemyHitPoints);
//            fightText.text = "You hit the enemy with " + diceroll + " " + classAttackModifier + ".";
//            CheckConditions();
//        }
//        if (diceroll + classAttackModifier < enemyAttack)
//        {
//            fightText.text = "You missed the enemy!";
//        }

//        //StartCoroutine(Wait(5f));

//        playerTurn = false;

//        // StartCoroutine(Wait(5f));

//        playerTurnDone = true;
//    }

//    private IEnumerator WaitEnemy(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        EnemyAttackPart2();
//    }

//    private IEnumerator WaitRestart(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        ClearFight();
//    }

//    private void CheckConditions()
//    {
//        CallUpdateUI();

//        if (_currentEnemyHitPoints <= 0)
//        {
//            currentBattleState = BattleState.fightFinished;
//            whichRoundText.text = "-";

//            fightText.text = "You killed the enemy. Good for you.";
//            StartCoroutine(WaitRestart(5f));
//        }
//        if (_currentPlayerHitPoints <= 0)
//        {
//            currentBattleState = BattleState.fightFinished;
//            whichRoundText.text = "-";

//            fightText.text = "The enemy wounded you badly. Are you dying?";
//            StartCoroutine(WaitRestart(3f));
//        }
//    }

//    private void ClearFight()
//    {
//        currentBattleState = BattleState.noFight;

//        currentClass = null;
//        currentEnemy = null;
//        _className.text = "Name";
//        enemyName.text = "Name";
//        classAttackText.text = "Attack";
//        enemyAttackText.text = "Attack";
//        classAttack = 0;
//        enemyAttack = 0;
//        classHitPointsText.text = "Hitpoints";
//        enemyHitPointsText.text = "Hitpoints";
//        _currentPlayerHitPoints = 0;
//        _currentEnemyHitPoints = 0;

//        fightText.text = "";
//        playerAttackButton.SetActive(false);

//        //CallUpdateUI();
//    }

//    #endregion
//}

//enum BattleState
//{
//    fight,
//    noFight,
//    fightFinished
//}