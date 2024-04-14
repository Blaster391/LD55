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
        public List<GameObject> MobEnemies;
        public float MobWaveTimer;
        public int MobWaveEnemyBaseCount;

        // And harder 'boss' like enemies that spawn less frequently but require some focus fire and attention
        public List<GameObject> BossEnemies;
        public float BossSpawnTimer;
    }

    [SerializeField] private List<StageData> m_StageData;
    [SerializeField] private float m_StageDuration;

    [SerializeField] private float m_MobEnemyCountMultiplierPerSlime = 0.05f;

    [SerializeField] private float m_SpawnDistanceFromPlayer;

    private float m_DurationSinceMobWaveSpawned = 0;
    private float m_DurationSinceBossSpawned = 0;


    void Update()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager.IsPaused)
            return;

        float gameTime = gameManager.GameTime;
        int stageIndex = Mathf.FloorToInt(gameTime / m_StageDuration);
        if (m_StageData.Count <= stageIndex)
            // Finished spawning
            return;

        StageData currentStage = m_StageData[stageIndex];

        m_DurationSinceMobWaveSpawned += gameManager.GameDeltaTime;
        m_DurationSinceBossSpawned += gameManager.GameDeltaTime;

        if(m_DurationSinceMobWaveSpawned >= currentStage.MobWaveTimer)
        {
            m_DurationSinceMobWaveSpawned = 0;

            SpawnMobWave(currentStage);
        }

        if(m_DurationSinceBossSpawned >= currentStage.BossSpawnTimer)
        {
            m_DurationSinceBossSpawned = 0;

            SpawnBoss(currentStage);
        }
    }

    private void SpawnMobWave(StageData forStage)
    {
        int enemiesToSpawn = Mathf.FloorToInt(forStage.MobWaveEnemyBaseCount * (1 + m_MobEnemyCountMultiplierPerSlime * GameManager.Instance.RunResources.SlimeInventory.Count));
        for(int i = 0; i < enemiesToSpawn; ++i)
        {
            SpawnRandomEnemy(forStage.MobEnemies);
        }
    }

    private void SpawnBoss(StageData forStage)
    {
        SpawnRandomEnemy(forStage.BossEnemies);
    }

    private void SpawnRandomEnemy(List<GameObject> possibleEnemies)
    {
        if (possibleEnemies.Count == 0)
            return;

        SpawnEnemy(possibleEnemies[UnityEngine.Random.Range(0, possibleEnemies.Count)]);
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 direction = UnityEngine.Random.insideUnitCircle.normalized;

        GameObject enemy = Instantiate(enemyPrefab, GameManager.Instance.Player.transform.position + (Vector3)(direction * m_SpawnDistanceFromPlayer), Quaternion.identity, GameManager.Instance.transform);
    }
}
