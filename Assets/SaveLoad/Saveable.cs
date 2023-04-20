using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;

public class Saveable : MonoBehaviour, ISaveable
{
    public string guid;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(guid) && !string.IsNullOrEmpty(gameObject.scene.path))
        {
            guid = System.Guid.NewGuid().ToString();
        }
    }

    [System.Serializable]
    private struct TransformData
    {   
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    public object Save()
    {
        // Gather all ISaveables and turn them into a Dictionary to be JSON-ified
        Dictionary<System.Type, string> saveables = new();
        var components = GetComponentsInChildren<ISaveable>(); ;
        foreach (var component in components)
        {
            if (component is not Saveable)
            {
                saveables.Add(component.GetType(), component.ToJson());
            }
        }
        saveables.Add(typeof(TransformData), JsonUtility.ToJson(new TransformData()
        {
            position = transform.position,
            rotation = transform.rotation,
            scale = transform.localScale
        }));
        return saveables;
    }

    public void Load(object saveData)
    {
        // Parse dictionary and fill up each ISaveable
        Dictionary<System.Type, string> saveables = (Dictionary<System.Type, string>)saveData;
        foreach (var saveable in saveables)
        {
            if (saveable.Key == typeof(TransformData))
            {
                var transformData = JsonUtility.FromJson<TransformData>(saveable.Value);
                var nt = GetComponent<NetworkTransform>();
                if (nt != null)
                {
                    var navMeshAgent = GetComponent<NavMeshAgent>();
                    bool navMeshAgentEnabled = false;
                    if (navMeshAgent != null)
                    {
                        navMeshAgentEnabled = navMeshAgent.enabled;
                        navMeshAgent.enabled = false;
                    }
                    nt.Teleport(transformData.position, transformData.rotation, transformData.scale);
                    if (navMeshAgent != null)
                        navMeshAgent.enabled = navMeshAgentEnabled;
                }
            }
            else
            {
                var component = GetComponentInChildren(saveable.Key);
                if (component == null)
                {
                    Debug.LogError($"Unable to find {saveable.Key} for component {guid}");
                    continue;
                }

              (component as ISaveable).FromJson(saveable.Value);
            }
        }
    }
}