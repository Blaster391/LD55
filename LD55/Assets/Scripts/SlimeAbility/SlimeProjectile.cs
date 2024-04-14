using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    [SerializeField] private float m_ProjectileDamage;
    [SerializeField] private float m_ProjectileRange;

    private Vector2 m_StartPosition;

    private void Awake()
    {
        m_StartPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy hitEnemy = collision.gameObject.GetComponent<Enemy>();
        if (hitEnemy != null)
        {
            hitEnemy.OnDamaged(m_ProjectileDamage);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, m_StartPosition) >= m_ProjectileRange)
        {
            Destroy(gameObject);
        }
    }
}
