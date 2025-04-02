using PJH.Core;
using UnityEngine;

namespace YTH.Effect
{
    public class ProjectileEffectCollision : EffectCollision
    {
        protected override void Awake()
        {
            _whatIsPlayer = LayerMask.GetMask("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
          
            if (other.gameObject.layer != _whatIsPlayer) return;
            if (other.TryGetComponent(out MInterface.IDamageable health))
            {
                health.ApplyDamage(Boss.currentCombatData);
            }
        }
    }
}

