using System.Collections.Generic;
using PJH.Combat;
using PJH.Core;
using PJH.Manager;
using PJH.Scene;
using UnityEngine;
using YTH.Boss;

namespace YTH.Effect
{
    public class EffectCollision : MonoBehaviour
    {
        private ParticleSystem _ps;
        private List<ParticleSystem.Particle> _enterParticles = new();
        public LayerMask _whatIsPlayer;
        [HideInInspector] public BossEnemy Boss;

        private Collider[] _colliders;

        protected virtual void Awake()
        {
            _whatIsPlayer = LayerMask.GetMask("Player");
            
            _ps = GetComponent<ParticleSystem>();
            _colliders = new Collider[2];
            _ps.trigger.AddCollider((Managers.Scene.CurrentScene as GameScene).Player.GetComponent<Collider>());
        }

        private void OnParticleTrigger()
        {
            int cnt = _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enterParticles,
                out var enterColliderData);
            UnityEngine.Debug.Log(cnt);

            for (int i = 0; i < cnt; i++)
            {
                for (int j = 0; j < enterColliderData.GetColliderCount(i); j++)
                {
                    var col = enterColliderData.GetCollider(i, j);
                    if (col.TryGetComponent(out MInterface.IDamageable health))
                    {
                        CombatData combatData = Boss.currentCombatData;
                        combatData.hitPoint = col.transform.position;
                        UnityEngine.Debug.Log(combatData.damage);
                        health.ApplyDamage(combatData);
                    }
                }
            }
        }
    }
}