using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedDictionary<TKey, TValue>
{
    [SerializeField]
    List<SerializedDictionaryItem<TKey, TValue>> dictionary;

    public SerializedDictionary()
    {
        dictionary = new();
    }
    public Dictionary<TKey, TValue> ToDictionary()
    {
        Dictionary<TKey, TValue> newDict = new Dictionary<TKey, TValue>();

        foreach(var item in dictionary)
        {
            newDict.Add(item.key, item.value);
        }

        return newDict;
    }

    public void Add(TKey key, TValue value) => dictionary.Add(new SerializedDictionaryItem<TKey, TValue>(key, value));
    public void Clear() => dictionary.Clear();
}

[Serializable]
class SerializedDictionaryItem<TKey, TValue>
{
    [SerializeField]
    public TKey key;
    [SerializeField]
    public TValue value;

    public SerializedDictionaryItem(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}