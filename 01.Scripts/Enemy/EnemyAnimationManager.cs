using Cysharp.Threading.Tasks;
using UnityEngine;
using YTH.Boss;

namespace YTH.Manager
{
    public class EnemyAnimationManager : MonoBehaviour
    {
        private BossEnemy _enemyBase;
        private Animator _animator;
        private void Start()
        {
            _enemyBase = transform.GetComponentInParent<BossEnemy>();
            _animator = _enemyBase.AnimatorCompo;
        }

        public void JumpAttack() => _enemyBase.JumpEvent?.Invoke();

        public async void Attack()
        {
            _enemyBase.LegAnimatorCompo.enabled = false;
            await UniTask.WaitForFixedUpdate();
            _enemyBase.AttackEvent?.Invoke();
            _enemyBase.LegAnimatorCompo.enabled = true;
        }

        private void OnAnimatorMove()
        {
            if (!_enemyBase.IsRootMotion) return;
            Vector3 targetPos = _animator.rootPosition;
            if (!_enemyBase)
            {
                transform.position = targetPos;
                return;
            }

            targetPos.y = _enemyBase.NavMeshCompo.nextPosition.y;
            _enemyBase.transform.position = targetPos;
            _enemyBase.NavMeshCompo.nextPosition = targetPos;
        }

        private void StartAttack()
        {
            _enemyBase.CanRotate = false;
            _enemyBase.StartAttackAction();
        }

        private void EndAttack()
        {
            _enemyBase.CanRotate = true;
            _enemyBase.EndAttackAction();
        }

        private void OnNavmesh()
        {
            _enemyBase.NavMeshCompo.enabled = true;
            _enemyBase.NavMeshCompo.SetDestination(_enemyBase.PlayerTrm.position);
        }

        private void PlayEffect()
        {
        }

        private void Die()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}