using PJH.Combat;
using PJH.Core;
using UnityEngine;

namespace  YTH.Effect
{
    public class LazerEffectCollision : EffectCollision
    {
        private Collider _playerCollider;
        private bool _inside = false;
        private float _currentTime;
        private float _damageTime = 0.3f;
        
        protected override void Awake()
        {
            _whatIsPlayer = LayerMask.GetMask("Player");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            UnityEngine.Debug.Log("레이저 닿았다");
            _playerCollider = other;
            _inside = true;
            _currentTime = 0;
            ApplyDamage();
        }

        private void OnTriggerExit(Collider other)
        {
            UnityEngine.Debug.Log("레이저 나감");
            _inside = false;
            _currentTime = 0; 
        }

        private void Update()
        {
            if (_inside)
            {
                _currentTime += Time.deltaTime;
                if (_currentTime >= _damageTime)
                {
                    ApplyDamage();
                    _currentTime = 0;   
                }
            }
        }

        private void ApplyDamage()
        {
            if (_playerCollider.gameObject.layer != 8) return;
            UnityEngine.Debug.Log("여기까진 뚤리고");
            if (_playerCollider.TryGetComponent(out MInterface.IDamageable health))
            {
                UnityEngine.Debug.Log("딜 안주고");
                CombatData combatData = Boss.currentCombatData;
                combatData.hitPoint = _playerCollider.transform.position;
                UnityEngine.Debug.Log(combatData.damage);
                health.ApplyDamage(combatData);
            }
        }
    }
}

