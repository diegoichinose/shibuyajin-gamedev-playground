using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Data List/Tool Prefix Data List")]
public class ToolPrefixDataList : ScriptableObject
{
    public List<ToolPrefixData> everyExistingToolPrefixData;
    public ToolPrefixData Find(SerializableGuid id) => everyExistingToolPrefixData.Find(x => x.id == id);
    public ToolPrefixData GetRandom() => everyExistingToolPrefixData[Random.Range(0, everyExistingToolPrefixData.Count)];
}