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
            Debug.Log("tq 닿았다");
            if (other.gameObject.layer != _whatIsPlayer) return;
            if (other.TryGetComponent(out MInterface.IDamageable health))
            {
                Debug.Log("데미지 준다");
                // CombatData combatData = new CombatData();
                // combatData.damage = power;
                // combatData.damageCategory = Define.EDamageCategory.Normal;
                // combatData.hitPoint = _colliders[i].transform.position;
                health.ApplyDamage(Boss.currentCombatData);
            }
        }
    }
}

