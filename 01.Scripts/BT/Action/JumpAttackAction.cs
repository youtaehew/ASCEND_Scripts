using DG.Tweening;
using UnityEngine;

namespace YTH.BT.Actions
{
    public class JumpAttackAction : BaseAction
    {
        [SerializeField] private AnimationCurve _animationCurve;
        public override void OnStart()
        {
            base.OnStart();
            enemyBase.Value.JumpEvent += JumpAttack;
            enemyBase.Value.IsRootMotion = false;
            enemyBase.Value.CanRotate = true;
        }

        public void JumpAttack()
        {
            Vector3 playerPos = enemyBase.Value.PlayerTrm.position;
            // playerPos.y = 0;
            Vector3[] movePos =
            {
                transform.position,
                Vector3.Lerp(transform.position, playerPos, 0.5f) + Vector3.up * 3f,
                playerPos
            };
            
            transform.DOPath(movePos, 0.6f, pathType: PathType.CatmullRom).SetEase(_animationCurve);
            
            // Debug.Log(DOCurve.CubicBezier.GetPointOnSegment(transform.position, new Vector3(0, 0.69f), enemyBase.Value.PlayerTrm.position,
            //     new Vector3(0.19f, 0.93f), 1));
        }

        public override void OnEnd()
        {
            base.OnEnd();
            enemyBase.Value.IsRootMotion = true;
            enemyBase.Value.JumpEvent -= JumpAttack;
        }
    }
}