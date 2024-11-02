using System;
using UnityEngine;

[Serializable]
public class IPersistentData : ScriptableObject
{
    public Action OnSaveData;
    public Action OnLoadData;

    public virtual void OnNewSave(){}
    
    public virtual void SaveData(SaveFile saveFile)
    {
        OnSaveData?.Invoke();
    }
    
    public virtual void LoadData(SaveFile saveFile)
    {
        OnLoadData?.Invoke();
    }
}