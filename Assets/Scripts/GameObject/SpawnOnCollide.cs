using UnityEngine;
using UnityTools;

public class SpawnOnCollide : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public bool useObjectPool = true;

    public void OnCollisionEnter()
    {
        if (useObjectPool)
        {
            var instantiated = GameObjectPooler.instance.Get(prefabToSpawn);
            instantiated.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
        else
        {
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
    }
}