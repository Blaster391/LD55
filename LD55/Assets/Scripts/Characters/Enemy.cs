using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float m_movementForce = 1.0f;

    [SerializeField]
    private float m_health = 3.0f;

    [SerializeField]
    private float m_maxRecoveryTime = 0.5f;

    [SerializeField]
    private float m_bounceForce = 100.0f;

    [SerializeField]
    private GameObject m_dropType = null;

    [SerializeField]
    private int m_dropMin = 1;

    [SerializeField]
    private int m_dropMax = 3;

    private FlockManager m_flockManager = null;
    private Vector2 m_movement = Vector2.zero;
    private Rigidbody2D m_rigidbody = null;
    private Player m_player = null;
    private float m_recoveryTime = 0.0f;

    public void OnDamaged(float _damage)
    {
        m_health -= _damage;
        if (m_health <= 0.0f)
        {
            int dropCount = Random.Range(m_dropMin, m_dropMax + 1);
            for(int i = 0; i < dropCount; i++)
            {
                GameObject drop = Instantiate(m_dropType, GameManager.Instance.transform);
                drop.transform.position = transform.position;
            }
    
            Destroy(gameObject);
        }
    }

    void Start()
    {
        m_flockManager = GameManager.Instance.FlockManager;
        m_player = GameManager.Instance.Player;
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_flockManager.Register(this);
    }

    private void OnDestroy()
    {
        m_flockManager.Unregister(this);
    }

    private void FixedUpdate()
    {
        if (m_player == null)
        {
            return;
        }

        m_rigidbody.AddForce(m_movement * m_movementForce * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if(m_player == null)
        {
            return;
        }

        if (GameManager.Instance.IsPaused())
        {
            return;
        }

        if(m_recoveryTime > 0.0f) 
        {
            m_recoveryTime -= Time.deltaTime;
            m_movement = Vector2.zero;
            return;
        }


        Vector2 target = (m_player.transform.position - transform.position);
        m_movement = target;
        m_movement.Normalize();
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.collider.gameObject.GetComponent<Player>() != null)
        {
            m_rigidbody.AddForce(_collision.GetContact(0).normal * m_bounceForce, ForceMode2D.Impulse);
            _collision.collider.gameObject.GetComponent<Player>().TakeDamage();
            m_recoveryTime = m_maxRecoveryTime;
        }
    }
}
