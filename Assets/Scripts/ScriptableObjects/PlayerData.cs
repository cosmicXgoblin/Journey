using UnityEngine;

[CreateAssetMenu(menuName = "Create new PlayerData")]
public class PlayerData : ScriptableObject
{
    // wip

    public int id = -1;
    public string className;
    public Sprite classSprite;
    public Sprite classSpriteRound;
    public Sprite mapFigure_Base;
    public Sprite mapFigure_noBase;

    [Range(0, 10)]
    public int fight;

    [Range(0, 10)]
    public int thinking;

    [Range(0, 10)]
    public int speed;

    [Range(0, 10)]
    public int observing;

    [Range(0, 10)]
    public int dexterity;

    [Range(0, 10)]
    public int charme;

    public int attack;
    public int attackModifier;
    public int maxHitPoints;
    public int currentHitPoints;

    public Item Item0;
    public Item Item1;
    public Item Item2;
    public Item Item3;
    public Item Item4;
    public Item Item5;
    public Item Item6;
    public Item Item7;
    public Item Item8;

    public Transform lastLocation;
    public int gold;

    /* 
     public GameObject lastCamp;
     
    */

}
