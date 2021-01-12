using UnityEngine;
using UnityEngine.UI;

public class TimeControlSystem : MonoBehaviour
{
    public Text speedDisplay;
    public float fastforwardSpeed;
    public bool pause;
    public Button PlayButton;
    public Button PauseButton;
    public Button FastButton;
    public Button SlowButton;


    void Start()
    {
        speedDisplay.text = fastforwardSpeed.ToString();
        PlayButton.onClick.AddListener(Play);
        PauseButton.onClick.AddListener(Pause);
        FastButton.onClick.AddListener(IncreaseSpeed);
        SlowButton.onClick.AddListener(DecreaseSpeed);
        //start playing at start
        pause = false;
    }

    void Update()
    {
        Time.timeScale = pause ? 0f : fastforwardSpeed;

        if (fastforwardSpeed < 0 || fastforwardSpeed > 100)
        {
            fastforwardSpeed = 0f;
        }

        speedDisplay.text = string.Format("speed {0}:", fastforwardSpeed.ToString());
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
        if (fastforwardSpeed < 1f)
        {
            fastforwardSpeed += 0.2f;
        }
        else
        {
            fastforwardSpeed += 1f;
        }
    }

    public void DecreaseSpeed()
    {
        if (fastforwardSpeed <= 1f && fastforwardSpeed > 0.2f)
        {
            fastforwardSpeed -= 0.2f;
        }
        else if (fastforwardSpeed > 1f)
        {
            fastforwardSpeed -= 1f;
        }
    }

    public float GetfastforwardSpeed()
    {
        return fastforwardSpeed;
    }

}