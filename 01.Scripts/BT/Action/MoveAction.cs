using UnityEngine.AI;
using YTH.Boss;
using YTH.SO;

namespace YTH.BT.Actions
{
    using BehaviorDesigner.Runtime.Tasks;

    public class MoveAction : Action
    {
        public SharedBossEnemy enemyBase;
        private NavMeshAgent _navMeshAgent;
        public AnimParamSO _animParamSO;

        public override void OnStart()
        {
            #region Navmesh
            _navMeshAgent = enemyBase.Value.NavMeshCompo;
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(enemyBase.Value.PlayerTrm.position);
            #endregion
            enemyBase.Value.LegAnimatorCompo.User_FadeToDisabled(0);
            enemyBase.Value.IsRootMotion = false;
            enemyBase.Value.SetParam(_animParamSO, true);
        }

        public override TaskStatus OnUpdate()
        {
            RemapDestination();
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            #region Navmesh
            _navMeshAgent.isStopped = true;
            _navMeshAgent.SetDestination(transform.position);
            _navMeshAgent.enabled = false;
            #endregion
            
            enemyBase.Value.LegAnimatorCompo.User_FadeEnabled(0);
            enemyBase.Value.IsRootMotion = true;
            enemyBase.Value.SetParam(_animParamSO, false);
        }

        private void RemapDestination()
        {
            if (enemyBase.Value.DistanceToDestination > 0.2f)
            {
                _navMeshAgent.SetDestination(enemyBase.Value.PlayerTrm.position);
            }
        }
    }
    
}