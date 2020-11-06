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
        public List<GameObject> initialPrefabs;

        private Dictionary<GameObject, Queue<GameObject>> prefabToInstantiatedObjects = new Dictionary<GameObject, Queue<GameObject>>();
        private Queue<GameObject> prefabsToInstantiate = new Queue<GameObject>(); // Inefficient. Could use a counter for each prefab instead. Observe and update if necessary
        private Coroutine instantiatorCoroutine;

        private const int CHUNK_SIZE = 100;

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            foreach (var prefab in initialPrefabs)
            {
                QueueInstantiation(prefab, CHUNK_SIZE);
            }
        }

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
            while (prefabsToInstantiate.Count > 0)
            {
                var prefab = prefabsToInstantiate.Dequeue();
                var instantiated = Instantiate(prefab);
                instantiated.SetActive(false);
                AddToPool(prefab, instantiated);

                yield return 0;
            }

            instantiatorCoroutine = null;
        }

        public GameObject Get(GameObject prefab)
        {
            GameObject returnVal;

            // If the prefab isn't in the pool, or the pool has been emptied. Increase the size of the pool
            if (!prefabToInstantiatedObjects.ContainsKey(prefab) || prefabToInstantiatedObjects[prefab].Count == 0) 
            {
                returnVal = Instantiate(prefab);
                QueueInstantiation(prefab, CHUNK_SIZE);
            } else
            {
                returnVal = prefabToInstantiatedObjects[prefab].Dequeue();
                QueueInstantiation(prefab, 1);
            }

            returnVal.SetActive(prefab.activeSelf);
            return returnVal;
        }

        private void QueueInstantiation(GameObject prefab, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                prefabsToInstantiate.Enqueue(prefab);
            }

            StartInstantiator();
        }

        private void AddToPool(GameObject prefab, GameObject instantiated)
        {
            if (!prefabToInstantiatedObjects.ContainsKey(prefab))
            {
                prefabToInstantiatedObjects[prefab] = new Queue<GameObject>();
            }

            prefabToInstantiatedObjects[prefab].Enqueue(instantiated);
        }
    }
}
