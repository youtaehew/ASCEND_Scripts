using Cysharp.Threading.Tasks;
using FMODUnity;
using PJH.Manager;
using PJH.Scene;
using UnityEngine;
using YTH.Boss;

namespace YTH.Effect
{
    public class SocererEffect : BossEffect
    {
        [SerializeField] private PoolTypeSO[] _ToPlayerEffectPool;
        [SerializeField] private PoolTypeSO _chargeShotEffect;
        [SerializeField] private Transform _chargeShotTrm;

        [SerializeField] private EventReference _blackHoldeSfx;
        [SerializeField] private EventReference _chargeShotSfx;
        [SerializeField] private EventReference _planetShotSfx;
        [SerializeField] private EventReference _lazerSfx;

        private async void PlayEffectToPlayer(int number)
        {
            _legAnimator.enabled = false;
            await UniTask.NextFrame();

            Vector3 pos = _playerTrm.position;
            pos.y += 0.1f;
            var effect = _poolManager.Pop(_ToPlayerEffectPool[number]) as BossEffectPlay;
            effect.SetupBossEnemy(_bossEnemy);
            effect.transform.SetPositionAndRotation(pos, transform.parent.rotation);
            effect.PlayEffects();
            _legAnimator.enabled = true;
        }

        private async void PlayChargeShotEffect()
        {
            _legAnimator.enabled = false;
            await UniTask.NextFrame();

            Vector3 pos = _chargeShotTrm.position;
            var effect = _poolManager.Pop(_chargeShotEffect) as BossEffectPlay;
            effect.SetupBossEnemy(_bossEnemy);
            effect.transform.SetPositionAndRotation(pos, transform.parent.rotation);
            effect.PlayEffects();
            _legAnimator.enabled = true;
        }
        private async void PlayPlanetEffect()
        {
            _legAnimator.enabled = false;
            await UniTask.NextFrame();

            Vector3 pos = _playerTrm.position;
            var effect = _poolManager.Pop(_ToPlayerEffectPool[3]) as BossEffectPlay;
            effect.transform.SetPositionAndRotation(pos, transform.parent.rotation);
            effect.PlayEffects();
            _legAnimator.enabled = true;
        }

        private void PlayRainEffect()
        {
            PlayEffectToPlayer(0);
        }

        private void PlayThrowBallEffect()
        {
            PlayEffect(1);
        }

        private async void PlayDebuffEffect()
        {
            for (int i = 0; i < 3; i++)
            {
                PlayEffectToPlayer(0);
                await UniTask.WaitForSeconds(1f);
            }
        }

        private void PlayBlackholeEffect()
        {
                PlayEffectToPlayer(1);
            RuntimeManager.PlayOneShot(_blackHoldeSfx, _playerTrm.position);
        }

        private void PlayBeamEffect()
        {
            PlayEffect(0);
            RuntimeManager.PlayOneShot(_lazerSfx, _playerTrm.position);
        }

        private void PlayChargeShot()
        {
            RuntimeManager.PlayOneShot(_chargeShotSfx, _playerTrm.position);
            PlayChargeShotEffect();
        }

        private void PlayPlanetShot()
        {
            PlayPlanetEffect();
        }

        private void PlayMakePlanet()
        {
            RuntimeManager.PlayOneShot(_planetShotSfx, _playerTrm.position);
            PlayEffect(2);
        }
    }
}
