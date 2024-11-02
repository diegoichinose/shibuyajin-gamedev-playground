using System;
using UnityEngine;

[Serializable]
public class SpawnedProp
{
    public SerializableGuid id;
    public string prefabName;
    public Vector3 position;
    public bool alwaysLoaded;

    public SpawnedProp()
    {
        
    }

    public SpawnedProp(SerializableGuid id, string prefabName, Vector3 position, bool alwaysLoaded = false)
    {
        this.id = id;
        this.prefabName = prefabName;
        this.position = position;
        this.alwaysLoaded = alwaysLoaded;
    }
}