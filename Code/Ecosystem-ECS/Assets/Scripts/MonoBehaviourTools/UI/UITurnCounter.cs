using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.UI
{
    public class UITurnCounter : MonoBehaviour
    {
        [SerializeField] private UIGraph uiGraph;
        [SerializeField] private Text turnDisplay;
        private float counter;

        private void Start()
        {
            counter = 0f;
            turnDisplay.text = counter.ToString();
        }

        private void Update()
        {
            counter = uiGraph.GetGraphListCount();
            turnDisplay.text = string.Format("Turn {0}:", Mathf.RoundToInt(counter).ToString());
        }
    }
}
