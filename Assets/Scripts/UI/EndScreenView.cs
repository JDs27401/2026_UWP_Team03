using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndScreenView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endScreenTitle;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button continueButton; // Only for victory

    private void Start()
    {
        // Ensure buttons are not null
        if (tryAgainButton == null || quitButton == null)
        {
            Debug.LogError("EndScreenView: required buttons are not assigned!");
        }
    }

    public void SetupWinScreen()
    {
        if (endScreenTitle != null)
            endScreenTitle.text = "You won!";
        
        // Show continue button only for victory
        if (continueButton != null)
            continueButton.gameObject.SetActive(true);
        
        gameObject.SetActive(true);
    }

    public void SetupLoseScreen()
    {
        if (endScreenTitle != null)
            endScreenTitle.text = "You lost!";
        
        // Hide continue button for loss
        if (continueButton != null)
            continueButton.gameObject.SetActive(false);
        
        gameObject.SetActive(true);
    }

    public void OnTryAgainClicked()
    {
        // This will be handled by the presenter
    }

    public void OnContinueClicked()
    {
        // This will be handled by the presenter (only for victory)
    }

    public void OnQuitClicked()
    {
        // This will be handled by the presenter
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public Button GetTryAgainButton() => tryAgainButton;
    public Button GetContinueButton() => continueButton;
    public Button GetQuitButton() => quitButton;
}

