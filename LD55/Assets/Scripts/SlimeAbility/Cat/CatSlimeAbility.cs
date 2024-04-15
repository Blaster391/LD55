using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSlimeAbility : SlimeAbility
{
    [SerializeField]
    private Kitten m_kittenPrefab = null;

    [SerializeField]
    private int m_kittenCount = 10;

    protected override void ActiveActive()
    {
        for(int i = 0; i < m_kittenCount; i++)
        {
            Kitten kitten = Instantiate(m_kittenPrefab, GameManager.Instance.transform);
            kitten.transform.position = transform.position;

            float alpha = ((float)i / m_kittenCount) * Mathf.PI * 2.0f;

            kitten.Setup(new Vector2(Mathf.Sin(alpha), Mathf.Cos(alpha)));
        }
    }

    protected override void UpdatePassive(float deltaTime)
    {
    }
}
