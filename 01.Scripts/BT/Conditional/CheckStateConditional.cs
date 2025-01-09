using YTH.Boss;

namespace YTH.BT.Conditional
{
    using BehaviorDesigner.Runtime.Tasks;

    public class CheckStateConditional : Conditional
    {
        public SharedBossEnemy enemyBase;
        private BossState currentState;

        public override void OnStart()
        {
            currentState = enemyBase.Value.CurrentBossState;
        }

        public override TaskStatus OnUpdate()
        {
            if (currentState == BossState.Step1) return TaskStatus.Failure;
            else if (currentState == BossState.Step2) return TaskStatus.Success;
            else return TaskStatus.Running;
        }
    }
}