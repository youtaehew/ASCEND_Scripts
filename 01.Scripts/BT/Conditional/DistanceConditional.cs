using YTH.Boss;

namespace YTH.BT.Conditional
{
    using BehaviorDesigner.Runtime.Tasks;

    public class DistanceConditional : Conditional
    {
        
        public SharedBossEnemy enemyBase;
        public DistanceType distanceType;
        protected float _distance;

        public override void OnStart()
        {
            _distance = enemyBase.Value.DistanceDist[distanceType];
        }

        public override TaskStatus OnUpdate()
        {
            if (enemyBase.Value.DistanceToPlayer > _distance)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}