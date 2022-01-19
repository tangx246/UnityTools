using UnityEngine;

namespace UnityTools
{
    [RequireComponent(typeof(LookAtPlayerConstraint))]
    public class GPUProgressBar : MonoBehaviour
    {
        public MeshRenderer bar;
        public bool visibleWhenFull = false;

        private MaterialPropertyBlock propertyBlock;

        public void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        public void Start()
        {
            bar.gameObject.SetActive(visibleWhenFull);
        }

        /**
         * Fill the bar. 1 for full, 0 for empty
         */
        public void SetBarFill(float barFill)
        {
            bar.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_Cutoff", 1 - barFill);
            bar.SetPropertyBlock(propertyBlock);

            // Full HP
            if (barFill >= 1)
            {
                if (visibleWhenFull)
                {
                    bar.gameObject.SetActive(true);
                }
                else
                {
                    bar.gameObject.SetActive(false);
                }
            }
            // HP above 0
            else if (barFill > 0)
            {
                bar.gameObject.SetActive(true);
            }
            // Dead
            else
            {
                bar.gameObject.SetActive(false);
            }
        }
    }
}