using UnityEngine;

public class OnApplicationQuitSaveGame : MonoBehaviour
{
    void OnApplicationQuit()
    {
        PersistentDataManager.instance.SaveGame();
    }
}