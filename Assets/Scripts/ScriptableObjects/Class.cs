using UnityEngine;

[CreateAssetMenu(menuName = "Create new Class")]
public class Class : ScriptableObject
{
    public int id = -1;
    public string className;
    public Sprite classSprite;
    public Sprite classSpriteRound;
    public Sprite classMapfigure_Base;
    public Sprite classMapfigure_noBase;

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
}
