using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

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
    private TMPro.TextMeshProUGUI m_menuScore = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_timeBonus = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_gameOver = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_coins = null;
    [SerializeField]
    private GameObject m_menuUI = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_win = null;
    [SerializeField]
    private List<GameObject> m_bubblePrefabs = new List<GameObject>();
    [SerializeField]
    private float m_minBubble = 0.1f;
    [SerializeField]
    private float m_maxBubble = 1.0f;
    [SerializeField]
    private float m_bubbleTime = 0.0f;

    private FlockManager m_flockManager = null;
    private Player m_player = null;
    private Canvas m_canvas = null;
    private bool m_showMenuUI = false;

    public bool IsMenuOpen()
    {
        return m_showMenuUI;
    }

    private void Start()
    {
        m_player = GameManager.Instance.Player;
        m_flockManager = GameManager.Instance.FlockManager;
        m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if(!GameManager.Instance.IsPaused)
        {
            UpdateTarget();
        }

        if(!GameManager.Instance.IsGameOver() && !GameManager.Instance.IsGameWon())
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                m_showMenuUI = !m_showMenuUI;
            }
        }

        m_menuUI.SetActive(m_showMenuUI);
        UpdateTextUI();
        UpdateBubbles();
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
        m_score.text = $"Score {GameManager.Instance.Score}";
        m_menuScore.text = $"Score {GameManager.Instance.Score}";
        m_coins.text = $"Coins {GameManager.Instance.RunResources.SlimeTokens}";
        m_timeBonus.text = $"Time Bonus {GameManager.Instance.GetTimeBonus()}";

        float timeRemaining = GameManager.Instance.GetTimeRemaining();

        if(timeRemaining > 0.0f)
        {
            int minRemaining = Mathf.FloorToInt(timeRemaining / 60.0f);
            int secondsRemaining = Mathf.FloorToInt(timeRemaining - (minRemaining * 60.0f));
            m_time.text = $"Remaining {minRemaining}:{secondsRemaining:00}";
        }
        else 
        {
            m_time.text = "BOSS";
        }


        m_gameOver.gameObject.SetActive(GameManager.Instance.IsGameOver());
        m_win.gameObject.SetActive(GameManager.Instance.IsGameWon());
        m_showMenuUI |= GameManager.Instance.IsGameOver() || GameManager.Instance.IsGameWon();
    }

    private void UpdateBubbles()
    {
        //m_bubbleTime -= Time.deltaTime;
        //if (m_bubbleTime < 0.0f)
        //{
        //    GameObject bubble = Instantiate(m_bubblePrefabs[Random.Range(0, m_bubblePrefabs.Count)], m_canvas.transform);
        //    bubble.transform.localPosition = new Vector2(Random.Range(m_canvas.pixelRect.xMin, m_canvas.pixelRect.xMax), m_canvas.pixelRect.yMin) - new Vector2(m_canvas.pixelRect.xMax * 0.5f, m_canvas.pixelRect.yMax * 0.5f);

        //    m_bubbleTime = Random.Range(m_minBubble, m_maxBubble);
        //}
    }
}
