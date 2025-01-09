using UnityEngine;

namespace YTH.SO
{
    public enum ParamType
    {
        Boolean,
        Float,
        Trigger,
        Integer
    }

    [CreateAssetMenu(menuName = "SO/Enemy/Animation/Param")]
    public class AnimParamSO : ScriptableObject
    {


        [Header("ParamSetting")]
        public ParamType paramType;
        public string paramName;
        public int hashValue;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(paramName) == false)
            {
                hashValue = Animator.StringToHash(paramName);
            }
        }
    }

}
