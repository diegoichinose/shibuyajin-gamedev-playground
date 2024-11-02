using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions 
{
    private static Random rng = new Random();  
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;
     
        while (n > 1) 
        {
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    public static void AddIfNotExists<T>(this List<T> list, T value)
    {
        if (!list.Contains(value))
            list.Add(value);
    }

    public static void AddIfNotExists<T>(this List<T> list, T value, Func<T, bool> filter)
    {
        if (!list.Where(filter).Any())
            list.Add(value);
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        return list[rng.Next(list.Count)];
    }

    public static T GetRandom<T>(this T[] array)
    {
        return array[rng.Next(array.Length)];
    }
}