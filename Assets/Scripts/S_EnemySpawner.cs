using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Wave
{
    public string mName = "Wave";
    public List<GameObject> mEnemies = new List<GameObject>();
    public List<float> mSpawnRates = new List<float>(); // spawn rate pour chaque ennemi
}

public class S_EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public List<Transform> mSpawnPoints;

    [Header("Waves Configuration")]
    public List<Wave> mWaves = new List<Wave>();

    private int mCurrentWave = 0;
    private bool mIsSpawning = false;
    private List<GameObject> mAliveEnemies = new List<GameObject>();

    private void Update()
    {
        if (!mIsSpawning && mAliveEnemies.Count == 0 && mCurrentWave < mWaves.Count)
        {
            StartCoroutine(SpawnWave(mWaves[mCurrentWave]));
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        mIsSpawning = true;

        for (int i = 0; i < wave.mEnemies.Count; i++)
        {
            GameObject prefab = wave.mEnemies[i];
            float spawnRate = (i < wave.mSpawnRates.Count) ? wave.mSpawnRates[i] : 0f;

            Transform spawnPoint = mSpawnPoints[Random.Range(0, mSpawnPoints.Count)];
            GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            mAliveEnemies.Add(enemy);
            StartCoroutine(TrackEnemyDeath(enemy));

            yield return new WaitForSeconds(spawnRate);
        }

        mIsSpawning = false;
        mCurrentWave++;
    }

    private IEnumerator TrackEnemyDeath(GameObject enemy)
    {
        while (enemy != null)
            yield return null;

        mAliveEnemies.Remove(enemy);
    }
}
