using UnityEngine;

public enum CardRarity { Common, Rare, Epic , Legendary }

[CreateAssetMenu(menuName = "SkySurge/UpgradeCard")]
public class UpgradeCard : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;

    public CardRarity rarity;

    // àÍ¿à¿¡µì
    public int addMaxHp;
    public int addDamage;
    public float addMoveSpeed;
    public float fireRateMultiplier = 1f;
}
