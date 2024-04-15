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
    private TMPro.TextMeshProUGUI m_gameOver = null;
    [SerializeField]
    private Button m_ReturnToMenuButton = null;
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

    private void Start()
    {
        m_player = GameManager.Instance.Player;
        m_flockManager = GameManager.Instance.FlockManager;
        m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void Update()
    {
        if(!GameManager.Instance.IsPaused)
        {
            UpdateTarget();
        }

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
        m_ReturnToMenuButton.gameObject.SetActive(GameManager.Instance.IsGameOver() || GameManager.Instance.IsGameWon());
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

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
