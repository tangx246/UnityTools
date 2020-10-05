using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UnityTools
{
    public class DrainingResource : Resource<float>
    {
        public float resourceDrainRate = 0.0033f; // Have to eat every 5 minutes
        public float resourceTickRateSeconds = 1f;

        private Coroutine coroutine;
        public UnityEvent resourceOut = new UnityEvent();
        public UnityEvent resourceFilled = new UnityEvent();

        private void OnEnable()
        {
            coroutine = StartCoroutine(DrainResource());
        }

        private void OnDisable()
        {
            StopCoroutine(coroutine);
        }

        public IEnumerator DrainResource()
        {
            while (true)
            {
                value -= resourceDrainRate;
                value = Math.Max(0, value);

                if (value <= 0)
                {
                    resourceOut.Invoke();
                }
                else if (value >= 1)
                {
                    resourceFilled.Invoke();
                }
                yield return new WaitForSeconds(resourceTickRateSeconds);
            }
        }
    }
}
