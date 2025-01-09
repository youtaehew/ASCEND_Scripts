using PJH.Manager;
using YTH.Boss;


namespace YTH.BT.Actions
{
    using BehaviorDesigner.Runtime.Tasks;

    public class DieAction : Action
    {
        public SharedBossEnemy enemyBase;

        public override void OnStart()
        {
            enemyBase.Value.CurrentBossState = BossState.Die;
            enemyBase.Value.Collider.enabled = false;
            enemyBase.Value.AnimatorCompo.CrossFadeInFixedTime("Die", 0.2f);
            EnemySpawnManager.Instance.DeleteObject(enemyBase.Value);
        }

        public override TaskStatus OnUpdate()
        {
            if (!enemyBase.Value.AnimationEnd("Die"))
            {
                return TaskStatus.Running;
            }

            return TaskStatus.Success;
        }

        public override void OnEnd()
        {
            //die to action
            enemyBase.Value.gameObject.SetActive(false);
            enemyBase.Value.BT.DisableBehavior();
        }
    }
}