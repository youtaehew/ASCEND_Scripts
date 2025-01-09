using BehaviorDesigner.Runtime;
using DG.Tweening;
using PJH.Combat;
using System;
using System.Collections.Generic;
using FIMSpace.FProceduralAnimation;
using PJH.Manager;
using PJH.Scene;
using UnityEngine;
using UnityEngine.AI;
using YTH.SO;
using Baek.UI;
using PJH.Core;

namespace YTH.Boss
{
    public enum DistanceType
    {
        Close,
        Middle,
        Far
    }

    public enum BossState
    {
        Die,
        Step1,
        Step2
    }

    public class BossEnemy : MonoBehaviour, IPoolable, MInterface.IEnemy
    {
        [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
        public GameObject GameObject => gameObject;
        public Transform PlayerTrm;
        public AnimParamSO AnimSpeedParamSO;
        [SerializeField] private PopupTypeSO _popupTypeSO;

        private Pool _pool;

        #region Component

        public Health HealthCompo { get; protected set; }

        public Animator AnimatorCompo { get; protected set; }
        public NavMeshAgent NavMeshCompo { get; protected set; }
        public LegsAnimator LegAnimatorCompo { get; protected set; }
        public BehaviorTree BT { get; protected set; }
        public CapsuleCollider Collider { get; protected set; }

        #endregion

        #region Info

        public bool CanRotate;
        public bool IsRootMotion;
        public float DistanceToPlayer { get; protected set; }
        public float DistanceToDestination { get; protected set; }
        public AnimatorStateInfo CurrentAnimation { get; protected set; }

        private BossState currentBossState = BossState.Step1;

        public BossState CurrentBossState
        {
            get => currentBossState;
            set => currentBossState = value;
        }

        #endregion

        #region Stats

        public BossEnemyStat BossStat;
        public CombatData currentCombatData;

        private int _rotateSpeed;

        public int RotateSpeed
        {
            get => _rotateSpeed;
            set => _rotateSpeed = value;
        }

        public Dictionary<DistanceType, int> DistanceDist = new Dictionary<DistanceType, int>();

        #endregion

        #region Action

        public Action JumpEvent;
        public Action AttackEvent;
        public Action StartAttackAction;
        public Action EndAttackAction;
        public Action ChangeAction;
        public Action DieAction;

        #endregion


        public new BossEnemyStat GetStat() => BossStat;

        public virtual void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public virtual void ResetItem()
        {
        }

        protected virtual void Awake()
        {
            PlayerTrm = (Managers.Scene.CurrentScene as GameScene).Player.transform;

            #region ComponetsSetting

            Transform visualTrm = transform.Find("Visual");
            AnimatorCompo = visualTrm.GetComponent<Animator>();
            LegAnimatorCompo = visualTrm.GetComponent<LegsAnimator>();
            BT = GetComponent<BehaviorTree>();
            NavMeshCompo = GetComponent<NavMeshAgent>();
            Collider = GetComponent<CapsuleCollider>();
            HealthCompo = GetComponent<Health>();


            HealthCompo.SetOwner(this);

            #endregion

            #region StatSetting

            Debug.Log(BossStat);
            SettingStat(BossStat);
            CanRotate = true;
            IsRootMotion = true;

            #endregion

            SharedBossEnemy enemy = BT.GetVariable("enemy") as SharedBossEnemy;
            enemy.Value = this;
        }

        protected virtual void Start()
        {
            HealthCompo.DeathEvent += DeadAction;
            Baek.Manager.Managers.UI.ShowPopup(_popupTypeSO);
            Baek.Manager.Managers.UI.PopupDictionary[_popupTypeSO.name].GetComponent<BossHealthPresenterUI>()
                .SetOwner(this);
        }

        private void DeadAction()
        {
            BT.OnDisable();
            CurrentBossState = BossState.Die;
            Collider.enabled = false;
            AnimatorCompo.CrossFadeInFixedTime("Die", 0.2f);
            EnemySpawnManager.Instance.DeleteObject(this);
        }

        private void OnDestroy()
        {
            HealthCompo.DeathEvent += DeadAction;
        }

        private void SettingStat(BossEnemyStat stat)
        {
            HealthCompo.SetMaxHealth(stat.health);
            NavMeshCompo.speed = stat.moveSpeed;
            _rotateSpeed = stat.rotateSpeed;

            DistanceDist.Add(DistanceType.Close, stat.closeDistance);
            DistanceDist.Add(DistanceType.Far, stat.farDistance);
            DistanceDist.Add(DistanceType.Middle, stat.closeDistance);
        }

        protected virtual void Update()
        {
            if (currentBossState == BossState.Die) return;
            CheckToPlayerDistance();
        }

        private void FixedUpdate()
        {
            if (currentBossState == BossState.Die) return;
            if (CanRotate)
                RotateToPlayer();
        }

        private void RotateToPlayer()
        {
            if (PlayerTrm == null) return;
            Vector3 dir = PlayerTrm.position - transform.position;
            dir.y = 0;
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * _rotateSpeed);
        }

        private void CheckToPlayerDistance()
        {
            DistanceToPlayer = Vector3.Distance(PlayerTrm.transform.position, transform.position);
            DistanceToDestination = (NavMeshCompo.destination - transform.position).magnitude;
        }

        public bool AnimationEnd(string name, float exitTime = .9f)
        {
            CurrentAnimation = AnimatorCompo.GetCurrentAnimatorStateInfo(0);
            return CurrentAnimation.IsName(name) && CurrentAnimation.normalizedTime >= exitTime;
        }

        public void LookPlayer()
        {
            Vector3 dir = (PlayerTrm.transform.position - transform.position).normalized;
            dir.y = 0;
            Quaternion look = Quaternion.LookRotation(dir);
            transform.DORotateQuaternion(look, 1f);
        }

        public virtual void SetCombatData(AtkInfoSO atkInfoSO)
        {
            currentCombatData = new CombatData()
            {
                damage = atkInfoSO.damage,
                hitPoint = Vector3.zero,
                knockBackDir = (PlayerTrm.position - transform.position).normalized,
                knockBackPower = 1,
                knockBackDuration = 0.3f,
                damageCategory = Define.EDamageCategory.Normal
            };
        }

        #region SetParameters

        public void SetParam(AnimParamSO param, bool value)
            => AnimatorCompo.SetBool(param.hashValue, value);

        public void SetParam(AnimParamSO param, float value)
            => AnimatorCompo.SetFloat(param.hashValue, value);

        public void SetParam(AnimParamSO param, float value, float damp, float deltaTime)
        {
            AnimatorCompo.SetFloat(param.hashValue, value, damp, deltaTime);
        }

        public void SetParam(AnimParamSO param, int value)
            => AnimatorCompo.SetInteger(param.hashValue, value);

        public void SetParam(AnimParamSO param)
            => AnimatorCompo.SetTrigger(param.hashValue);

        #endregion
    }

    public class SharedBossEnemy : SharedVariable<BossEnemy>
    {
        public static implicit operator SharedBossEnemy(BossEnemy value)
        {
            return new SharedBossEnemy { Value = value };
        }
    }
}