using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.UI
{
    public class UITimeControl : MonoBehaviour
    {
        public static UITimeControl instance;
        private bool pause;
        private float maxSpeed;
        private float minSpeed;
        public float fastForwardSpeed;
        
        [SerializeField] private Text speedDisplay;
        [SerializeField] private Button playButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button fastButton;
        [SerializeField] private Button slowButton;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Start()
        {
            fastForwardSpeed = 1f;
            speedDisplay.text = fastForwardSpeed.ToString();
            playButton.onClick.AddListener(Play);
            pauseButton.onClick.AddListener(Pause);
            fastButton.onClick.AddListener(IncreaseSpeed);
            slowButton.onClick.AddListener(DecreaseSpeed);
            //start playing at start
            pause = false;
            maxSpeed = 20f;
            minSpeed = 0.2f;
        }

        private void Update()
        {
            UpdateFastForwardSpeed();
            speedDisplay.text = string.Format("speed {0}:", fastForwardSpeed.ToString());
        }

        public void UpdateFastForwardSpeed()
        {
            if (fastForwardSpeed < minSpeed)
            {
                fastForwardSpeed = minSpeed;
            }
            else if (fastForwardSpeed > maxSpeed)
            {
                fastForwardSpeed = maxSpeed;
            }
            Time.timeScale = pause ? 0f : fastForwardSpeed;
        }

        public void Play()
        {
            pause = false;
        }

        public void Pause()
        {
            pause = true;
        }

        public void IncreaseSpeed()
        {
            // if speed less than 1f, will increase 0.2f every time
            if (fastForwardSpeed < 1f)
            {
                fastForwardSpeed += 0.2f;
            }
            else
            {
                fastForwardSpeed += 1f;
            }
        }

        public void DecreaseSpeed()
        {
            // if speed less than 1f, will decrease 0.2f every time
            if (fastForwardSpeed <= 1f && fastForwardSpeed > minSpeed)
            {
                fastForwardSpeed -= 0.2f;
            }
            else if (fastForwardSpeed > 1f)
            {
                fastForwardSpeed -= 1f;
            }
        }

        public bool GetPause()
        {
            return pause;
        }

    }
}