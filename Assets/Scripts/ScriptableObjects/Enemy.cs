using Ink.Parsed;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Create new Enemy")]
public class Enemy : ScriptableObject
{
    public int id = -1;
    public string enemyName;
    public Sprite enemySprite;

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

    public List<Item> loot = new List<Item>();
}
