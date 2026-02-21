using Ink.Parsed;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    public List<string> tutorial;
    public int tutorialIndex = 0;
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
     
        tutorial.Add("I’ll guide you through everything you see on the screen.");
        tutorial.Add("Leftclick on ok to move proceed.");
        tutorial.Add("...");   // 3
        tutorial.Add("This is where your choices are displayed.");
        tutorial.Add("You can select them via WASD and [space] on your keyboard...");

        tutorial.Add("... or via left mouseclick.");
        tutorial.Add("Try it out!");
        tutorial.Add("");   // 8
        tutorial.Add("This is where you can choose your class and therefore your character.");
        tutorial.Add("");   // 10

        tutorial.Add("Every character is different and through playing you’ll uncover their personality and story.");
        tutorial.Add("For the tutorial, the differences are not that big.");
        tutorial.Add("If you’re wondering about their stats, check your players handbook.");
        tutorial.Add("...");   // 14
        tutorial.Add("At this stage in development, you will be teleported to the locations you have to go.");

        tutorial.Add("More freedom is coming with a more developed version of this Demo.");
        tutorial.Add("...");   // 17
        tutorial.Add("These are the items you can buy.");
        tutorial.Add("After you click on them, the shopkeeper will tell you their cost and you can decide.");
        tutorial.Add("...");   // 20

        tutorial.Add("This is your inventory.");
        tutorial.Add("Bought items are automatically added.");
        tutorial.Add("If you click on an item in your inventory, you get more options with it.");
        tutorial.Add("...");   // 24
        tutorial.Add("This is your money.");

        tutorial.Add("At later stages, you can get money from quest, random events or through loot.");
        tutorial.Add("For now, you only have 100G.");
        tutorial.Add("Spend them wise!");
        tutorial.Add("..."); // 28
    }

    public void OnClickNext()
    {
        tutorialText.text = tutorial[tutorialIndex];

        if (tutorialIndex == 2 || tutorialIndex == 9 || tutorialIndex == 18)
        {
            _outline1.SetActive(true);
        }
        else if (tutorialIndex == 8 || tutorialIndex == 14 || tutorialIndex == 28)
        {
            DisableOutlines();
            _tutorialPanel.SetActive(false);
        }
        else if (tutorialIndex == 20 || tutorialIndex == 24)
        {
            DisableOutlines();
        }
        else if (tutorialIndex == 21)
        {
            _outline2.SetActive(true);
        }
        else if (tutorialIndex == 25)
        {
            _outline3.SetActive(true);
        }

        tutorialIndex++;
    }

    private void DisableOutlines()
    {
        _outline1.SetActive(false);
        _outline2.SetActive(false);
        _outline3.SetActive(false);
    }
   
}
