using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitten : MonoBehaviour
{
    [SerializeField]
    private float m_damage = 3.0f;

    [SerializeField]
    private float m_speed = 10.0f;

    [SerializeField]
    private float m_spinSpeed = 360.0f;

    Vector2 m_direction = Vector2.up;
    float m_lifespan = 100.0f;

    public void Setup(Vector2 _direction)
    {
        m_direction = _direction;

        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        Color color = sprite.color;

        color.r = Random.value;
        color.g = Random.value;
        color.b = Random.value;

        sprite.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = GameManager.Instance.GameDeltaTime;

        transform.position = m_direction * deltaTime * m_speed;

        transform.Rotate(new Vector3(0,0,1) * Time.deltaTime * m_spinSpeed);

        m_lifespan -= deltaTime;
        if(m_lifespan < 0)
        {
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().OnDamaged(m_damage);
            Destroy(gameObject);
        }
    }
}
