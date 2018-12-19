using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListHandling
{
    public static IEnumerable<T> RandomizeList<T>(List<T> elements)
    {
        var array = elements.ToArray();
        var result = new List<T>();

        for (var l = array.Length; l > 0; l--)
        {
            var i = Random.Range(0, l);
            var e = array[i];
            array[i] = array[l - 1];
            result.Add(e);
        }
        return result;
    }

    public static void UpdateDictionary<K, V>(Dictionary<K, V> dict, K key, V value)
    {
        if (dict.ContainsKey(key)) dict[key] = value;
        else dict.Add(key, value);
    }
}
