using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetDirectory<T> : ScriptableObject where T : ScriptableObject
{
    public List<T> assets = new List<T>();
    public readonly Dictionary<string, T> nameToAsset = new Dictionary<string, T>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        assets = AssetDatabase.FindAssets($"t: {typeof(T).Name}").ToList()
                     .Select(AssetDatabase.GUIDToAssetPath)
                     .Select(AssetDatabase.LoadAssetAtPath<T>)
                     .ToList();
    }
#endif

    public void OnEnable()
    {
        foreach (var asset in assets)
        {
            nameToAsset.Add(asset.name, asset);
        }
    }
}
