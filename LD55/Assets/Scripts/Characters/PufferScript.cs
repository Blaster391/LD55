using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferScript : MonoBehaviour
{
    [SerializeField]
    private float m_maxPuff = 3.0f;
    [SerializeField]
    private float m_maxPuffTime = 10.0f;

    private bool m_grow = true;
    private float m_puffTime = 0.0f;
    private Vector3 m_scaleRef = Vector3.zero;

    void Start()
    {
        m_scaleRef = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_grow)
        {
            m_puffTime += Time.deltaTime;
            if(m_puffTime > m_maxPuffTime)
            {
                m_grow = false;
            }
        }
        else
        {
            m_puffTime -= Time.deltaTime;
            if (m_puffTime < 0.0f)
            {
                m_grow = true;
            }
        }

        float size = Mathf.Lerp(1.0f, m_maxPuff, m_puffTime / m_maxPuffTime);
        transform.localScale = m_scaleRef * size;

    }
}
