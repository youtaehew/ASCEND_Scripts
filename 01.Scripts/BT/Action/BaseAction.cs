using YTH.Boss;
using YTH.SO;

namespace YTH.BT.Actions
{
    using BehaviorDesigner.Runtime.Tasks;

    public class BaseAction : Action
    {
        public SharedBossEnemy enemyBase;
        public AnimInfoSO animInfoSO;
        public AtkInfoSO atkInfoSO;

        public float blend = 1;
        public float EndAnimTime = 0.9f;

        public override void OnStart()
        {
            // enemyBase.Value.LegAnimatorCompo.User_FadeToDisabled(0);//legsAnimator
            // enemyBase.Value.AnimatorCompo.SetFloat("AnimSpeed", animInfoSO.AnimSpeed);
            enemyBase.Value.RotateSpeed = enemyBase.Value.GetStat().attackRotateSpeed;
            enemyBase.Value.SetParam(enemyBase.Value.AnimSpeedParamSO,
                enemyBase.Value.CurrentBossState == BossState.Step2
                    ? animInfoSO.AnimSpeed + 0.1f
                    : animInfoSO.AnimSpeed);
            enemyBase.Value.AnimatorCompo.CrossFadeInFixedTime(animInfoSO.AnimName, blend);
            if (atkInfoSO != null)
                enemyBase.Value.SetCombatData(atkInfoSO);
        }

        public override TaskStatus OnUpdate()
        {
            if (!enemyBase.Value.AnimationEnd(animInfoSO.AnimName, EndAnimTime))
            {
                return TaskStatus.Running;
            }

            return TaskStatus.Success;
        }

        public override void OnEnd()
        {
            enemyBase.Value.SetParam(enemyBase.Value.AnimSpeedParamSO, 1.0f);
            // enemyBase.Value.RotateSpeed = enemyBase.Value.GetStat().rotateSpeed;
            // enemyBase.Value.LegAnimatorCompo.User_FadeEnabled(0);
            // enemyBase.Value.AnimatorCompo.SetFloat("AnimSpeed", 1);
        }
    }
}