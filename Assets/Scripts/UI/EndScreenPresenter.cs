using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;

public class EndScreenPresenter : MonoBehaviour
{
    [SerializeField] private EndScreenView endScreenView;

    private void Start()
    {
        if (endScreenView == null)
        {
            Debug.LogError("EndScreenPresenter: EndScreenView is not assigned!");
            return;
        }

        endScreenView.Hide();

        // Subscribe to game events
        GameManager.Instance.OnGameWon += ShowWinScreen;
        GameManager.Instance.OnGameLost += ShowLoseScreen;

        // Add button listeners
        if (endScreenView.GetTryAgainButton() != null)
            endScreenView.GetTryAgainButton().onClick.AddListener(ResetGame);

        if (endScreenView.GetContinueButton() != null)
            endScreenView.GetContinueButton().onClick.AddListener(ContinueGame);

        if (endScreenView.GetQuitButton() != null)
            endScreenView.GetQuitButton().onClick.AddListener(QuitGame);
    }

    private void ShowWinScreen()
    {
        Time.timeScale = 0f;
        endScreenView.SetupWinScreen();
    }

    private void ShowLoseScreen()
    {
        Time.timeScale = 0f;
        endScreenView.SetupLoseScreen();
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.PrepareForRestart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.StartGame();
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        endScreenView.Hide();
        GameManager.Instance.ContinueAfterWin();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameWon -= ShowWinScreen;
            GameManager.Instance.OnGameLost -= ShowLoseScreen;
        }

        if (endScreenView != null)
        {
            if (endScreenView.GetTryAgainButton() != null)
                endScreenView.GetTryAgainButton().onClick.RemoveListener(ResetGame);

            if (endScreenView.GetContinueButton() != null)
                endScreenView.GetContinueButton().onClick.RemoveListener(ContinueGame);

            if (endScreenView.GetQuitButton() != null)
                endScreenView.GetQuitButton().onClick.RemoveListener(QuitGame);
        }
    }
}
