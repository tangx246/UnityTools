using UnityEngine;

namespace UnityTools
{
    public class TimedDestroy : MonoBehaviour
    {
        public float maxLifeSeconds = 3f;

        void Start()
        {
            Destroy(gameObject, maxLifeSeconds);
        }
    }
}