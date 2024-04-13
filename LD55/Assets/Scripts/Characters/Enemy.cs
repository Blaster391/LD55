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
    private float m_bounceForce = 20.0f;

    private FlockManager m_flockManager = null;
    private Vector2 m_movement = Vector2.zero;
    private Rigidbody2D m_rigidbody = null;
    private Player m_player = null;
    public void OnDamaged(float _damage)
    {
        m_health -= _damage;
        if (m_health <= 0.0f)
        {
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

        Vector2 target = (m_player.transform.position - transform.position);
        m_movement = target;
        m_movement.Normalize();
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.collider.gameObject.GetComponent<Player>() != null)
        {
            m_rigidbody.AddForce(_collision.GetContact(0).normal * m_bounceForce, ForceMode2D.Impulse);

            Destroy(_collision.collider.gameObject);
        }
    }
}
