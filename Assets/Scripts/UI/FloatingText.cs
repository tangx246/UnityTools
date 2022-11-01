using UnityEngine;

namespace UnityTools
{
    public class FloatingText : MonoBehaviour
    {
        public Vector3 velocity = new Vector3(0, 0.1f, 0);

        public void OnEnable()
        {
            var camera = Camera.main;
            if (camera != null)
            {
                transform.LookAt(camera.transform);
            }
        }

        void Update()
        {
            transform.position += velocity * Time.deltaTime;
        }
    }
}