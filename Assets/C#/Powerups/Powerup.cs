using UnityEngine;

public abstract class Powerup : ScriptableObject
{
    public PowerupType type;
    
    [Header("UI")]
    public string powerup_name;
    public Sprite icon;
    public string description;
    
    [Header("Rare")]
    public float rarity = 100f;
}

public enum PowerupType
{
    StatBoost,
    BulletModifer,
    GunModifier
}