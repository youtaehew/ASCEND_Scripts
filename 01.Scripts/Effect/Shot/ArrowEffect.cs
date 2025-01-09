using PJH.Manager;
using PJH.Scene;
using UnityEngine;

namespace YTH.Effect
{
    public class ArrowEffect : DelayActive
    {
        private Transform _playerTrm;
        private float _rotateSpeed = 7f;

        protected override void Start()
        {
            base.Start();
            _playerTrm = (Managers.Scene.CurrentScene as GameScene).Player.transform;
        }


        private void FixedUpdate()
        {
            if (_playerTrm == null || !_canRotate) return;
            Vector3 dir = _playerTrm.position - transform.position;
            dir.y = 0;
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * _rotateSpeed);
        }

        public override void ResetItem()
        {
            base.ResetItem();
            m_activeObj[0].SetActive(false);
            m_activeObj[1].SetActive(false);
        }
    }
}

