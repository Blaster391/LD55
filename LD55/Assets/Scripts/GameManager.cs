using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }
    public FlockManager FlockManager { get; private set; }

    private bool m_isPaused = false;

    void Awake()
    {
        Player = GetComponentInChildren<Player>();
        FlockManager = GetComponent<FlockManager>();

        Instance = this;
    }

    public bool IsPaused()
    {
        return m_isPaused;
    }

    public void SetPaused(bool _paused)
    {
        m_isPaused = _paused;
    }


}
