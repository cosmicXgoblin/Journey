using UnityEngine;

public class Database : MonoBehaviour
{
    [Header("Classes")]
    public Class fighter;
    public Class thief;
    public Class sorcerer;

    [Header("Enemies")]
    public Enemy rat;
    public Enemy rat1;
    public Enemy rat2;

    [Header("Potraits")]
    public Sprite Narrator;
    public Sprite Shopkeeper;
    public Sprite Tavernkeeper;
    public Sprite Fighter;
    public Sprite Thief;
    public Sprite Sorcerer;

    [Header("Backgrounds")]
    public Sprite Shop;
    public Sprite Village;
    public Sprite Tavern;
    public Sprite None;

    [Header("Items")]
    public Item Apple;
    public Item Cheese;
    public Item MysteryPotion;
    public Item PotionOfHealing;
    public Item PotionOfStrength;
    public Item Sword;
    public Item sharpSword;

    public static Database Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Found more than Database in the scene.");
    }
}
