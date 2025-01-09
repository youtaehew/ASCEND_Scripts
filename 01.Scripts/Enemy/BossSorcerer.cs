using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using PJH.Combat;
using PJH.Core;
using PJH.Manager;
using UnityEngine;
using YTH.SO;

namespace YTH.Boss
{
    public class BossSorcerer : BossEnemy
    {
        public Collider MapCollider;
        private int _maxSpawnEnemy = 0;
        private float _currentTime = 0;
        [SerializeField] private PoolTypeSO[] _enemyPooltypes;
        private List<Enemy> _enemies = new List<Enemy>();

        protected override void Start()
        {
            base.Start();
            
            MapCollider = GameObject.Find("Level/GroundBound").GetComponent<Collider>();
            _enemies.Add(EnemySpawnManager.Instance.SpawnEnemy(_enemyPooltypes[Random.Range(0, 9)]));
            _enemies.Add(EnemySpawnManager.Instance.SpawnEnemy(_enemyPooltypes[Random.Range(0, 9)]));
            _enemies.Add(EnemySpawnManager.Instance.SpawnEnemy(_enemyPooltypes[Random.Range(0, 9)]));
        }

        protected override void Update()
        {
            base.Update();
            if (CurrentBossState != BossState.Die)
            {
                if (_enemies.Count <= 6)
                {
                    _currentTime += Time.deltaTime;
                    if (_currentTime >= 20)
                    {
                        for (int i = 0; i < Random.Range(1, 3); i++)
                        {
                            _enemies.Add(EnemySpawnManager.Instance.SpawnEnemy(_enemyPooltypes[Random.Range(0, 9)]));
                        }

                        // _enemies.Add(EnemySpawnManager.Instance.SpawnEnemy(_enemyPooltypes[Random.Range(0, 9)]));
                        _currentTime = 0;
                    }
                }

                for (int i = 0; i < _enemies.Count; i++)
                {
                    if (_enemies[i].HealthCompo.IsDead)
                        _enemies.Remove(_enemies[i]);
                }
            }
        }

        public override void SetCombatData(AtkInfoSO atkInfoSO)
        {
            currentCombatData = new CombatData()
            {
                damage = atkInfoSO.damage,
                hitPoint = Vector3.zero,
                knockBackDir = Vector3.zero,
                // knockBackPower = 1,
                // knockBackDuration = 0.5f,
                damageCategory = Define.EDamageCategory.Normal
            };
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetStat().closeDistance);
        }
#endif
    }
}