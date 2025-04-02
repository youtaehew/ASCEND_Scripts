using System;
using Baek.Manager;
using Cysharp.Threading.Tasks;
using FIMSpace.FProceduralAnimation;
using PJH.Scene;
using UnityEngine;
using YTH.Boss;
using Managers = PJH.Manager.Managers;

namespace YTH.Effect
{
    public class BossEffect : MonoBehaviour
    {
        protected PoolManagerSO _poolManager;
        [SerializeField] private PoolTypeSO[] _EffectPoolType;
        [SerializeField] private Transform[] _effectTrm;
        protected LegsAnimator _legAnimator;
        protected BossEnemy _bossEnemy;
        protected Transform _playerTrm;


        private void Awake()
        {
            _poolManager = Managers.Addressable.Load<PoolManagerSO>("PoolManager");
            _legAnimator = GetComponent<LegsAnimator>();
        }

        protected virtual void Start()
        {
            _playerTrm = (Managers.Scene.CurrentScene as GameScene).Player.transform;
            _bossEnemy = GetComponentInParent<BossEnemy>();
        }

        protected async void PlayEffect(int number)
        {
            _legAnimator.enabled = false;
            await UniTask.NextFrame();
            var effect = _poolManager.Pop(_EffectPoolType[number]) as BossEffectPlay;
            var pos = _effectTrm[number];
            effect.SetupBossEnemy(_bossEnemy);
            effect.transform.SetPositionAndRotation(pos.position, transform.parent.rotation);
            effect.PlayEffects();
            _legAnimator.enabled = true;
        }
    }
}
