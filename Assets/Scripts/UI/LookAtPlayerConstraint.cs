using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityTools
{
    [RequireComponent(typeof(LookAtConstraint))]
    public class LookAtPlayerConstraint : MonoBehaviour
    {
        public Vector3 rotationOffset = Vector3.zero;
        private LookAtConstraint lookAtConstraint;

        private void Awake()
        {
            lookAtConstraint = GetComponent<LookAtConstraint>();
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(LookForCamera());
        }

        private IEnumerator LookForCamera()
        {
            var cameraInitialized = false;
            while (!cameraInitialized)
            {
                if (Camera.main != null)
                {
                    var constraintSource = new ConstraintSource()
                    {
                        sourceTransform = Camera.main.transform,
                        weight = 1
                    };
                    lookAtConstraint.AddSource(constraintSource);
                    lookAtConstraint.rotationOffset = rotationOffset;
                    lookAtConstraint.constraintActive = true;
                    cameraInitialized = true;
                }

                yield return 0;
            }
        }
    }
}