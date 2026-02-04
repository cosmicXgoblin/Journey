using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Security.Cryptography;

public class TestFight : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] GameObject uiManager;

    [Header("Enemy")]
    public ScriptableObject currentEnemy;
    public Image enemyImage;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyAttackText;
    private int enemyAttack;
    public TextMeshProUGUI enemyHitPointsText;
    private int enemyHitPoints;

    [Header("Player")]
    public ScriptableObject currentClass;
    public Image classImage;
    public TextMeshProUGUI className;
    public TextMeshProUGUI classAttackText;
    private int classAttack;
    public TextMeshProUGUI classHitPointsText;
    private int classHitPoints;
    private int classAttackModifier;
    public TextMeshProUGUI classAttackModifierText;

    [Header("Database")]
    public Enemy rat;
    public Enemy rat1;
    public Enemy rat2;
    public Class fighter;
    public Class thief;
    public Class sorcerer;

    [Header("UI")]
    public TextMeshProUGUI fightText;
    public TextMeshProUGUI whichRoundText;
    public GameObject playerAttackButton;

    [Header("Fight")]
    GameState currentGameState;
    [SerializeField] private int round = -1;
    int coinflipNumber1;
    int oddOrEvenNumber;
    bool playerTurn;
    int diceroll;
    bool playerTurnDone;
    bool enemyTurnDone;
    bool playerFirst;

    //[SerializeField] private Slider healthBar;
    //[SerializeField] private Slider manaBar;

    #region init
    private void Awake()
    {
        playerAttackButton.SetActive(false);
        currentGameState = GameState.noFight;
        InitFightUpdate();
    }

    void Update()
    {
        if (currentGameState == GameState.fight)
        {
            CheckTurn(playerTurn);
            //UpdateUI();

            Debug.Log(currentGameState + " | round: " + round + " | " + "playerTurn: " + playerTurn);
        }

        uiManager.GetComponent<TestUiManager>().UpdateUI();
    }
    #endregion

    #region OnClick
    public void OnClickRestart()
    {
        Restart();
    }

    public void OnClickBackToMap()
    {
        uiManager.GetComponent<TestUiManager>().CallBackToMap();
    }

    public void OnClickPlayerAttack()
    {
        PlayerAttack();
    }

    #endregion

    #region Fight

    public void InitStartFight(ScriptableObject enemy)
    {
        Debug.Log("Fight started");

        currentGameState = GameState.fight;

        currentEnemy = enemy;
        InitFightUpdate();

        Debug.Log(currentGameState.ToString());

        coinflipNumber1 = Random.Range(0, 100);
        oddOrEvenNumber = coinflipNumber1 % 2;

        OddOrEven(oddOrEvenNumber);
    }
    
    private void UpdateUI()
    {
        enemyHitPointsText.text = enemyHitPoints.ToString();
        classHitPointsText.text = classHitPoints.ToString();

        whichRoundText.text = round.ToString();
    }

    public void InitFightUpdate()
    {
        if (currentEnemy == null || currentClass == null) return;

        className.text = currentClass.name;
        enemyName.text = currentEnemy.name;

        if (currentEnemy != null)
        {
            if (currentEnemy == rat)
            {
                enemyAttackText.text = rat.attack.ToString();
                enemyImage.sprite = rat.enemySprite;
                enemyAttack = rat.attack;
                enemyHitPointsText.text = rat.hitPoints.ToString();
                enemyHitPoints = rat.hitPoints;
            }
            if (currentEnemy == rat1)
            {
                enemyAttackText.text = rat1.attack.ToString();
                enemyImage.sprite = rat1.enemySprite;
                enemyAttack = rat1.attack;
                enemyHitPointsText.text = rat1.hitPoints.ToString();
                enemyHitPoints = rat1.hitPoints;
            }
            if (currentEnemy == rat2)
            {
                enemyAttackText.text = rat2.attack.ToString();
                enemyImage.sprite = rat2.enemySprite;
                enemyAttack = rat2.attack;
                enemyHitPointsText.text = rat2.hitPoints.ToString();
                enemyHitPoints = rat2.hitPoints;
            }
        }

        if (currentClass != null)
        {
            if (currentClass == fighter)
            {
                classAttackText.text = fighter.attack.ToString();
                classImage.sprite = fighter.classSprite;
                classAttack = fighter.attack;
                classHitPointsText.text = fighter.hitPoints.ToString();
                classHitPoints = fighter.hitPoints;
                classAttackModifier = fighter.fight;
            }
            if (currentClass == thief)
            {
                classAttackText.text = thief.attack.ToString();
                classImage.sprite = thief.classSprite;
                classAttack = thief.attack;
                classHitPointsText.text = thief.hitPoints.ToString();
                classHitPoints = thief.hitPoints;
                classAttackModifier = fighter.dexterity;
            }
            if (currentClass == sorcerer)
            {
                classAttackText.text = sorcerer.attack.ToString();
                classImage.sprite = thief.classSprite;
                classAttack = sorcerer.attack;
                classHitPointsText.text = sorcerer.hitPoints.ToString();
                classHitPoints = sorcerer.hitPoints;
                classAttackModifier = fighter.thinking;
            }

            classAttackModifierText.text = classAttackModifier.ToString();
        }
    }

    private void OddOrEven(int oddOrEvenNumber)
    {
        if (oddOrEvenNumber == 0)
        {
            playerTurn = true;
            playerTurnDone = false;

            Debug.Log("The coinflip was odd.");
        }

        else
        {
            playerTurn = false;
            playerTurnDone = true;
            playerFirst = false;

            Debug.Log("The coinflip was even.");
        }
    }

    private void CheckTurn(bool playerTurn)
    {
        if (playerTurn & !playerTurnDone)
        {
            playerAttackButton.SetActive(true);
        }

        if (!playerTurn & !enemyTurnDone)
        {
            playerAttackButton.SetActive(false);
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
    
    private void RollTheDice()
    {
        diceroll = Random.Range(0, 20);
    }

    private void EnemyAttackPart1()
    {
        if (playerTurn) return;

        RollTheDice();
        Debug.Log("Enemy rolled the dice.");
        StartCoroutine(WaitEnemy(2f));
    }

    private void EnemyAttackPart2()
    {
        if (enemyTurnDone) return;

        if (diceroll >= classAttack)
        {
            classHitPoints = classHitPoints - 1;
            Debug.Log("classHitPoints " + classHitPoints);
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

        RollTheDice();

        if (diceroll + classAttackModifier >= enemyAttack)
        {
            enemyHitPoints = enemyHitPoints - 1;
            Debug.Log("enemyHitPoints " + enemyHitPoints);
            fightText.text = "You hit the enemy with " + diceroll + " " + classAttackModifier + ".";
            CheckConditions();
        }
        if (diceroll + classAttackModifier < enemyAttack)
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
        UpdateUI();

        if (enemyHitPoints <= 0)
        {
            currentGameState = GameState.fightFinished;
            whichRoundText.text = "-";

            fightText.text = "You killed the enemy. Good for you.";
            StartCoroutine(WaitAdventure(5f));
        }
        if (classHitPoints <= 0)
        {
            currentGameState = GameState.fightFinished;
            whichRoundText.text = "-";

            fightText.text = "The enemy wounded you badly. Are you dying?";
            StartCoroutine(WaitAdventure(0.5f));
        }
    }

    private void Restart()
    {
        currentGameState = GameState.noFight;

        currentClass = null;
        currentEnemy = null;
        className.text = "Name";
        enemyName.text = "Name";
        classAttackText.text = "Attack";
        enemyAttackText.text = "Attack";
        classAttack = 0;
        enemyAttack = 0;
        classHitPointsText.text = "Hitpoints";
        enemyHitPointsText.text = "Hitpoints";
        classHitPoints = 0;
        enemyHitPoints = 0;

        fightText.text = "";
        playerAttackButton.SetActive(false);

        //UpdateUI();
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
        uiManager.GetComponent<TestUiManager>().CallBackToMap();
    }

    #endregion

    public void Heal()
    {
        Debug.Log("Your soul is healed. Your body not. Maybe the first one was a lie, sorry.");
    }
}

enum GameState
{
    fight,
    noFight,
    fightFinished
}