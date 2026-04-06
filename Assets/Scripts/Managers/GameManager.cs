using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Singeltons;
using UnityEngine;

namespace Managers
{
    public class GameManager : SingletonPersistant<GameManager>
    {
        [SerializeField] private float timeUntilFirstWave = 60f;
        [SerializeField] private int numberOfWaves = 1;
        protected IEnumerator Start()
        {
            yield return new WaitForSeconds(timeUntilFirstWave);
            StartCoroutine(WaveManager.Instance.StartWave());
            WaveManager.Instance.OnWaveCompleted += HandleWaveCompleted;
        }

        private void HandleWaveCompleted()
        {
            StartCoroutine(WaveManager.Instance.StartWave());
        }
        
        private void OnDestroy()
        {
            WaveManager.Instance.OnWaveCompleted -= HandleWaveCompleted;
        }
    }
}