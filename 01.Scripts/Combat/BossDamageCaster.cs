using UnityEngine;
using UnityEngine.Events;
using YTH.Boss;

namespace YTH.Combat
{
    public abstract class BossDamageCaster : MonoBehaviour
    {
        public LayerMask targetLayer;
        public UnityEvent OnCastDamageEvent;
        [SerializeField] protected Transform _castTrm;
        
        protected BossEnemy _enemy;
        protected Collider[]  _colliders;
        private int _maxColliderCount = 1; 
        
        public void InitDamageCast(BossEnemy enemy)
        {
            _enemy = enemy;
            _colliders = new Collider[_maxColliderCount];
        }
        public abstract void CastDamage();
    }
}

