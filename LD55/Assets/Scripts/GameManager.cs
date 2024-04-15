using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float m_timeLimit = 300.0f;

    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }
    public FlockManager FlockManager { get; private set; }
    public IRunResources RunResources { get; private set; }
    public WorldSpawnManager WorldSpawnManager { get; private set; }
    public int Score { get; private set; } = 0;

    public float GameTime { get; private set; } = 0.0f;
    public float GameDeltaTime { get; private set; } = 0.0f;
    public bool IsPaused { get; private set; } = false;

    private bool m_bossKilled = false;
    public void OnBossKilled()
    {
        m_bossKilled = true;
    }

    void Awake()
    {
        Player = GetComponentInChildren<Player>();
        FlockManager = GetComponent<FlockManager>();
        WorldSpawnManager = GetComponent<WorldSpawnManager>();
        RunResources = GetComponentInParent<IRunResources>();

        Instance = this;
    }

    private void Update()
    {
        if (!IsPaused)
        {
            GameTime += Time.deltaTime;
            GameDeltaTime = Time.deltaTime;
        }
        else
        {
            GameDeltaTime = 0f;
        }

        if (IsGameOver())
        {
            IsPaused = true;
            return;
        }
    }

    public void SetPaused(bool _paused)
    {
        IsPaused = _paused;
    }

    public float GetTimeRemaining()
    {
        return Mathf.Clamp(m_timeLimit - GameTime, 0.0f, m_timeLimit);
    }

    public bool IsGameOver()
    {
        return Player == null;
    }
    public bool IsGameWon()
    {
        return m_bossKilled;
    }

    public void AddScore(int _score)
    {
        Score += _score;
    }
}
