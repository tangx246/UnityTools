using System.Collections;
using UnityEngine;

namespace UnityTools
{
    public class TimedDestroy : MonoBehaviour
    {
        public float maxLifeSeconds = 3f;
        public bool releaseIntoGameObjectPooler = false;

        void OnEnable()
        {
            StartCoroutine(StartDestroy());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator StartDestroy()
        {
            yield return new WaitForSeconds(maxLifeSeconds);
            if (releaseIntoGameObjectPooler)
            {
                GameObjectPooler.instance.Release(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}