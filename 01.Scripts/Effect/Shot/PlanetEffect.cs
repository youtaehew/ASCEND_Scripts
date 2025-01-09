using DG.Tweening;
using UnityEngine;

namespace YTH.Effect
{
    public class PlanetEffect : BossEffectPlay
    {
        // private float startY;
        
        
        private void Start()
        {
            MoveBall();
        }

        public override void ResetItem()
        {
            base.ResetItem();
            MoveBall();
        }

        private void MoveBall()
        {
            Sequence seq = DOTween.Sequence();
            transform.localScale = new Vector3(0, 0, 0);
            // transform.position = new Vector3(0, startY, 0);
            seq.Append(transform.DOScale(0.1f, 1f));
            seq.Append(transform.DOMoveY(transform.position.y + 15, 1.2f));
        }
    }
}