using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlSystem : MonoBehaviour
{
    public Text speedDisplay;
    public float fastForwardMultiplier;
    bool pause;

    void Start()
    {
        speedDisplay.text = fastForwardMultiplier.ToString();
    }
    void Update()
    {
        if (pause)
        {
            Time.timeScale = 0f;
        }
        if (fastForwardMultiplier < 0)
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
        if (fastForwardMultiplier <= 1f && fastForwardMultiplier>0.2f)
        {
            fastForwardMultiplier -= 0.2f;
            Time.timeScale = pause ? 0f : fastForwardMultiplier;
        }
        else if(fastForwardMultiplier > 1f)
        {
            fastForwardMultiplier -= 1f;
            Time.timeScale = pause ? 0f : fastForwardMultiplier;
        }
    }

}
