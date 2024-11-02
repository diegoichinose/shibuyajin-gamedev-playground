using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Item Data/Consumable/Health Potion")]
public class HealthPotionData : ConsumableItemData
{
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private int healthPointsToRecover;

    protected override bool TryConsume()
    {
        AudioManager.instance.sfxLibrary.PlayConsumeHealthPotionSound();
        _playerData.player.health.Heal(healthPointsToRecover);

        return true;
    }
}