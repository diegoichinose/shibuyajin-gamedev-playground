using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventoryItemDataList<T> : ScriptableObject where T : InventoryItemData
{
    public List<T> list;
    public List<InventoryItemDataList<T>> listOfLists;
    public T Find(SerializableGuid id) => GetCombinedList().Find(x => x.id == id);
    public T GetRandom() => GetCombinedList()[UnityEngine.Random.Range(0, GetCombinedList().Count - 1)];
    private List<T> combinedListCache;

    public List<T> GetCombinedList()
    {
        if (combinedListCache != null)
        if (combinedListCache.Count > 0)
            return combinedListCache.ToList(); 

        List<T> combinedList = list.ToList();
        listOfLists.ForEach(listofList => combinedList.AddRange(listofList.list.ToList()));
        combinedListCache = combinedList.ToList();

        return combinedListCache.ToList();
    }

    public void RefreshInventoryItemsDataListCache() 
    {
        combinedListCache = null;
        GetCombinedList();
    }
}