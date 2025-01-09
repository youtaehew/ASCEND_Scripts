using MoreMountains.Feedbacks;
using UnityEngine;

namespace YTH.Combat
{
    public class BossFeedback : MonoBehaviour
    {
        [SerializeField]
        private MMF_Player _groundFeedback;

        private void GroundAtkFeedback()
        {
            _groundFeedback.PlayFeedbacks();
        }


    }
}

