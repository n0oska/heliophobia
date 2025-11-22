using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using NUnit.Framework;
using System.Linq;

[System.Serializable]
public class Wave
{
    public string mName = "Wave";
    public List<GameObject> mEnemies = new List<GameObject>();
    public List<float> mSpawnRates = new List<float>();
}

public class S_EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public List<Transform> mSpawnPoints;

    [Header("Waves Configuration")]
    public List<Wave> mWaves = new List<Wave>();

    private int mCurrentWave = 0;
    private int ennemyByWave = 0;
    private bool mIsSpawning = false;
    private bool hasSpawnedAll = false;
    public bool hasClearedAllWaves = false;
    private bool hasClearedThisWave = false;
    private bool canSpawnNextWave = true;

    private List<GameObject> mAliveEnemies = new List<GameObject>();

    private List<GameObject> dead = new List<GameObject>();

    public bool isDead = false;

    private void Update()
    {


        if (!mIsSpawning && mAliveEnemies.Count == 0 && mCurrentWave < mWaves.Count && canSpawnNextWave)
        {
            StartCoroutine(SpawnWave(mWaves[mCurrentWave]));
            canSpawnNextWave = false;
        }

        if (hasSpawnedAll && ennemyByWave == dead.Count)
        {
            //Debug.Log("has enter code");
            StartCoroutine(C_CheckWave());            
        }        

        if (mCurrentWave >= mWaves.Count && mAliveEnemies.Count == 0)
        {
            hasClearedAllWaves = true;
        }

        Debug.Log(mAliveEnemies.Count);
        //Debug.Log(mWaves.Count);
        Debug.Log(mCurrentWave);
    }
    
    private IEnumerator C_CheckWave()
    {
        hasClearedThisWave = true;
        hasSpawnedAll = false;

        if (hasClearedThisWave)
        {
            //Debug.Log("devrait changer de Wave");
            mCurrentWave++;
            hasClearedThisWave = false;
            canSpawnNextWave = true;
            yield return new WaitForSeconds(0.3f);            
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        mIsSpawning = true;
        dead.Clear();
        for (int i = 0; i < wave.mEnemies.Count; i++)
        {
            GameObject prefab = wave.mEnemies[i];
            float spawnRate = (i < wave.mSpawnRates.Count) ? wave.mSpawnRates[i] : 0f;

            Transform spawnPoint = mSpawnPoints[Random.Range(0, mSpawnPoints.Count)];
            GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            mAliveEnemies.Add(enemy);
            
            StartCoroutine(C_TrackEnemyDeath(enemy));

            yield return new WaitForSeconds(spawnRate);
        }
        ennemyByWave = mAliveEnemies.Count;
        Debug.Log(ennemyByWave);
        hasSpawnedAll = true;       
        mIsSpawning = false; 
    }

    private IEnumerator C_TrackEnemyDeath(GameObject enemy)
    {

        GameObject trackedEnemy = enemy;
        

        while (trackedEnemy != null)
            yield return null;
        //Debug.Log(trackedEnemy);
        

        mAliveEnemies.Remove(trackedEnemy);
        dead.Add(trackedEnemy);
        Debug.Log(dead.Count);
    }
}
