using UnityEngine;

namespace UnityTools
{
    public class RecursiveStaticGameObject : MonoBehaviour
    {
        public void OnValidate()
        {
            RecursiveStatic(gameObject);
        }

        private void RecursiveStatic(GameObject go)
        {
            go.isStatic = true;

            foreach (Transform child in go.transform)
            {
                RecursiveStatic(child.gameObject);
            }
        }
    }
}