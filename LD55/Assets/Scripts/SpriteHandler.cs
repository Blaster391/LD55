using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> m_sprites = new List<Sprite>();

    [SerializeField]
    private float m_updateTime = 1.0f;

    private int m_spriteIndex = 0;
    private float m_time = 0.0f;

    private SpriteRenderer m_renderer = null;
    private Vector2 m_previousPosition = Vector2.zero;

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
    }

    public void SetSpriteList(List<Sprite> _spriteList)
    {
        m_sprites = _spriteList;
        m_renderer.sprite = m_sprites[0];
    }

    void Start()
    {
        m_renderer.sprite = m_sprites[0];
        m_previousPosition = transform.position;

        m_time = Random.Range(0.0f, m_updateTime);
    }

    // Update is called once per frame
    void Update()
    {

        if(GetComponent<Summon>() != null && GetComponent<Summon>().GetState() == Summon.State.Recoil)
        {
            m_previousPosition = transform.position;
        }

        if(Vector2.Distance(transform.position, m_previousPosition) > 0.05f)
        {
            m_renderer.flipX = transform.position.x > m_previousPosition.x;
            m_previousPosition = transform.position;
        }

        m_time += GameManager.Instance.GameDeltaTime;

        if(m_time > m_updateTime)
        {
            m_time = 0.0f;

            m_spriteIndex++;
            if(m_spriteIndex >= m_sprites.Count)
            {
                m_spriteIndex = 0;
            }

            m_renderer.sprite = m_sprites[m_spriteIndex];
        }
    }
}
