using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace UnityTools
{
    public class GameObjectPooler : MonoBehaviour
    {
        private static GameObjectPooler _instance;
        public static GameObjectPooler instance { 
            get 
            {
                if (_instance == null)
                {
                    var gameObject = new GameObject(typeof(GameObjectPooler).Name);
                    _instance = gameObject.AddComponent<GameObjectPooler>();
                }

                return _instance;
            } }
        public List<GameObject> initialPrefabs = new List<GameObject>();

        private class PooledGameObject : MonoBehaviour
        {
            public GameObject originalPrefab;
        }

        private Dictionary<GameObject, ObjectPool<GameObject>> prefabToPools = new Dictionary<GameObject, ObjectPool<GameObject>>();

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            foreach (var prefab in initialPrefabs)
            {
                AddPool(prefab);
            }
        }

        public GameObject Get(GameObject prefab)
        {
            if (!prefabToPools.ContainsKey(prefab)) 
            {
                AddPool(prefab);
            }

            return prefabToPools[prefab].Get();
        }

        public void Release(GameObject prefabInstance)
        {
            var pooledGameObjectComponent = prefabInstance.GetComponent<PooledGameObject>();
            var prefab = pooledGameObjectComponent.originalPrefab;

            if (!prefabToPools.ContainsKey(prefab))
            {
                Debug.LogWarning("Releasing a prefabInstance that doesn't have a pool", prefabInstance);
                return;
            }

            prefabToPools[prefab].Release(prefabInstance);
        }

        private void AddPool(GameObject prefab)
        {
            if (!prefabToPools.ContainsKey(prefab))
            {
                var gameObjectPool = new ObjectPool<GameObject>(() => CreatePooledItem(prefab), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
                prefabToPools.Add(prefab, gameObjectPool);
            }
        }

        private GameObject CreatePooledItem(GameObject prefab)
        {
            var instantiated = Instantiate(prefab);
            var pooledGameObjectComponent = instantiated.AddComponent<PooledGameObject>();
            pooledGameObjectComponent.originalPrefab = prefab;
            return instantiated;
        }

        private void OnTakeFromPool(GameObject go)
        {
            go.SetActive(true);
        }

        private void OnDestroyPoolObject(GameObject go)
        {
            Destroy(go);
        }

        private void OnReturnedToPool(GameObject go)
        {
            go.SetActive(false);
        }
    }
}