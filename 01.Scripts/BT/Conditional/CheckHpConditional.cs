using YTH.Boss;

namespace YTH.BT.Conditional
{
    using BehaviorDesigner.Runtime.Tasks;
    public class CheckHpConditional : Conditional
    {
        public SharedBossEnemy enemyBase;
        public float hpPercent;
        
        private int _maxHp;
        private int _currentHp;
        private int _checkHp;
        private bool _once = false;

        
        public override void OnStart()
        {
            _maxHp = enemyBase.Value.GetStat().health;
            _checkHp = (int)(_maxHp * hpPercent);
            _currentHp = enemyBase.Value.HealthCompo.CurrentHealth;
            
        }

        public override TaskStatus OnUpdate()
        {
            if (_currentHp <= _checkHp && !_once)
            {
                _once = true;
                return TaskStatus.Success;  
            }
            return TaskStatus.Failure;
        }
    }
}

