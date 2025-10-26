using System.Collections;
using Beam;
using UnityEngine;

namespace Beam
{
    public class BeamLifeTime : MonoBehaviour
    {
        public void StartLifetime(float lifetime)
        {
            StopAllCoroutines();
            StartCoroutine(LifetimeCoroutine(lifetime));
        }

        private IEnumerator LifetimeCoroutine(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            GetComponent<BeamManager>()?.DeActivate();
        }
    }
}