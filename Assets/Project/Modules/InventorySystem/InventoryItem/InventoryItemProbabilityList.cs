using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class InventoryItemProbabilityList
{
    public List<InventoryItemProbability> list;
    [SerializeField] private int minAmount;
    [SerializeField] private int additionalAmountToRandomize;
    [SerializeField] private PlayerData _playerData;

    public List<InventoryItem> GetWeightedRandom()
    {
        if (list == null || list.Count == 0)
            return new List<InventoryItem>();

        var listToUse = list.FindAll(x => x.inventoryItem.inventoryItemData.CanSpawn()).ToList();

        int amount = minAmount + Random.Range(0, additionalAmountToRandomize);

        List<InventoryItem> result = new List<InventoryItem>();

        for (int i = 0; i < amount; i++)
        {
            int chanceSumCounter = 0;
            int randomPercentageRoll = Random.Range(1, GetTotalWeight(listToUse));
            
            var listToUseThisIteration = listToUse.ToList();
            listToUseThisIteration.ForEach(item =>
            {
                if (randomPercentageRoll > chanceSumCounter)
                if (randomPercentageRoll <= chanceSumCounter + item.weight)
                {
                    result.Add(item.inventoryItem);
                    item.inventoryItem.inventoryItemData.OnSpawnConfirmed();

                    if (item.preventRepetition)
                        listToUse.Remove(item);
                }

                chanceSumCounter += item.weight;
            });
        }
        
        return result;
    }

    private int GetTotalWeight(List<InventoryItemProbability> list)
    {
        int total = 0;

        list.ForEach(item => 
        {
            if (item.weight <= 0)
                item.weight = 1;

            total += item.weight;
        });

        return total;
    }
}


[Serializable]
public class InventoryItemProbability
{
    public InventoryItem inventoryItem;
    public int weight = 10;
    public bool preventRepetition;

    public InventoryItemProbability(InventoryItem inventoryItem, bool preventRepetition = false)
    {
        this.inventoryItem = inventoryItem;
        this.weight = 10;
        this.preventRepetition = preventRepetition;
    }

    public InventoryItemProbability(InventoryItemData inventoryItemData, bool preventRepetition = false)
    {
        this.inventoryItem = new InventoryItem(inventoryItemData, quantity: 1);
        this.weight = 10;
        this.preventRepetition = preventRepetition;
    }
}