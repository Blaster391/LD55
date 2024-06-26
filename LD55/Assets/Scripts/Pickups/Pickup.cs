using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField]
    private float m_minHorizontalBounce = 0.5f;
    [SerializeField]
    private float m_maxHorizontalBounce = 2.0f;
    [SerializeField]
    private float m_minVerticalBounce = 0.5f;
    [SerializeField]
    private float m_maxVerticalBounce = 3.0f;
    [SerializeField]
    bool m_initialBounce = true;
    [SerializeField]
    private float m_bounceTotalTime = 1.0f;
    [SerializeField]
    private float m_bounceMinTotalTime = 0.5f;
    [SerializeField]
    private float m_bounceMaxTotalTime = 1.0f;
    [SerializeField]
    private float m_suckSpeed = 1.0f;

    [SerializeField]
    private float m_lifespan = 120.0f;

    private bool m_bounceLeft = false;
    private float m_horizontalBounce = 1.0f;
    private float m_verticalBounce = 1.0f;
    private float m_bounceTime = 0.0f;
    private Vector2 m_initialPosition = Vector2.zero;
    private bool m_suck = false;

    void Start()
    {
        m_initialPosition = transform.position;
        m_bounceLeft = Random.value > 0.5f;
        m_horizontalBounce = Random.Range(m_minHorizontalBounce, m_maxHorizontalBounce);
        m_verticalBounce = Random.Range(m_minVerticalBounce, m_maxVerticalBounce);
        m_bounceTotalTime = Random.Range(m_bounceMinTotalTime, m_bounceMaxTotalTime);
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = GameManager.Instance.GameDeltaTime;
        if (m_initialBounce)
        {
            m_bounceTime += deltaTime;

            float alpha = (m_bounceTime / m_bounceMaxTotalTime);

            Vector2 newPosition = m_initialPosition;

            float horizontal = Mathf.Sin(alpha * Mathf.PI * 0.5f) * m_horizontalBounce;
            if(m_bounceLeft)
            {
                horizontal = -horizontal;
            }

            float vertical = Mathf.Sin(alpha * Mathf.PI * 0.9f) * m_verticalBounce;

            newPosition.x += horizontal;
            newPosition.y += vertical;


            transform.position = newPosition;

            if (m_bounceTime > m_bounceTotalTime)
            {
                m_initialBounce = false;
            }
        }

        if(m_suck)
        {
            Player player = GameManager.Instance.Player;
            if(player == null)
            {
                return;
            }

            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();

            transform.position = transform.position + (direction * m_suckSpeed * deltaTime);

        }
        else
        {
            m_lifespan -= deltaTime;

            if(m_lifespan < 0 )
            {
                Destroy(gameObject);
            }
        }

    }

    public void StartSuck()
    {
        m_initialBounce = false;
        m_suck = true;
    }

    public void StopSuck()
    {
        m_suck = false;
    }

    public abstract void OnPickup();

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if(_collider.isTrigger)
        {
            return;
        }

        if(_collider.GetComponent<Player>() != null)
        {
            OnPickup();
            Destroy(gameObject);
        }
    }
}
