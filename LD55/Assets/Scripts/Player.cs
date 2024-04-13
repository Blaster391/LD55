using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_movementSpeed = 1.0f;

    public Vector2 MovementInput { get; private set; } = Vector2.zero;

    private Rigidbody2D m_rigidbody = null;
    private float m_playerRadius = 1.0f;

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
            RaycastHit2D hit = Physics2D.CircleCast(newPosition, m_playerRadius * 0.51f, movement, movementMagintude, layerMask);
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
    }

    private void UpdateInputs()
    {
        Vector2 movementInput = Vector2.zero;
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        if(movementInput.magnitude > 1.0f)
        {
            movementInput.Normalize();
        }

        MovementInput = movementInput;
    }

    //private void OnCollisionEnter2D(Collision2D _collision)
    //{
    //    Vector2 newPosition = m_rigidbody.position;

    //    ColliderDistance2D contactDistance = Physics2D.Distance(_collision.collider, _collision.otherCollider);
    //    Vector2 resolutionDirection = contactDistance.normal * contactDistance.distance;
    //    newPosition -= resolutionDirection;

    //    m_rigidbody.MovePosition(newPosition);
    //}
}
