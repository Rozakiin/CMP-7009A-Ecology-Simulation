using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.UI
{
    public class UITimeControl : MonoBehaviour
    {
        public static UITimeControl Instance;
        private bool _pause;
        private float _maxSpeed;
        private float _minSpeed;
        public float FastForwardSpeed;

        [SerializeField] private Text _speedDisplay;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _fastButton;
        [SerializeField] private Button _slowButton;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            FastForwardSpeed = 1f;
            _speedDisplay.text = $"Speed: {FastForwardSpeed}";
            _playButton.onClick.AddListener(Play);
            _pauseButton.onClick.AddListener(Pause);
            _fastButton.onClick.AddListener(IncreaseSpeed);
            _slowButton.onClick.AddListener(DecreaseSpeed);
            //start playing at start
            _pause = false;
            _maxSpeed = 20f;
            _minSpeed = 0.2f;
        }

        private void Update()
        {
            UpdateFastForwardSpeed();
            _speedDisplay.text = $"Speed: {FastForwardSpeed}";
        }

        public void UpdateFastForwardSpeed()
        {
            if (FastForwardSpeed < _minSpeed)
            {
                FastForwardSpeed = _minSpeed;
            }
            else if (FastForwardSpeed > _maxSpeed)
            {
                FastForwardSpeed = _maxSpeed;
            }
            Time.timeScale = _pause ? 0f : FastForwardSpeed;
        }

        public void Play()
        {
            _pause = false;
        }

        public void Pause()
        {
            _pause = true;
        }

        public void IncreaseSpeed()
        {
            // if speed less than 1f, will increase 0.2f every time
            if (FastForwardSpeed < 1f)
            {
                FastForwardSpeed += 0.2f;
            }
            else
            {
                FastForwardSpeed += 1f;
            }
        }

        public void DecreaseSpeed()
        {
            // if speed less than 1f, will decrease 0.2f every time
            if (FastForwardSpeed <= 1f && FastForwardSpeed > _minSpeed)
            {
                FastForwardSpeed -= 0.2f;
            }
            else if (FastForwardSpeed > 1f)
            {
                FastForwardSpeed -= 1f;
            }
        }

        public bool GetPause()
        {
            return _pause;
        }

    }
}