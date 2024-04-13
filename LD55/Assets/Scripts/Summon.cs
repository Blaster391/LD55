using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    public enum State
    {
        Follow,
        Attack,
        Recoil
    }

    [SerializeField]
    private float m_movementForce = 1.0f;

    private FlockManager m_flockManager = null;
    private State m_state = State.Follow;
    private Vector2 m_movement = Vector2.zero;
    private Rigidbody2D m_rigidbody = null;

    void Start()
    {
        m_flockManager = GameManager.Instance.FlockManager;
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        m_rigidbody.AddForce(m_movement * m_movementForce);
    }

    void Update()
    {
        m_movement = Vector2.zero;

        switch (m_state)
        {
            case State.Follow:
                UpdateFollow();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Recoil:
                UpdateRecoil();
                break;
        }

        if (m_movement.magnitude > 1.0f)
        {
            m_movement.Normalize();
        }
    }

    private void UpdateFollow()
    {
        Vector2 targetPosition = m_flockManager.TargetPosition;
        m_movement = targetPosition - m_rigidbody.position;
    }

    private void UpdateAttack()
    {

    }

    private void UpdateRecoil()
    {

    }
}
