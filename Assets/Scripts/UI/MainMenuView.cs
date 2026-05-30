using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI endScreenTitle;
        [SerializeField] private Button startButton;
        [SerializeField] private Button endButton;
        
        private void Start()
        {
            // Ensure buttons are not null
            if (startButton == null || endButton == null)
            {
                Debug.LogError("EndScreenView: required buttons are not assigned!");
                return;
            }
        }
        
        public Button GetStartButton() => startButton;
        public Button GetEndButton() => endButton;
    }
}