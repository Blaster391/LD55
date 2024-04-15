using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // The game is built up of fixes stages that get harder over time, by way or more difficult enemies
    // and faster spawning
    [System.Serializable]
    private struct StageData
    {
        // Each stage will have waves of easier mobs to keep the player occupied
        // These enemies can be spawned
        public List<Enemy> MobEnemies;

        // This is how many mobs could be spawned in a single wave
        public int MobWaveCountMax;

        // This is a fixed delay between waves regardless of how many enemies are alive. To stop them all spawning instantly.
        public float MobWaveMinDuration;

        // The number at which we will spawn the next wave to ensure there's always something coming at ya
        public int MobTotalCountMin;

        // The total number of mobs we allow to exist in this wave
        public int MobTotalCountMax;


        // And harder 'boss' like enemies that spawn less frequently but require some focus fire and attention
        public Enemy BossEnemy;
        public float BossSpawnTimer;


        // The number of kills required to move to the next stage
        public int KillCountRequired;
    }
    [Header("Spawning Rules")]
    [SerializeField] 
    private List<StageData> m_StageData;
    [SerializeField]
    private Enemy m_bossPrefab = null;

    [Header("Location")]
    [SerializeField] private float m_SpawnDistanceFromPlayer;

    public event Action<int> StageIncreased;
    private int m_CurrentStage = 0;

    private float m_DurationSinceMobWaveSpawned = 0;
    private float m_DurationSinceBossSpawned = 0;

    private List<Enemy> m_SpawnedMobEnemies = new List<Enemy>();
    private List<Enemy> m_SpawnedBossEnemies = new List<Enemy>();

    private int m_EnemiesKilledThisStage = 0;
    private bool m_bossSpawned = false;

    private void Awake()
    {
        // Make sure we spawn the first wave straight away
        m_DurationSinceMobWaveSpawned = m_StageData[m_CurrentStage].MobWaveMinDuration;
    }

    void Update()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager.IsPaused)
            return;

        if(!m_bossSpawned && (gameManager.GetTimeRemaining() <= 0.0f))
        {
            SpawnEnemy(m_bossPrefab);
            m_bossSpawned = true;
        }

        StageData currentStage = m_StageData[m_CurrentStage];

        m_DurationSinceMobWaveSpawned += gameManager.GameDeltaTime;
        m_DurationSinceBossSpawned += gameManager.GameDeltaTime;

        // We must wait a fixed time before spawning the next wave
        if(m_DurationSinceMobWaveSpawned >= currentStage.MobWaveMinDuration)
        {
            // Then after that we'll spawn it whenever the player has killed enough other things
            if (m_SpawnedMobEnemies.Count <= currentStage.MobTotalCountMin)
            {
                m_DurationSinceMobWaveSpawned = 0;

                SpawnMobWave(currentStage);
            }
        }

        // Bosses just spawn periodically atm
        if (m_DurationSinceBossSpawned >= currentStage.BossSpawnTimer)
        {
            m_DurationSinceBossSpawned = 0;

            SpawnBoss(currentStage);
        }
    }

    private void SpawnMobWave(StageData forStage)
    {
        // Work out how many to spawn
        int currentMobCount = m_SpawnedMobEnemies.Count;
        int currentMobLimit = forStage.MobTotalCountMax;

        int mobCountToSpawn = Mathf.Min(forStage.MobWaveCountMax, currentMobLimit - currentMobCount);

        for(int i = 0; i < mobCountToSpawn; ++i)
        {
            SpawnRandomMob(forStage.MobEnemies);
        }
    }

    private void SpawnBoss(StageData forStage)
    {
        Enemy boss = SpawnEnemy(forStage.BossEnemy);
        m_SpawnedBossEnemies.Add(boss);
        boss.Died += OnBossDied;
    }

    private void OnBossDied(Enemy boss)
    {
        m_SpawnedBossEnemies.Remove(boss);
        boss.Died -= OnMobDied;

        IncrementKillCount();
    }

    private void SpawnRandomMob(List<Enemy> possibleEnemies)
    {
        if (possibleEnemies.Count == 0)
            return;

        Enemy mob = SpawnEnemy(possibleEnemies[UnityEngine.Random.Range(0, possibleEnemies.Count)]);
        m_SpawnedMobEnemies.Add(mob);
        mob.Died += OnMobDied;
    }

    private void OnMobDied(Enemy mob)
    {
        m_SpawnedMobEnemies.Remove(mob);
        mob.Died -= OnBossDied;

        IncrementKillCount();
    }

    private Enemy SpawnEnemy(Enemy enemyPrefab)
    {
        Vector2 direction = UnityEngine.Random.insideUnitCircle.normalized;

        float screenWidth = Camera.main.orthographicSize * 2.0f;
        return Instantiate(enemyPrefab, GameManager.Instance.Player.transform.position + (Vector3)(direction * screenWidth), Quaternion.identity, GameManager.Instance.transform);
    }

    private void IncrementKillCount()
    {
        m_EnemiesKilledThisStage++;

        if (m_EnemiesKilledThisStage > m_StageData[m_CurrentStage].KillCountRequired)
        {
            if (m_StageData.Count > m_CurrentStage - 1)
            {
                m_CurrentStage++;
                m_EnemiesKilledThisStage = 0;

                StageIncreased?.Invoke(m_CurrentStage);
            }
        }
    }
}
