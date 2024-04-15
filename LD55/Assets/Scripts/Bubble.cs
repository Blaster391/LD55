using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    float m_minSpeed = 1.0f;
    [SerializeField]
    float m_maxSpeed = 1.5f;

    float m_speed = 1.0f;
    float m_lifespan = 120.0f;

    void Start()
    {
        m_speed = Random.Range(m_minSpeed, m_maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.up * m_speed * Time.deltaTime);

        m_lifespan -= Time.deltaTime;
        if(m_lifespan < 0 )
        {
            Destroy(gameObject);
        }
    }
}
