using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityTools
{
    [RequireComponent(typeof(LookAtConstraint))]
    public class LookAtPlayerConstraint : MonoBehaviour
    {
        public Vector3 rotationOffset = Vector3.zero;
        public bool constantUpdate = true;
        public float constantUpdateRefreshSeconds = 1f;

        private LookAtConstraint lookAtConstraint;
        private Camera foundCam;

        private void Awake()
        {
            lookAtConstraint = GetComponent<LookAtConstraint>();
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(LookForCameraCoroutine());
        }

        private IEnumerator LookForCameraCoroutine()
        {
            var cameraInitialized = false;
            while (!cameraInitialized || constantUpdate)
            {
                var mainCamera = Camera.main;
                if (mainCamera != foundCam)
                {
                    foundCam = mainCamera;
                    var constraintSources = new List<ConstraintSource>(1);
                    var constraintSource = new ConstraintSource()
                    {
                        sourceTransform = mainCamera.transform,
                        weight = 1
                    };
                    constraintSources.Add(constraintSource);
                    lookAtConstraint.SetSources(constraintSources);
                    lookAtConstraint.rotationOffset = rotationOffset;
                    lookAtConstraint.constraintActive = true;
                }

                yield return constantUpdate ? new WaitForSeconds(constantUpdateRefreshSeconds) : 0;
            }
        }
    }
}