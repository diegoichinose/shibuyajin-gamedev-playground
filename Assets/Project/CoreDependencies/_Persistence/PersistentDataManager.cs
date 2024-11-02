using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    public static PersistentDataManager instance { get; private set; }
    [SerializeField] private PersistentDataList _persistentDataList;
    [SerializeField] private SettingsData _settingsData;

    
    [Header("DO NOT TOUCH - FOR INSPECTION ONLY")]
    public SaveFileList localSaveFileList;
    public int currentlySelectedSaveSlotIndex;
    private SaveFileJsonUtiliy saveFileJsonUtility;
    private string fileName = "SaveData.json";
    public bool isLocalSaveFileNotLoaded => localSaveFileList == null || localSaveFileList.list.Count() == 0;
    public bool isLocalSaveFileLoaded => isLocalSaveFileNotLoaded == false;

    void Awake()
    {
        if (instance != null) 
            Debug.LogError("Found more than one Data Persistance Manager in the scene");

        instance = this;
        saveFileJsonUtility = new SaveFileJsonUtiliy(Application.persistentDataPath, fileName);

        TryLoadLocalSaveFileList();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void TryLoadLocalSaveFileList()
    {
        if (isLocalSaveFileNotLoaded)
            localSaveFileList = saveFileJsonUtility.ReadLocalSaveFileList();
            
        if (localSaveFileList != null)
            _settingsData.settings = localSaveFileList.settings;
    }

    public void CreateNewSaveData()
    {
        // CREATE NEW SAVE FILE
        var saveFile = new SaveFile(currentlySelectedSaveSlotIndex);
        
        // FEED DATA WITH THE NEW SAVE FILE "EMPTY" VALUES
        foreach (IPersistentData persistentData in _persistentDataList.everyExistingPersistentData)
        {
            persistentData.LoadData(saveFile);
            persistentData.OnNewSave(); // USE THIS TO FEED DATA WITH DEFAULT VALUES
        }

        SaveGame(saveFile);
    }
    
    public void TryLoadGame(int saveSlotIndex)
    {   
        currentlySelectedSaveSlotIndex = saveSlotIndex;
        SaveFile selectedSaveFile;

        if (isLocalSaveFileNotLoaded)
            localSaveFileList = saveFileJsonUtility.ReadLocalSaveFileList();

        if (isLocalSaveFileNotLoaded) 
        {
            Debug.Log("NO LOCAL SAVE FILE FOUND");
            CreateNewSaveData();
            
            return;
        }
        else
        {
            selectedSaveFile = localSaveFileList.GetSaveFile(saveSlotIndex);

            if (selectedSaveFile == null || selectedSaveFile.Equals(default(SaveFile)))
            {
                Debug.Log("SELECTED SAVE SLOT " + saveSlotIndex + " WAS EMPTY");
                CreateNewSaveData();
                
                return;
            }
        }

        foreach (IPersistentData persistentData in _persistentDataList.everyExistingPersistentData)
        { 
            persistentData.LoadData(selectedSaveFile);
        }
    }
    
    public void SaveGame()
    {
        var saveFile = new SaveFile(currentlySelectedSaveSlotIndex);

        // FILL saveFile VARIABLE WITH CURRENT DATA (from "SaverLoaders" scriptable objects that inherit from IPersistenData)
        foreach (IPersistentData persistentData in _persistentDataList.everyExistingPersistentData)
        { 
            persistentData.SaveData(saveFile);
        }

        SaveGame(saveFile);
    }

    private void SaveGame(SaveFile saveFile)
    {
        // LOAD CURRENT LOCAL SAVE FILE
        if (isLocalSaveFileNotLoaded)
            localSaveFileList = saveFileJsonUtility.ReadLocalSaveFileList();

        // IF NO LOCAL SAVE FILE IS FOUND, CREATE NEW ONE
        if (isLocalSaveFileNotLoaded) 
            localSaveFileList = new SaveFileList(saveFile);
        else
        {
            // IF SAVE SLOT EXIST, REPLACE IT (SAME INDEX), IF NOT, ADD IT
            int index = localSaveFileList.list.FindIndex(x => x.saveSlotIndex == currentlySelectedSaveSlotIndex);

            if (index != -1)
                localSaveFileList.list[index] = saveFile;
            else
                localSaveFileList.list.Add(saveFile);
        }

        localSaveFileList.settings = _settingsData.settings;

        saveFileJsonUtility.WriteLocalSaveFile(localSaveFileList);
    }

    public void DeleteSaveSlot(int saveSlotIndex)
    {
        if (isLocalSaveFileNotLoaded)
            localSaveFileList = saveFileJsonUtility.ReadLocalSaveFileList();

        if (isLocalSaveFileNotLoaded) 
        {
            Debug.LogWarning("NO LOCAL SAVE FILE FOUND: There is nothing to delete here, lil bro");
            return;
        }
        
        localSaveFileList.list.RemoveAll(x => x.saveSlotIndex == saveSlotIndex);
        saveFileJsonUtility.WriteLocalSaveFile(localSaveFileList);
    }
}

[Serializable]
public class SaveFileList
{
    public Settings settings;
    public List<SaveFile> list;
    public SaveFile GetSaveFile(int saveSlotIndex) => list.FirstOrDefault(x => x.saveSlotIndex == saveSlotIndex);

    public SaveFileList(SaveFile saveFile)
    {
        settings = new Settings();
        list = new List<SaveFile>
        {
            saveFile
        };
    }
}