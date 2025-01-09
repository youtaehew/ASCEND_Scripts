using BehaviorDesigner.Runtime.Tasks;

namespace YTH.BT.Conditional
{
    public class MinDistanceConditional : DistanceConditional
    {
        public override TaskStatus OnUpdate()
        {
            if (enemyBase.Value.DistanceToPlayer < _distance)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }

}
