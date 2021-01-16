using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.UI
{
    public class UITimeControl : MonoBehaviour
    {
        private bool pause;
        public float fastForwardSpeed;
        [SerializeField] private Text speedDisplay;
        [SerializeField] private Button playButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button fastButton;
        [SerializeField] private Button slowButton;

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
        }

        private void Update()
        {
            UpdateFastForwardSpeed();
            speedDisplay.text = string.Format("speed {0}:", fastForwardSpeed.ToString());
        }

        public void UpdateFastForwardSpeed()
        {
            if (fastForwardSpeed < 0.2f)
            {
                fastForwardSpeed = 0f;
            }
            else if (fastForwardSpeed > 100f)
            {
                fastForwardSpeed = 100f;
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
            if (fastForwardSpeed <= 1f && fastForwardSpeed > 0.2f)
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