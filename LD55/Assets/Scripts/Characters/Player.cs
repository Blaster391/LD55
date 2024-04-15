using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_movementSpeed = 1.0f;

    [SerializeField]
    private int m_health = 5;
    public Vector2 MovementInput { get; private set; } = Vector2.zero;

    private Rigidbody2D m_rigidbody = null;
    private float m_playerRadius = 1.0f;

    public int GetHealth()
    {
        return m_health;
    }

    public void AddHealth(int _hp)
    {
        m_health += _hp;
    }

    public void TakeDamage()
    {
        --m_health;
        if(m_health <= 0) 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_playerRadius = GetComponent<CircleCollider2D>().radius / transform.localScale.x;
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = m_rigidbody.position;
        Vector2 movement = (MovementInput * m_movementSpeed * Time.fixedDeltaTime);

        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        m_rigidbody.GetContacts(contacts);

        if(contacts.Count > 0)
        {
            Vector2 resolvedPosition = newPosition;
            foreach (ContactPoint2D contact in contacts)
            {
                if(contact.collider.tag == "Unit")
                {
                    continue;
                }

                ColliderDistance2D contactDistance = Physics2D.Distance(contact.collider, contact.otherCollider);
                Vector2 resolutionDirection = contactDistance.normal * contactDistance.distance;

                resolvedPosition -= resolutionDirection;

                if(movement.x > 0.0f && resolutionDirection.x > 0.0f)
                {
                    movement.x = 0.0f;
                }

                if (movement.x < 0.0f && resolutionDirection.x < 0.0f)
                {
                    movement.x = 0.0f;
                }

                if (movement.y > 0.0f && resolutionDirection.y > 0.0f)
                {
                    movement.y = 0.0f;
                }

                if (movement.y < 0.0f && resolutionDirection.y < 0.0f)
                {
                    movement.y = 0.0f;
                }
            }
            newPosition = resolvedPosition;
        }

        int layerMask = LayerMask.GetMask("Wall");
        float movementMagintude = movement.magnitude;
        if(movementMagintude > 0.0f)
        {
            RaycastHit2D hit = Physics2D.CircleCast(newPosition, m_playerRadius, movement, movementMagintude, layerMask);
            if (hit.collider != null)
            {
                movement *= (hit.distance / movementMagintude);
                // TODO - issues with sliding along walls (use normal here?)
            }
        }

        m_rigidbody.MovePosition(newPosition + movement);
    }

    private void Update()
    {
        UpdateInputs();

        if(GameManager.Instance.IsGameWon())
        {
            float spinSpeed = 50.0f;
            transform.Rotate(0.0f, 0.0f, spinSpeed * GameManager.Instance.GameDeltaTime);
        }
    }

    private void UpdateInputs()
    {
        if(GameManager.Instance.IsPaused || GameManager.Instance.IsGameWon())
        {
            MovementInput = Vector2.zero;
            return;
        }

        Vector2 movementInput = Vector2.zero;
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        if(movementInput.magnitude > 1.0f)
        {
            movementInput.Normalize();
        }

        MovementInput = movementInput;
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.GetComponent<Pickup>() != null)
        {
            _collider.GetComponent<Pickup>().StartSuck();
        }
    }

    private void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.GetComponent<Pickup>() != null)
        {
            _collider.GetComponent<Pickup>().StopSuck();
        }
    }
}
