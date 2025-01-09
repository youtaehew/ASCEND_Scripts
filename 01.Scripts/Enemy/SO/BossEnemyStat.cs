using UnityEngine;

namespace YTH.SO
{
    [CreateAssetMenu(menuName = "SO/Enemy/Stats")]
    public class BossEnemyStat : ScriptableObject
    {
        [Header("Base Stat")] public int health;
        public int moveSpeed;
        public int rotateSpeed;

        public int attackRotateSpeed;
        //public int changeStep2;
        
        [Header("Distance")]
        public int closeDistance;
        public int middleDistance;
        public int farDistance;
    }
}