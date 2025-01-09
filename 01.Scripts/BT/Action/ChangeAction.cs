using YTH.Boss;

namespace YTH.BT.Actions
{
    public class ChangeAction: BaseAction
    {
        public override void OnStart()
        {
            base.OnStart();
            enemyBase.Value.ChangeAction?.Invoke();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            enemyBase.Value.CurrentBossState = BossState.Step2;
            //change visual
        }
    }
}

