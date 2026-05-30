using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField] private MainMenuView view;
        private void Start()
        {
            if (view == null)
            {
                Debug.LogError("MainMenuPresenter: MainMenuPresenter is not assigned!");
                return;
            }
            
            if (view.GetStartButton() != null)
                view.GetStartButton().onClick.AddListener(StartGame);
            
            if (view.GetEndButton() != null)
                view.GetEndButton().onClick.AddListener(QuitGame);
        }

        public void StartGame()
        {
            SceneManager.LoadScene("Scena_fajna");
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
    }
}