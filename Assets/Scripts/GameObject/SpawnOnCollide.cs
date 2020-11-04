using UnityEngine;

public class SpawnOnCollide : MonoBehaviour
{
    public GameObject prefabToSpawn;

    public void OnCollisionEnter()
    {
        Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }
}