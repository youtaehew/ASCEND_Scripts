using TrailsFX;
using UnityEngine;
using YTH.Combat;

namespace YTH.Boss
{
    public class BossSlayer : BossEnemy
    {
        private OverlapBoxBossDamageCaster _boxDamageCaster;
        // [SerializeField]private ParticleSystem _trailEffect;

        [SerializeField] private TrailEffect _trailEffect;

        protected override void Awake()
        {
            base.Awake();
            Transform casterTrm = transform.Find("DamageCaster");
            _boxDamageCaster = casterTrm.GetComponent<OverlapBoxBossDamageCaster>();
            _boxDamageCaster.InitDamageCast(this);

            StartAttackAction += OnTrail;
            EndAttackAction += OffTrail;
            ChangeAction += ChangeStep2;
        }

        protected override void Start()
        {
            base.Start();
            AttackEvent += DamageCast;
        }

        private void DamageCast()
        {
            _boxDamageCaster.CastDamage();
        }

        private void OnDestroy()
        {
            AttackEvent -= DamageCast;
        }

        private void ChangeStep2()
        {
            NavMeshCompo.speed = NavMeshCompo.speed + 2;
        }

        private void OnTrail()
        {
            _trailEffect.active = true;
            // _trailEffect.Play();
        }

        private void OffTrail()
        {
            _trailEffect.active = false;
            // _trailEffect.Stop();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetStat().closeDistance);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, GetStat().middleDistance);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, GetStat().farDistance);
        }
#endif
    }
}