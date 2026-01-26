using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using NUnit.Framework;
using System.Linq;
using Unity.Cinemachine;

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
    private Wave m_wave = new Wave();
    public List<Wave> mWaves = new List<Wave>();

    [Header("Chara ref")]
    [SerializeField] private GameObject m_chara;

    private S_CharaController m_charaScript;
    private S_TriggerCam m_trigger;
    private int mCurrentWave = 0;
    private int ennemyByWave = 0;
    private bool mIsSpawning = false;
    private bool hasSpawnedAll = false;
    public bool hasClearedAllWaves = false;
    private bool hasClearedThisWave = false;
    private bool canSpawnNextWave = true;

    [SerializeField] private CinemachineImpulseSource impulseSource;

    private List<GameObject> mAliveEnemies = new List<GameObject>();

    private List<GameObject> dead = new List<GameObject>();

    private List<Wave> mWaveOrigin = new List<Wave>();

    public bool isDead = false;

    private float destroyTimer = 3f;

    void Start()
    {
        m_charaScript = m_chara.GetComponent<S_CharaController>();
        m_trigger = gameObject.GetComponentInParent<S_TriggerCam>();
        mWaveOrigin = mWaves;
        Debug.Log(m_trigger);
        hasClearedAllWaves = false;
    }

    private void Update()
    {
        if (m_trigger.hasEnteredTriggerCam && !hasClearedAllWaves)
        {
            if (!mIsSpawning && mAliveEnemies.Count == 0 && mCurrentWave < mWaves.Count && canSpawnNextWave && this.enabled == true)
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
                Debug.Log("In");
                hasClearedAllWaves = true;
                Debug.Log(hasClearedAllWaves);
                m_trigger.hasEnteredTriggerCam = false;
                Debug.Log(m_trigger.hasEnteredTriggerCam);

                destroyTimer -= Time.deltaTime;
                if (destroyTimer < 0)
                    Destroy(this.gameObject);
                //StartCoroutine(C_ResetWave());
            }            
        }
    }

    private IEnumerator C_ResetWave()
    {
        yield return new WaitForSeconds(1.5f);
        mCurrentWave = 0;
        mWaves = mWaveOrigin;
        //hasClearedAllWaves = false;
        Debug.Log("fin");
        Debug.Log(hasClearedAllWaves);
    }
  
    
    private IEnumerator C_CheckWave()
    {
        hasClearedThisWave = true;
        hasSpawnedAll = false;

        if (hasClearedThisWave)
        {
            //Debug.Log("devrait changer de Wave");

            if(impulseSource != null)
                impulseSource.GenerateImpulse();


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
            float spawnRate = (i < wave.mSpawnRates.Count) ? wave.mSpawnRates[i] : 0;

            Transform spawnPoint = mSpawnPoints[Random.Range(0, mSpawnPoints.Count)];
            GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            mAliveEnemies.Add(enemy);
            Debug.Log(mWaves.Count());
            Debug.Log(mCurrentWave);
            Debug.Log(mAliveEnemies.Count);
            
            StartCoroutine(C_TrackEnemyDeath(enemy));

            yield return new WaitForSeconds(1);
        }
        ennemyByWave = wave.mEnemies.Count;
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

        //ennemyByWave = mAliveEnemies.Count;

        mAliveEnemies.Remove(trackedEnemy);
        dead.Add(trackedEnemy);
        Debug.Log(dead.Count);
        Debug.Log(mAliveEnemies);
    }
}
