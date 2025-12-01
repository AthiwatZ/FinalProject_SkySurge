using UnityEngine;
using System.Collections.Generic;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] List<UpgradeCard> pool = new();
    List<UpgradeCard> currentChoices = new();

    [Header("Rarity Chance (%)")]
    public float chanceCommon = 60f;
    public float chanceRare = 25f;
    public float chanceEpic = 10f;
    public float chanceLegendary = 5f;

    public List<UpgradeCard> RollChoices(int playerLv, int waveIdx)
    {
        List<UpgradeCard> result = new List<UpgradeCard>();

        for (int i = 0; i < 3; i++)
        {
            CardRarity wanted = RollRarity();
            List<UpgradeCard> poolByRarity = pool.FindAll(c => c.rarity == wanted);

            if (poolByRarity.Count == 0)
                poolByRarity = pool; // ถ้าระดับนั้นไม่มี ให้สุ่มทั้งหมดแทน

            var card = poolByRarity[Random.Range(0, poolByRarity.Count)];
            result.Add(card);
        }

        return result;
    }

    CardRarity RollRarity()
    {
        float roll = Random.value * 100f;

        if (roll < chanceLegendary) return CardRarity.Legendary;
        if (roll < chanceLegendary + chanceEpic) return CardRarity.Epic;
        if (roll < chanceLegendary + chanceEpic + chanceRare) return CardRarity.Rare;
        return CardRarity.Common;
    }

    public void ApplyUpgrade(Player player, UpgradeCard card)
    {
        player.maxHp += card.addMaxHp;
        player.hp = Mathf.Min(player.hp + card.addMaxHp, player.maxHp);
        player.moveSpeed += card.addMoveSpeed;

        if (player.weapon != null)
        {
            player.weapon.damageBonus += card.addDamage;
            player.weapon.fireRate *= card.fireRateMultiplier;
        }

        // อัปเดต HUD อีกที
        GameManager.I.ui.UpdateHUD(player.hp, player.maxHp, player.lv ,player.exp, player.ExpToNextLevel(),
                                   GameManager.I.waveMgr.WaveIndex,
                                   GameManager.I.score);
    }
}
