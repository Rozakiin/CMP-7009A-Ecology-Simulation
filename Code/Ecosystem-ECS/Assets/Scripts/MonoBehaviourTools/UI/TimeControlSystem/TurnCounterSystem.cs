using UnityEngine;
using UnityEngine.UI;

public class TurnCounterSystem : MonoBehaviour
{
    public Text turnDisplay;
    float counter = 0f;


    void Start()
    {
        turnDisplay.text = counter.ToString();
    }

    void Update()
    {
        counter += Time.deltaTime;
        turnDisplay.text = string.Format("Turn {0}:", Mathf.RoundToInt(counter).ToString());
    }
}
