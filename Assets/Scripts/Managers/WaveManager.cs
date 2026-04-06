using System;
using System.Collections;
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
        [SerializeField] private int waveSize = 0;
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
                yield return new WaitForSeconds(spawnDelta);
                _aliveEnemies++;
                enemy.GetComponent<Enemy>().OnDeath += HandleEnemyDeath;
            }
            waveSize = (int) (waveSize * waveSizeMultiplier);
        }
        
        private void HandleEnemyDeath()
        {
            _aliveEnemies--;

            if (_aliveEnemies <= 0)
            {
                WaveCompleted();
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