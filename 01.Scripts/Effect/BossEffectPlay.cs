using FMODUnity;
using NSubstitute.ReceivedExtensions;
using UnityEngine;
using UnityEngine.Serialization;
using YTH.Boss;

namespace YTH.Effect
{
    public class BossEffectPlay : PoolEffectPlayer
    {
        [SerializeField]
        private bool isSlef = false;
        private EffectCollision _effectCollision;

       //[SerializeField] private EventReference _sfx;

        public override void SetUpPool(Pool pool)
        {
            base.SetUpPool(pool);
            if (isSlef == true)
            {
                _effectCollision = GetComponent<EffectCollision>();
            }
            else
            {
                _effectCollision = GetComponentInChildren<EffectCollision>(true);
            }
        }

        public override void PlayEffects(Vector3 position, Quaternion rotation)
        {
            base.PlayEffects(position, rotation);
            //if (!_sfx.IsNull)
            //{
            //    RuntimeManager.PlayOneShotAttached(_sfx, gameObject);
            //}
        }

        public void SetupBossEnemy(BossEnemy bossEnemy)
        {
            if (_effectCollision != null)
            {
                _effectCollision.Boss = bossEnemy;
            }
        }
    }
}