using System.Collections;
using Beam;
using UnityEngine;

namespace Beam
{
    public class BeamLifeTime : MonoBehaviour
    {
        public void StartLifetime(float lifetime)
        {
            // 既存のコルーチンを止めてから新しいものを開始
            StopAllCoroutines();
            StartCoroutine(LifetimeCoroutine(lifetime));
        }

        private IEnumerator LifetimeCoroutine(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            // このゲームオブジェクトにアタッチされているBeamクラスに非アクティブ化を通知
            GetComponent<BeamManager>()?.DeActivate();
        }
    }
}