using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float m_timeLimit = 300.0f;

    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }
    public FlockManager FlockManager { get; private set; }
    public IRunResources RunResources { get; private set; }

    private bool m_isPaused = false;
    private float m_time = 0.0f;

    void Awake()
    {
        Player = GetComponentInChildren<Player>();
        FlockManager = GetComponent<FlockManager>();
        RunResources = GetComponentInParent<IRunResources>();

        Instance = this;
    }

    private void Update()
    {
        if(IsGameOver())
        {
            return;
        }

        m_time += Time.deltaTime;
        float timeRemaining = GetTimeRemaining();

        if(timeRemaining <= 0.0f)
        {
            m_isPaused = true;
        }
    }

    public bool IsPaused()
    {
        return m_isPaused;
    }

    public void SetPaused(bool _paused)
    {
        m_isPaused = _paused;
        Time.timeScale = (m_isPaused) ? 0.0f : 1.0f;
        
    }

    public float GetTimeRemaining()
    {
        return Mathf.Clamp(m_timeLimit - m_time, 0.0f, m_timeLimit);
    }

    public bool IsGameOver()
    {
        return Player == null;
    }
    public bool IsGameWon()
    {
        return GetTimeRemaining() <= 0.0f;
    }
}
