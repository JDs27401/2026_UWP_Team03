using System;
using System.Collections;
using FactoryPattern;
using Singeltons;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : SingletonPersistant<GameManager>
    {
        [SerializeField] private float timeUntilFirstWave = 60f;
        [SerializeField] private int numberOfWaves = 1;
        [SerializeField] private int startCoins = 150;
        [SerializeField] private int startBaseHealth = 100;
        [SerializeField] private int startWaveNumber = 1;

        private int _completedWaves = 0;
        private bool _gameEnded = false;
        private bool _endlessAfterWin = false;
        private bool _lastEndWasWin = false;
        private Coroutine _beginRunCoroutine;
        private int _beginRunVersion = 0;

        private WaveManager _subscribedWaveManager;

        public GameModel GameModel { get; private set; }

        // Events for game end states
        public event System.Action OnGameWon;
        public event System.Action OnGameLost;

        protected override void Awake() 
        {
            base.Awake();
            if (this == null) return;
            //print(gameObject.GetEntityId());
            //print(gameObject.GetInstanceID());
            //print(gameObject.IsDestroyed());
            CreateFreshModel();
            StartGame();
        }

        public void StartGame()
        {
            if (_beginRunCoroutine != null)
            {
                StopCoroutine(_beginRunCoroutine);
            }
            _beginRunCoroutine = StartCoroutine(BeginRunForCurrentScene());
        }

        private IEnumerator BeginRunForCurrentScene()
        {
            
            UnsubscribeFromWaveManager();

            yield return new WaitForSeconds(timeUntilFirstWave);
            
            while (WaveManager.Instance == null)
            {
                _subscribedWaveManager = gameObject.AddComponent<WaveManager>();
                yield return null;
            }
            //print("Wave Manager started");
            _subscribedWaveManager = WaveManager.Instance;
            _subscribedWaveManager.SetWaveSize(3);
            _subscribedWaveManager.SetEnemyFactory(GameObject.Find("EnemyFactory").GetComponent<EnemyFactory>());
            _subscribedWaveManager.OnWaveCompleted += HandleWaveCompleted;
            //print(_subscribedWaveManager!= null);
            //print(_subscribedWaveManager);

            if (!_gameEnded)
            {
                //print("Game not ended, starting first wave in " + timeUntilFirstWave);
                //print("Time.timeScale = "  + Time.timeScale);
                //print(!_gameEnded + " && " + (_subscribedWaveManager != null));
                if (!_gameEnded && _subscribedWaveManager != null)
                {
                    StartCoroutine(_subscribedWaveManager.StartWave());
                }
            }
        }

        private void HandleWaveCompleted()
        {
            print("GM handling wave completed");
            if (_gameEnded) return;
            print("game not ended");
            _completedWaves++;
            if (!_endlessAfterWin && _completedWaves >= numberOfWaves)
            {
                print("Game ending");
                _gameEnded = true;
                _lastEndWasWin = true;
                OnGameWon?.Invoke();
                return;
            }
            print("game still not ended");
            if (_subscribedWaveManager != null)
            {
                print("wave manager not null, starting next wave");
                StartCoroutine(_subscribedWaveManager.StartWave());
            }
        }
        
        public void LoseGame()
        {
            if (_gameEnded) return;
            
            _gameEnded = true;
            _lastEndWasWin = false;
            OnGameLost?.Invoke();
        }

        public void ContinueAfterWin()
        {
            if (!_gameEnded || !_lastEndWasWin)
            {
                return;
            }

            _gameEnded = false;
            _endlessAfterWin = true;

            if (_subscribedWaveManager != null)
            {
                StartCoroutine(_subscribedWaveManager.StartWave());
            }
        }

        public void PrepareForRestart()
        {
            _completedWaves = 0;
            _gameEnded = false;
            _endlessAfterWin = false;
            _lastEndWasWin = false;
            CreateFreshModel();
        }

        private void CreateFreshModel()
        {
            GameModel = new GameModel(startCoins, startBaseHealth, startWaveNumber);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void UnsubscribeFromWaveManager()
        {
            if (_subscribedWaveManager != null)
            {
                _subscribedWaveManager.OnWaveCompleted -= HandleWaveCompleted;
                _subscribedWaveManager = null;
            }
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromWaveManager();
        }
    }
}