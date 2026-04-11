using System; // Wymagane dla Action
using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float timeBetweenWaves = 5f;
    
    private float countdown = 2f;
    private int waveIndex = 1;

    public event Action<int> OnWaveStarted;

    void Start()
    {
        OnWaveStarted?.Invoke(waveIndex);
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
    }

    IEnumerator SpawnWave()
    {
        Debug.Log($"Fala {waveIndex} nadchodzi!");
        
        OnWaveStarted?.Invoke(waveIndex);

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        
        waveIndex++; 
    }

    void SpawnEnemy()
    {
        if(enemyPrefab != null && spawnPoint != null)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Brak przypisanego prefabu wroga lub punktu spawnu w WaveSpawner!");
        }
    }
}