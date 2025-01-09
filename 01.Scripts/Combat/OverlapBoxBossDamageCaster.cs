using FMODUnity;
using PJH.Combat;
using PJH.Core;
using UnityEngine;
using YTH.Boss;

namespace YTH.Combat
{
    public class OverlapBoxBossDamageCaster : BossDamageCaster
    {
        [SerializeField] private Vector3 _boxSize;

        [SerializeField] private Transform _extraBoxTrm;
        [SerializeField] private Vector3 _extarBoxSize;

        [SerializeField] private EventReference impactSound;

        public override void CastDamage()
        {

            int cnt = Physics.OverlapBoxNonAlloc(_castTrm.position, _boxSize, _colliders, _castTrm.rotation,
                targetLayer);
            if (cnt <= 0)
            {
                cnt = Physics.OverlapBoxNonAlloc(_extraBoxTrm.position, _extarBoxSize, _colliders, _extraBoxTrm.rotation,
                    targetLayer);
            }
            if (cnt > 0)
            {
                for (int i = 0; i < cnt; ++i)
                {
                    Debug.Log("플레이어 맞음");
                    Vector3 hitPos = _colliders[i].ClosestPointOnBounds(_enemy.transform.position);
                    Debug.Log(_colliders[i]);
                    if (_colliders[i].TryGetComponent<MInterface.IDamageable>(out MInterface.IDamageable health))
                    {
                        _enemy.currentCombatData.hitPoint = hitPos;
                        CombatData combatData = _enemy.currentCombatData;
                        health.ApplyDamage(combatData);

                        if (_enemy is BossSlayer s)
                        {
                            print("사운드 재생");
                            RuntimeManager.PlayOneShot(impactSound, transform.position);
                        }

                    }
                }
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_castTrm == null) return;
            Gizmos.color = Color.magenta;
            Gizmos.matrix = _castTrm.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _boxSize);

            if (_extraBoxTrm == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.matrix = _extraBoxTrm.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _extarBoxSize);
        }
#endif
    }
}