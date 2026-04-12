using System;
using System.Collections;
using Singeltons;
using UnityEngine;

namespace Managers
{
    public class GameManager : SingletonPersistant<GameManager>
    {
        [SerializeField] private float timeUntilFirstWave = 60f;
        [SerializeField] private int numberOfWaves = 1;
        private int _completedWaves = 0;

        public GameModel GameModel { get; private set; }

        protected override void Awake() 
        {
            base.Awake();
            
            if (GameModel == null)
            {
                GameModel = new GameModel(150, 100, 1);
            }
        }

        protected IEnumerator Start()
        {
            yield return new WaitForSeconds(timeUntilFirstWave);
            StartCoroutine(WaveManager.Instance.StartWave());
            WaveManager.Instance.OnWaveCompleted += HandleWaveCompleted;
        }

        private void HandleWaveCompleted()
        {
            _completedWaves++;
            if (_completedWaves >= numberOfWaves)
            {
                return;
            }
            StartCoroutine(WaveManager.Instance.StartWave());
        }
        
        private void OnDestroy()
        {
            if (WaveManager.Instance != null)
            {
                WaveManager.Instance.OnWaveCompleted -= HandleWaveCompleted;
            }
        }
    }
}