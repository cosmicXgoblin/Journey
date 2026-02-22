using Ink.Parsed;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    public List<string> tutorial;
    public int tutorialIndex = 1;
    public TextMeshProUGUI tutorialText;
    public UnityEngine.UI.Button buttonNext;

    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject _outline1;
    [SerializeField] private GameObject _outline2;
    [SerializeField] private GameObject _outline3;

    public static Tutorial Instance { get; private set; }
    public GameObject tutorialPanel => _tutorialPanel;

    private void Awake()
    {
        
        if (Instance == null)
          Instance = this;
        else Destroy(this);

        DisableOutlines();

        // tutorialIndex is starting with 1
        tutorial.Add("I’ll guide you through everything you see on the screen.");
        tutorial.Add("Leftclick on ok to proceed.");
        tutorial.Add("This is where your choices are displayed.");                                                          // 3
        tutorial.Add("You can select them via WASD and [space] on your keyboard...");
        tutorial.Add("... or via left mouseclick.");

        tutorial.Add("Try it out!");                                                                                        // 6
        tutorial.Add("This is where you can choose your class and therefore your character.");                              // 7
        tutorial.Add("Every character is different and through playing you’ll uncover their personality and story.");
        tutorial.Add("For the tutorial, the differences are not that big.");
        tutorial.Add("If you’re wondering about their stats, check your players handbook.");                                // 10

        tutorial.Add("At this stage in development, you will be teleported to the locations you have to go.");
        tutorial.Add("More freedom is coming with a more developed version of this Demo.");                                 // 12
        tutorial.Add("These are the items you can buy.");
        tutorial.Add("After you click on them, the shopkeeper will tell you their cost and you can decide.");               // 14
        tutorial.Add("This is your inventory.");

        tutorial.Add("Bought items are automatically added.");
        tutorial.Add("If you click on an item in your inventory, you get more options with it.");                           // 17
        tutorial.Add("This is your money.");
        tutorial.Add("At later stages, you can get money from quest, random events or through loot.");
        tutorial.Add("For now, you only have 100G.");

        tutorial.Add("Spend them wise!");                                                                                   // 21
        tutorial.Add("Sometimes, enemies will attack you for choices you made in game.");
        tutorial.Add("Othertimes, you will find them while exploring the map.");
        tutorial.Add("When in battle, the only important thing for now is to attack and hope.");
        tutorial.Add("When attacking, you're rolling a D20.");                                                              

        tutorial.Add("Your baseattack and your attackmodifier have to be higher...");
        tutorial.Add("... than the enemies baseattack");
        tutorial.Add("These calculations can change during developement, so make sure...");
        tutorial.Add("... to revisit the tutorial or read the updatenotes.");                         // 29
    }

    public void OnClickNext()
    {
        //tutorialText.text = tutorial[tutorialIndex];

        if (tutorialIndex == 2 || tutorialIndex == 6 || tutorialIndex == 12)
        {
            DisableOutlines();
            _outline1.SetActive(true);
        }
        if (tutorialIndex == 6 || tutorialIndex == 10)
        {
            //DisableOutlines();
            _tutorialPanel.SetActive(false);
        }
        if (tutorialIndex == 21)
        {
            DisableOutlines();
            _tutorialPanel.SetActive(false);
        }
        else if (tutorialIndex == 29)
        {
            DisableOutlines();
            _tutorialPanel.SetActive(false);
            GameManager.Instance.SetBattle("rat");
        }
        else if (tutorialIndex == 14)
        {
            DisableOutlines();
            _outline2.SetActive(true);
        }
        else if (tutorialIndex == 17)
        {
            DisableOutlines();
            _outline3.SetActive(true);
        }

        tutorialText.text = tutorial[tutorialIndex];
        tutorialIndex++;
    }

    public void DisableOutlines()
    {
        _outline1.SetActive(false);
        _outline2.SetActive(false);
        _outline3.SetActive(false);
    }
   
}
