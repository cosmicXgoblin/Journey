using UnityEngine;

[CreateAssetMenu(menuName = "Create new Class")]
public class Class : ScriptableObject
{
    public int id = -1;
    public string className;

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
    public int hitPoints;
}
