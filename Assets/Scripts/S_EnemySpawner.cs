using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

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
    public bool hasClearedAllWaves = false;
    private List<GameObject> mAliveEnemies = new List<GameObject>();

    private GameObject enemy;

    public bool isDead = false;

  
    
    private void Update()
    {
        if (!mIsSpawning && mAliveEnemies.Count == 0)
        {
            StartCoroutine(SpawnWave(mWaves[mCurrentWave]));
        }

        if (mAliveEnemies.Count != 0)
        {
            StartCoroutine(C_TrackEnemyDeath(enemy));//(enemy));
        }       

        if (mAliveEnemies.Count == 0)
        {
            mIsSpawning = false;
            mCurrentWave++;
            Debug.Log(mWaves.Count);
            Debug.Log(mCurrentWave);      
        }        
        

        if (!mIsSpawning)
            Debug.Log(mAliveEnemies.Count);


        if (mCurrentWave > mWaves.Count && mAliveEnemies.Count == 0)
        {
            hasClearedAllWaves = true;
        } 
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        mIsSpawning = true;
        if (mCurrentWave <= mWaves.Count)
        {
            for (int i = 0; i < wave.mEnemies.Count; i++)
            {
                GameObject prefab = wave.mEnemies[i];
                Debug.Log(prefab.name);
                float spawnRate = (i < wave.mSpawnRates.Count) ? wave.mSpawnRates[i] : 0f;

                Transform spawnPoint = mSpawnPoints[Random.Range(0, mSpawnPoints.Count)];
                enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                mAliveEnemies.Add(enemy);

                yield return new WaitForSeconds(spawnRate);
            }
        }        

        
    }

    private IEnumerator C_TrackEnemyDeath(GameObject enemy)//(GameObject enemy)
    {
        var enemyScript = enemy.GetComponent<S_EnemyController>();
        Debug.Log(enemy);

        if (isDead)
        {
            mAliveEnemies.Remove(enemy);
            Debug.Log(mAliveEnemies.Count);
            Debug.Log(mIsSpawning);
        }

        Debug.Log("tracked");
        
        while (enemy != null)
            yield return null;        
        
    }
}
