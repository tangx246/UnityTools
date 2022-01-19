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

        public void Awake()
        {
            lookAtConstraint = GetComponent<LookAtConstraint>();
        }

        public IEnumerator Start()
        {
            // Look at player
            lookAtConstraint = GetComponent<LookAtConstraint>();

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