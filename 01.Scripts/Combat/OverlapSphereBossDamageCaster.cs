using FMODUnity;
using PJH.Combat;
using UnityEngine;
using YTH.Boss;
using static PJH.Core.MInterface;

namespace YTH.Combat
{
    public class OverlapSphereBossDamageCaster : BossDamageCaster
    {
        [SerializeField] private float _castRadius;
        [SerializeField] private EventReference impactSound;

        public override void CastDamage()
        {
            int cnt = Physics.OverlapSphereNonAlloc(_castTrm.position, _castRadius, _colliders, targetLayer);

            if (cnt > 0)
            {
                for (int i = 0; i < cnt; ++i)
                {
                    Debug.Log("피 깍는다");
                    Vector3 hitPos = _colliders[i].ClosestPointOnBounds(_enemy.transform.position);
                    Debug.Log(_colliders[i]);
                    if (_colliders[i].TryGetComponent<IDamageable>(out IDamageable health))
                    {
                        CombatData combatData = _enemy.currentCombatData;
                        combatData.hitPoint = hitPos;
                        health.ApplyDamage(combatData);

                        if (_enemy is BossSlayer bossSlayer)
                        {
                            RuntimeManager.PlayOneShot(impactSound, transform.position);
                            print("사운드");
                        }
                    }
                }
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_castTrm.position, _castRadius);
        }
#endif
    }
}