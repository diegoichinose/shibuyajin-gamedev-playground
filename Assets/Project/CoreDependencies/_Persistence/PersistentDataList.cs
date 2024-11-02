using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Data List/Persistent Data List")]
public class PersistentDataList : ScriptableObject
{
    public List<IPersistentData> everyExistingPersistentData;
}