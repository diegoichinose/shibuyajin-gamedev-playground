using System;
using System.IO;
using UnityEngine;

public class SaveFileJsonUtiliy
{
    private string dataDirPath = ""; 
    private string dataFileName = ""; 

    public SaveFileJsonUtiliy(string dataDirPath, string dataFileName) 
    { 
        this.dataDirPath = dataDirPath; 
        this.dataFileName = dataFileName; 
    }

    public SaveFileList ReadLocalSaveFileList()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        SaveFileList loadedData = null; 

        if (!File.Exists(fullPath)) 
            return loadedData;
            
        try 
        { 
            string dataToLoad = ""; 
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            { 
                using (StreamReader reader = new StreamReader(stream))
                { 
                    dataToLoad = reader.ReadToEnd(); 
                }

                loadedData = JsonUtility.FromJson<SaveFileList>(dataToLoad); 
            }
        }
        catch (Exception e) 
        { 
            Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
        } 

        return loadedData;
    } 

    public void WriteLocalSaveFile(SaveFileList dataToSave) 
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try 
        { 
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string  dataToStore = JsonUtility.ToJson(dataToSave, true); 

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) 
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore); 
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        } 
    }
}