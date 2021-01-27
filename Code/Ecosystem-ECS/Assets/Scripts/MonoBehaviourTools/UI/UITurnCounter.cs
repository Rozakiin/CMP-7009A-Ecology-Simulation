using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.UI
{
    public class UITurnCounter : MonoBehaviour
    {
        [SerializeField] private Text _turnDisplay;

        private void Start()
        {
            _turnDisplay.text = $"Turn: {Mathf.RoundToInt(Time.timeSinceLevelLoad)}";
        }

        private void Update()
        {
            _turnDisplay.text = $"Turn: {Mathf.RoundToInt(Time.timeSinceLevelLoad)}";
        }
    }
}
