using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float m_timeLimit = 300.0f;
    [SerializeField]
    private float m_timeBonus = 1000.0f;
    [SerializeField]
    private float m_timeBonusDecay = 1.0f;

    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }
    public FlockManager FlockManager { get; private set; }
    public IRunResources RunResources { get; private set; }
    public WorldSpawnManager WorldSpawnManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public int Score { get; private set; } = 0;

    public float GameTime { get; private set; } = 0.0f;
    public float GameDeltaTime { get; private set; } = 0.0f;
    private bool m_gameplayPaused = false;
    public bool IsPaused { get { return m_gameplayPaused || UIManager.IsMenuOpen(); } private set { m_gameplayPaused = value; } }

    private bool m_bossKilled = false;
    public void OnBossKilled()
    {
        m_bossKilled = true;
        AddScore(GetTimeBonus());
    }

    public int GetTimeBonus()
    {
        return Mathf.RoundToInt(m_timeBonus);
    }

    void Awake()
    {
        Player = GetComponentInChildren<Player>();
        FlockManager = GetComponent<FlockManager>();
        WorldSpawnManager = GetComponent<WorldSpawnManager>();
        RunResources = GetComponentInParent<IRunResources>();
        UIManager = GetComponent<UIManager>();
        AudioManager = GetComponent<AudioManager>();

        Instance = this;
    }

    private void Update()
    {
        if (!IsPaused)
        {
            GameTime += Time.deltaTime;
            GameDeltaTime = Time.deltaTime;

            m_timeBonus -= m_timeBonusDecay * Time.deltaTime;
            m_timeBonus = Mathf.Max(m_timeBonus, 0.0f);
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
