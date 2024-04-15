using UnityEngine;

public class Shadow : MonoBehaviour
{
    private SpriteRenderer m_shadowSprite = null;
    private SpriteRenderer m_parentSprite = null;

    void Start()
    {
        m_shadowSprite = GetComponent<SpriteRenderer>();
        m_parentSprite = gameObject.transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_shadowSprite.sprite = m_parentSprite.sprite;
        m_shadowSprite.flipX = m_parentSprite.flipX;
        m_shadowSprite.flipY = m_parentSprite.flipY;
    }
}
