using UnityEngine;

[CreateAssetMenu(menuName = "Create new item")]
public class Item : ScriptableObject
{
    public int id = -1;
    public string itemName;
    public Sprite itemSprite;
    public ItemType type;
    public ItemRarity rarity;

    public string description;

    public int goldValue;

    public bool equipable;
    public bool consumable;
    public ItemEffect effect;
    public Die die;

    //public bool randomEvent;
    public RandomEvent randomEvent;

    public int buff;
    public int debuff;

}
public enum ItemType
{
    Weapon,
    Armor,
    Potion,
    Food,
    etc
}

public enum ItemRarity
{
    common,
    uncommon,
    rare,
    mythical,
    legendary
}

public enum ItemEffect
{
    Heal,
    Damage,
    RandomEvent,
    Coin
}

public enum RandomEvent
{
    None,
    HealOrDamage
}

public enum Die
{
    None,
    d12
}
