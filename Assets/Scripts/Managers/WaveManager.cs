using System;
using System.Collections;
using Economy;
using Singeltons;
using UnityEngine;

namespace Managers
{
    public class WaveManager : SingletonNonPersistant<WaveManager>
    {
        public event Action OnWaveCompleted;
        
        private float _aliveEnemies;
        
        [Header("Wave Settings")]
        [SerializeField] private Vector3 spawnpoint;
        [SerializeField] private int waveSize;
        [SerializeField] private float waveSizeMultiplier = 1.5f;
        [SerializeField] private float spawnDelta = 1.0f;

        [Header("Enemy Prefab List")]
        [SerializeField] private GameObject[] enemies;

        public IEnumerator StartWave()
        {
            for (int i = 0; i < waveSize; i++)
            {
                var random = new System.Random();
                GameObject enemy = Instantiate(enemies[random.Next(enemies.Length)], spawnpoint, Quaternion.identity);
                 
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.OnDeath += HandleEnemyDeath;
                    _aliveEnemies++;
                }
                
                yield return new WaitForSeconds(spawnDelta);
            }

            // Czekaj aż wszystkie spawnięte przeciwniki będą martwi, zanim fala się skończy
            while (_aliveEnemies > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            waveSize = (int) (waveSize * waveSizeMultiplier);
            WaveCompleted();
        }
        
        private void HandleEnemyDeath()
        {
            _aliveEnemies--;
            if (SimpleEconomyService.Instance != null)
            {
                SimpleEconomyService.Instance.AddCredits(1, "zabicie przeciwnika");
            }
        }

        private void WaveCompleted()
        {
            #if UNITY_EDITOR
            print("Wave completed");
            #endif
            OnWaveCompleted?.Invoke();
        }
    }
}