using System.Collections;
using Singeltons;
using UnityEngine;

namespace Managers
{
    public class WaveManager : SingletonNonPersistant
    {
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
                Instantiate(enemies[random.Next(enemies.Length)], spawnpoint, Quaternion.identity);
                yield return new WaitForSeconds(spawnDelta);
            }
            waveSize = (int) (waveSize * waveSizeMultiplier);
        }
    }
}