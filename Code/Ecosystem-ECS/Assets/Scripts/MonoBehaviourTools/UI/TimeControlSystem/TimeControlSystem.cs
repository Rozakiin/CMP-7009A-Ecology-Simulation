using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlSystem : MonoBehaviour
{
    public Text speedDisplay;
    public float fastForwardMultiplier;
    bool pause;
    public Button PlayButton;
    public Button PauseButton;
    public Button SlowButton;
    public Button DownButton;
    void Start()
    {
        speedDisplay.text = fastForwardMultiplier.ToString();
        Button PlayButton1 = PlayButton.GetComponent<Button>();
        PlayButton1.onClick.AddListener(Play);
        Button PauseButton1 = PauseButton.GetComponent<Button>();
        PauseButton1.onClick.AddListener(Pause);
        Button SlowButton1 = SlowButton.GetComponent<Button>();
        SlowButton1.onClick.AddListener(IncreaseSpeed);
        Button DownButton1 = DownButton.GetComponent<Button>();
        DownButton1.onClick.AddListener(DecreaseSpeed);
    }
    void Update()
    {
        if (pause)
        {
            Time.timeScale = 0f;
        }
        if (fastForwardMultiplier < 0 || fastForwardMultiplier>100)
        {
            fastForwardMultiplier = 0f;
        }
        speedDisplay.text = string.Format("speed {0}:", fastForwardMultiplier.ToString());
    }
    public void Play()
    {
        pause = false;
        Time.timeScale = fastForwardMultiplier;
    }
    public void Pause()
    {
        pause = true;
    }
    public void IncreaseSpeed()
    {
        if (fastForwardMultiplier < 1f)
        {
            fastForwardMultiplier += 0.2f;
            Time.timeScale = pause ? 0f : fastForwardMultiplier;
        }
        else
        {
            fastForwardMultiplier += 1f;
            Time.timeScale = pause ? 0f : fastForwardMultiplier;
        }
    }
    public void DecreaseSpeed()
    {
        if (fastForwardMultiplier <= 1f && fastForwardMultiplier > 0.2f)
        {
            fastForwardMultiplier -= 0.2f;
            Time.timeScale = pause ? 0f : fastForwardMultiplier;
        }
        else if (fastForwardMultiplier > 1f)
        {
            fastForwardMultiplier -= 1f;
            Time.timeScale = pause ? 0f : fastForwardMultiplier;
        }
    }

}