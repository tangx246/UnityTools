using System.Collections;
using UnityEngine;

namespace UnityTools
{
    public class TimedDestroy : MonoBehaviour
    {
        public float maxLifeSeconds = 3f;
        public bool releaseIntoGameObjectPooler = false;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(maxLifeSeconds);
            if (releaseIntoGameObjectPooler)
            {
                GameObjectPooler.instance.Release(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }
    }
}