using BehaviorDesigner.Runtime;
using DG.Tweening;
using UnityEngine;
using YTH.Boss;

namespace YTH.BT.Actions
{
    public class JumpAction : BaseAction
    {
        public SharedBossEnemy enemyBase;
        public Collider _collider;
        [SerializeField] private AnimationCurve _animationCurve;

        public override void OnStart()
        {
            base.OnStart();
            _collider = (enemyBase.Value as BossSorcerer).MapCollider;
            if (_collider == null)
            {
                _collider = GameObject.Find("Level/GroundBound").GetComponent<Collider>();
            }
            enemyBase.Value.JumpEvent += Jump;
            enemyBase.Value.IsRootMotion = false;
            enemyBase.Value.CanRotate = true;
        }

        public void Jump()
        {
            Vector3 destination = enemyBase.Value.PlayerTrm.position;

            float minDistance = 5f;
            float maxAttempts = 100;
            int attempts = 0;

            do
            {
                float x = Random.Range(_collider.bounds.min.x, _collider.bounds.max.x);
                float z = Random.Range(_collider.bounds.min.z, _collider.bounds.max.z);
                destination = new Vector3(x, transform.position.y, z);

                attempts++;
            } while (Vector3.SqrMagnitude(destination - enemyBase.Value.PlayerTrm.position) < minDistance * minDistance
                     && attempts < maxAttempts);

            Vector3[] movePos =
            {
                transform.position,
                Vector3.Lerp(transform.position, destination, 0.5f) + Vector3.up * 3f,
                destination
            };

            transform.DOPath(movePos, 1f, pathType: PathType.CatmullRom).SetEase(_animationCurve);

            // Debug.Log(DOCurve.CubicBezier.GetPointOnSegment(transform.position, new Vector3(0, 0.69f), enemyBase.Value.PlayerTrm.position,
            //     new Vector3(0.19f, 0.93f), 1));
        }

        public override void OnEnd()
        {
            base.OnEnd();
            enemyBase.Value.IsRootMotion = true;
            enemyBase.Value.JumpEvent -= Jump;
        }
    }
}