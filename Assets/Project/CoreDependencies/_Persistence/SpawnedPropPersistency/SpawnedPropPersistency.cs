using System;
using UnityEngine;

public class SpawnedPropPersistency : MonoBehaviour
{
    [SerializeField] protected SpawnedPropSaveData _spawnedPropSaveData;
    [SerializeField] private bool autoSaveOnStart;
    protected SpawnedProp spawnedProp;

    void Start()
    {
        if (autoSaveOnStart)
            AddSelfToSaveData(transform);
    }

    public virtual SpawnedProp GetSpawnedProp()
    {
        return spawnedProp;
    }
    
    public virtual void SetSpawnedProp(SpawnedProp spawnedProp)
    {
        this.spawnedProp = spawnedProp;
    }

    public virtual SerializableGuid AddSelfToSaveDataFromCrafting(Transform transform) 
    {
        return AddSelfToSaveData(transform.position);
    } 

    public virtual SerializableGuid AddSelfToSaveData(Transform transform)
    {
        return AddSelfToSaveData(transform.position);
    }
    
    public virtual SerializableGuid AddSelfToSaveData(Vector3 position)
    {
        spawnedProp = new SpawnedProp(Guid.NewGuid(), gameObject.GetPrefabName(), position);
        _spawnedPropSaveData.saveFile.spawnedProps.Add(spawnedProp);
        return spawnedProp.id;
    }

    public virtual void RemoveSelfFromSaveData()
    {
        if (spawnedProp == null)
            return;

        _spawnedPropSaveData.saveFile.spawnedProps.RemoveAll(x => x.id == spawnedProp.id);
    }

    public virtual void OnPropInstantiatedFromSaveData() { }
    public virtual void OnPropInstantiatedFromNewRun() { }
}