using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Spawned Prop Save Data")]
public class SpawnedPropSaveData : ScriptableObject
{
    public SpawnedPropSaveFile saveFile;
}

[Serializable]
public class SpawnedPropSaveFile
{
    [SerializeReference] public List<SpawnedProp> spawnedProps;

    public void ResetToDefault()
    {
        spawnedProps = new List<SpawnedProp>();
    }
}