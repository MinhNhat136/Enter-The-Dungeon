using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Atomic.Character
{
    public class SimpleDeadBody 
    {
        [SerializeField]
        private Rigidbody[] rigids;

        [SerializeField]
        private Collider[] colliders;

        [SerializeField]
        private Renderer[] renderers;

        private readonly Dictionary<int, Vector3> _localPositions = new Dictionary<int, Vector3>();
        private readonly Dictionary<int, Quaternion> _localRotations = new Dictionary<int, Quaternion>();
        private float _force; 
        
        public void Initialize(float force, float expansionRatio)
        {
            for (int i = 0; i < rigids.Length; i++)
            {
                _localPositions[i] = rigids[i].transform.localPosition;
                _localRotations[i] = rigids[i].transform.localRotation;
            }

            _force = force;
        }

        public void Setup(Vector3 pos, bool ragdollLayer)
        {
            for (int i = 0; i < rigids.Length; i++)
            {
                rigids[i].transform.localPosition = _localPositions[i];
                rigids[i].transform.localRotation = _localRotations[i];
            }

            
            if (ragdollLayer)
            {
                foreach (var collider in colliders)
                {
                    collider.gameObject.layer = LayerMask.NameToLayer("Ragdoll");
                }
            }
        }
        
        public  void Explode()
        {
            foreach (var rigid in rigids)
            {
                Vector3 explosionForce = Random.insideUnitSphere * _force;
                rigid.AddForce(explosionForce, ForceMode.Impulse);
            }

            // StartCoroutine(FadeOut());
        }

        private System.Collections.IEnumerator FadeOut()
        {
            float fadeDuration = 2f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

                foreach (var renderer in renderers)
                {
                    foreach (var material in renderer.materials)
                    {
                        Color color = material.color;
                        color.a = alpha;
                        material.color = color;
                    }
                }

                yield return null;
            }

            // gameObject.SetActive(false);
        }
    }
}
