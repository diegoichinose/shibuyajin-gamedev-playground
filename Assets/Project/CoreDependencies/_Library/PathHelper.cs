using System.Collections.Generic;
using UnityEngine;

public class PathHelper
{
    int randomPath;
    List<int> usedPaths = new List<int>();

    public Vector2 GetRandomNearbyPosition(Vector2 origin) => GetRandomWihoutRepeating(GetNearbyPositions(origin));

    public List<Vector2> GetNearbyPositions(Vector2 origin)
    {
        List<Vector2> list = new List<Vector2>();
        list.Add(new Vector2(origin.x + 0f,     origin.y + 0f));
        list.Add(new Vector2(origin.x + 0.3f,   origin.y + 0f));
        list.Add(new Vector2(origin.x + -0.3f,  origin.y + 0f));
        list.Add(new Vector2(origin.x + 0.5f,   origin.y + 0f));
        list.Add(new Vector2(origin.x + -0.5f,  origin.y + 0f));
        list.Add(new Vector2(origin.x + 0f,  origin.y + 0.3f));
        list.Add(new Vector2(origin.x + 0f,  origin.y + -0.3f));
        list.Add(new Vector2(origin.x + 0f,  origin.y + 0.5f));
        list.Add(new Vector2(origin.x + 0f,  origin.y + -0.5f));
        list.Add(new Vector2(origin.x + 0.3f,   origin.y + 0.3f));
        list.Add(new Vector2(origin.x + 0.5f,   origin.y + 0.5f));
        list.Add(new Vector2(origin.x + -0.3f,   origin.y + -0.3f));
        list.Add(new Vector2(origin.x + -0.5f,   origin.y + -0.5f));
        return list;
    }

    public Vector2 GetRandomWihoutRepeating(List<Vector2> vectors) 
    {
        if (usedPaths.Count == vectors.Count)
            usedPaths = new List<int>();

        do {
            randomPath = Random.Range(0, vectors.Count);
        } while (usedPaths.Contains(randomPath));
        usedPaths.Add(randomPath);

        return vectors[randomPath];
    }

    public Vector3 GetRandomPathPositionWihoutRepeating(Transform[] paths) 
    {
        if (usedPaths.Count == paths.Length)
            usedPaths = new List<int>();

        do {
            randomPath = Random.Range(0, paths.Length);
        } while (usedPaths.Contains(randomPath));
        usedPaths.Add(randomPath);

        return paths[randomPath].position;
    }
    
    public Vector3 GetRandomPathPositionWihoutRepeating(GameObject[] paths) 
    {
        if (usedPaths.Count == paths.Length)
            usedPaths = new List<int>();

        do {
            randomPath = Random.Range(0, paths.Length);
        } while (usedPaths.Contains(randomPath));
        usedPaths.Add(randomPath);

        return paths[randomPath].transform.position;
    }
}