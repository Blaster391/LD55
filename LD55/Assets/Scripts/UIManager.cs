using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_targetReticule = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_health = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_time = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_score = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_gameOver = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_win = null;

    private FlockManager m_flockManager = null;
    private Player m_player = null;

    private void Start()
    {
        m_player = GameManager.Instance.Player;
        m_flockManager = GameManager.Instance.FlockManager;
    }

    private void Update()
    {
        if(!GameManager.Instance.IsPaused())
        {
            UpdateTarget();
        }

        UpdateTextUI();
    }

    private void UpdateTarget()
    {
        m_targetReticule.SetActive(!m_flockManager.IsTargettingPlayer);
        m_targetReticule.transform.position = m_flockManager.TargetPosition;
    }

    private void UpdateTextUI()
    {
        int health = m_player != null ? m_player.GetHealth() : 0;
        m_health.text = $"Health {health}";
        m_health.text = $"Score {GameManager.Instance.Score}";

        float timeRemaining = GameManager.Instance.GetTimeRemaining();
        int minRemaining = Mathf.FloorToInt(timeRemaining / 60.0f);
        int secondsRemaining = Mathf.FloorToInt(timeRemaining - (minRemaining * 60.0f));
        m_time.text = $"Remaining {minRemaining}:{secondsRemaining:00}";

        m_gameOver.gameObject.SetActive(GameManager.Instance.IsGameOver());
        m_win.gameObject.SetActive(GameManager.Instance.IsGameWon());
    }
}
