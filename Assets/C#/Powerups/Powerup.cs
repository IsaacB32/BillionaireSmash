using UnityEngine;

public abstract class Powerup : ScriptableObject
{
    public PowerupType type;
    
    [Header("UI")]
    public string powerup_name;
    public Sprite icon;
    public string description;
}

public enum PowerupType
{
    StatBoost,
    BulletModifer,
    GunModifier
}