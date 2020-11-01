using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlSystem : MonoBehaviour
{
    public Text speedDisplay;
    public float fastForwardMultiplier;
    void Start()
    {
        speedDisplay.text = fastForwardMultiplier.ToString();
    }
    void Update()
    {
        if (fastForwardMultiplier < 0)
        {
            fastForwardMultiplier = 0f;
        }
        speedDisplay.text = string.Format("speed {0}:", Mathf.RoundToInt(fastForwardMultiplier).ToString());
    }
    public void Play()
    {
        fastForwardMultiplier = 1f;
        Time.timeScale = fastForwardMultiplier;
    }
    public void Pause()
    {
        fastForwardMultiplier = 0f;
        Time.timeScale = fastForwardMultiplier;
    }
    public void FastForward()
    {
        Time.timeScale = fastForwardMultiplier;
    }
    public void IncreaseSpeed()
    {
        fastForwardMultiplier += 1;
        Time.timeScale = fastForwardMultiplier;
    }
    public void DecreaseSpeed()
    {
        fastForwardMultiplier -= 1;
        Time.timeScale = fastForwardMultiplier;
    }

}
