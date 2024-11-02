using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class InventoryItemDataProbabilityList
{
    public List<InventoryItemDataProbability> list;
    [SerializeField] private int minAmount;
    [SerializeField] private int additionalAmountToRandomize;
    
    public List<InventoryItemData> GetRandom(int amount = 1)
    {
        var result = new List<InventoryItemData>();
        var listToUse = list.FindAll(x => x.inventoryItemData.CanSpawn()).ToList();
        var randomIndex = 0;
        
        for (int i = 0; i < amount; i++)
        {
            randomIndex = Random.Range(0, listToUse.Count - 1);
            result.Add(listToUse[randomIndex].inventoryItemData);
            listToUse.RemoveAt(randomIndex);
        }

        return result;
    }

    public List<InventoryItemData> GetWeightedRandom()
    {
        var listToUse = list.FindAll(x => x.inventoryItemData.CanSpawn()).ToList();

        int amount = minAmount + Random.Range(0, additionalAmountToRandomize);

        if (amount == 0)
            return new List<InventoryItemData>();
            
        int chanceTotal = 0;
        listToUse.ForEach(item => 
        {
            if (item.weight <= 0)
                item.weight = 1;

            chanceTotal += item.weight;
        });
        
        List<InventoryItemData> result = new List<InventoryItemData>();

        for (int i = 0; i < amount; i++)
        {
            int randomPercentageRoll = Random.Range(1, chanceTotal);
            int chanceSumCounter = 0;
            
            listToUse.ForEach(item =>
            {
                if (randomPercentageRoll > chanceSumCounter)
                if (randomPercentageRoll <= chanceSumCounter + item.weight)
                {
                    result.Add(item.inventoryItemData);
                    item.inventoryItemData.OnSpawnConfirmed();
                }

                chanceSumCounter += item.weight;
            });
        }
        
        return result;
    }
}


[Serializable]
public class InventoryItemDataProbability
{
    public InventoryItemData inventoryItemData;
    public int weight = 10;
}