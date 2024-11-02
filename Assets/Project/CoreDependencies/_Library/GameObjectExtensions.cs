using UnityEngine;

public static class GameObjectExtensions
{
    // WARNING! SCUFFED HOTFIX TO USE PREFAB NAMES FOR JSON PERSISTENCY. REFACTOR TO ADDRESSABLES IN THE FUTURE AND GET RID OF THIS
    public static string GetPrefabName(this GameObject thisGameObject) => thisGameObject.name.Split("(")[0];

    public static void DeleteAllChildren(this GameObject thisGameObject)
    {
        foreach (Transform children in thisGameObject.transform)
        {
            GameObject.Destroy(children.gameObject);
        }
    }

    public static void DeleteAllChildren(this GameObject thisGameObject, float timer)
    {
        foreach (Transform children in thisGameObject.transform)
        {
            GameObject.Destroy(children.gameObject, timer);
        }
    }
}