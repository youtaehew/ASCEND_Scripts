using UnityEngine;

namespace YTH.SO
{
    [CreateAssetMenu(menuName = "SO/Enemy/Animation/Info")]
    public class AnimInfoSO : ScriptableObject
    {
        [Header("AnimationSetting")]
        public string AnimName;
        public float AnimSpeed;
    }
}

