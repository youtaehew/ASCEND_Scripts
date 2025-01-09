using Cysharp.Threading.Tasks;
using UnityEngine;
using YTH.Boss;

namespace YTH.Effect
{
    public class BeamLazerEffect : MonoBehaviour, IPoolable
    {
        [Header("Prefabs")] public GameObject beamLineRendererPrefab; // Put a prefab with a line renderer here.
        public GameObject beamStartPrefab; // Prefab at the start of the beam.
        public GameObject beamEndPrefab; // Prefab at the end of the beam.

        private GameObject _beamStart;
        private GameObject _beamEnd;
        private GameObject _beam;
        private LineRenderer _line;

        [Header("Beam Options")] public float beamLength = 100f; // Maximum beam length.
        public float textureScrollSpeed = 0f; // Texture scrolling speed.
        public float textureLengthScale = 1f; // Texture length scale.

        [Header("Pulse Options")] public float widthMultiplier = 1.5f;
        private float _customWidth;
        private float _originalWidth;
        private float _lerpValue = 0.0f;
        public float PulseSpeed = 1.0f;
        private bool pulseExpanding = true;

        [Header("Beam Control")] public Transform beamOrigin; // Position where the beam starts.
        public Transform target; // Direction or position to point the beam.
        public float beamDuration = 1f; // Time for which the beam is active.

        private bool _isShooting = false;
        private float _currentTime = 0;
        private Vector3 start;
        private Vector3 end;
        private Vector3 dir;

        public BossEnemy Boss;


        void Start()
        {
            _originalWidth = 0.2f; // Default width for the beam.
            _customWidth = _originalWidth * widthMultiplier;
        }

        void FixedUpdate()
        {
            if (_beam && _isShooting)
            {
                float distance = 40;
                _line.SetPosition(0, start);
                _line.SetPosition(1, end);
                _beamStart.transform.position = start;
                _beamStart.transform.LookAt(end);
                _beamEnd.transform.position = end;
                _beamEnd.transform.LookAt(start);
                _line.material.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
                _line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);

                // Pulse the beam width.
                PulseBeamWidth();

                _currentTime += Time.deltaTime;
                if (_currentTime >= beamDuration)
                {
                    _isShooting = false;
                    _pool.Push(this);
                    _currentTime = 0;
                    TurnOffBeam();
                }
            }
        }

        private void PulseBeamWidth()
        {
            if (pulseExpanding)
                _lerpValue += Time.deltaTime * PulseSpeed;
            else
                _lerpValue -= Time.deltaTime * PulseSpeed;

            if (_lerpValue >= 1.0f)
            {
                pulseExpanding = false;
                _lerpValue = 1.0f;
            }
            else if (_lerpValue <= 0.0f)
            {
                pulseExpanding = true;
                _lerpValue = 0.0f;
            }

            float currentWidth = Mathf.Lerp(_originalWidth, _customWidth, Mathf.Sin(_lerpValue * Mathf.PI));
            _line.startWidth = currentWidth;
            _line.endWidth = currentWidth;
        }

        public async void TurnOnBeam()
        {
            start = beamOrigin.position;
            end = target ? target.position : beamOrigin.forward * beamLength;
            end.y += 1;
            dir = (end - start).normalized;
            if (!_beam)
            {
                SpawnBeam();
            }

            _isShooting = true;
            await UniTask.WaitForFixedUpdate(); 
            _beam.SetActive(true);
        }

        public void TurnOffBeam()
        {
            if (_beam)
            {
                _isShooting = false;
                _beam.SetActive(false);
            }
        }

        public void FireBeam()
        {
            TurnOnBeam();

            // Automatically turn off the beam after the specified duration.
            Invoke(nameof(TurnOffBeam), beamDuration);
        }

        private void SpawnBeam()
        {
            if (beamLineRendererPrefab)
            {
                // Spawn beam objects.
                _beam = Instantiate(beamLineRendererPrefab);
                _beam.transform.parent = transform;
                _line = _beam.GetComponent<LineRenderer>();
                _line.useWorldSpace = true;
                _line.positionCount = 2;

                if (beamStartPrefab)
                    _beamStart = Instantiate(beamStartPrefab, _beam.transform);
                if (beamEndPrefab)
                    _beamEnd = Instantiate(beamEndPrefab, _beam.transform);
            }
            else
            {
                Debug.LogError("BeamLineRendererPrefab is not assigned in the PolygonBeamStatic script.");
            }
        }


        private Pool _pool;
        [field: SerializeField] public PoolTypeSO PoolType { get; private set; }

        public GameObject GameObject => gameObject;

        public void SetUpPool(Pool pool)
        {
            _pool = pool;

            _pool.Push(this);
        }

        public void ResetItem()
        {
        }
    }
}