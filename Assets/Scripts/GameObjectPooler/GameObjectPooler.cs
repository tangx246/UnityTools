using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private const int CHUNK_SIZE = 100;
        private class GameObjectPool : MonoBehaviour
        {
            public GameObject prefab;
            private Queue<GameObject> instantiatedObjects = new Queue<GameObject>();
            private int prefabsToInstantiate = CHUNK_SIZE;
            private Coroutine instantiatorCoroutine;

            public void Start()
            {
                StartInstantiator();
            }

            private void StartInstantiator()
            {
                if (instantiatorCoroutine == null)
                {
                    instantiatorCoroutine = StartCoroutine(PrefabInstantiator());
                }
            }

            private IEnumerator PrefabInstantiator()
            {
                while (prefabsToInstantiate > 0)
                {
                    var instantiated = Instantiate(prefab, transform);
                    instantiated.SetActive(false);
                    instantiatedObjects.Enqueue(instantiated);

                    prefabsToInstantiate--;
                    yield return 0;
                }

                instantiatorCoroutine = null;
            }

            public GameObject Get()
            {
                GameObject returnVal;

                // If pool is empty
                if (instantiatedObjects.Count == 0)
                {
                    returnVal = Instantiate(prefab);

                    // If prefabsToInstantiate is less than a certain amount, we are fairly confident we should expand the pool
                    if (prefabsToInstantiate < CHUNK_SIZE / 2)
                    {
                        QueueInstantiation(CHUNK_SIZE);
                    }
                } else
                {
                    returnVal = instantiatedObjects.Dequeue();
                    QueueInstantiation(1);
                }

                returnVal.SetActive(prefab.activeSelf);
                return returnVal;
            }

            private void QueueInstantiation(int quantity)
            {
                prefabsToInstantiate += quantity;
                StartInstantiator();
            }
        }

        private Dictionary<GameObject, GameObjectPool> prefabToPools = new Dictionary<GameObject, GameObjectPool>();

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

        private void AddPool(GameObject prefab)
        {
            if (!prefabToPools.ContainsKey(prefab))
            {
                var poolObject = new GameObject(typeof(GameObjectPool).Name);
                poolObject.transform.parent = instance.transform;
                var poolComponent = poolObject.AddComponent<GameObjectPool>();
                poolComponent.prefab = prefab;
                prefabToPools.Add(prefab, poolComponent);
            }
        }
    }
}