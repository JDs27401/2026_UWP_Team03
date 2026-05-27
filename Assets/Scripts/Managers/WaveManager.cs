using System;
using System.Collections;
using System.Collections.Generic;
using Economy;
using FactoryPattern;
using Singeltons;
using UnityEngine;

namespace Managers
{
    public class WaveManager : SingletonPersistant<WaveManager>
    {
        public event Action OnWaveCompleted;
        
        private float _aliveEnemies;
        
        [Header("Wave Settings")]
        [SerializeField] private List<GameObject> spawnPoints = new();
        [SerializeField] private int waveSize;
        [SerializeField] private float waveSizeMultiplier = 1.5f;
        [SerializeField] private float spawnDelta = 1.0f;

        [Header("Enemy Factory Prefab List")]
        [SerializeField] private BaseEnemyFactory _enemyFactory;

        public IEnumerator StartWave()
        {
            _aliveEnemies = 0;
            for (int i = 0; i < waveSize; i++)
            {
                Enemy enemyComponent = _enemyFactory.CreateEnemy(transform);
                if (enemyComponent != null)
                {
                    Transform spawnTransform = GetRandomSpawnTransform();
                    enemyComponent.transform.position = spawnTransform != null ? spawnTransform.position : transform.position;
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
            print("all enemies killed");
            WaveCompleted();
        }

        private Transform GetRandomSpawnTransform()
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
            {
                return null;
            }

            int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            GameObject selectedSpawnPoint = spawnPoints[randomIndex];
            return selectedSpawnPoint != null ? selectedSpawnPoint.transform : null;
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
        public void SetWaveSize(int waveSize){
            this.waveSize = waveSize;
        }
        public void SetEnemyFactory(EnemyFactory enemyFactory){
            this._enemyFactory = enemyFactory;
        }
    }
    
}