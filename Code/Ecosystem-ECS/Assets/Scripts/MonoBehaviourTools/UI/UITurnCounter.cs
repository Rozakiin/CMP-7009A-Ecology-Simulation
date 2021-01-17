using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.UI
{
    public class UITurnCounter : MonoBehaviour
    {
        public Text turnDisplay;
        public float counter = 0f;

        private void Start()
        {
            turnDisplay.text = counter.ToString();
        }

        private void Update()
        {
            counter = Time.timeSinceLevelLoad;
            turnDisplay.text = string.Format("Turn {0}:", Mathf.RoundToInt(counter).ToString());
        }
    }
}
