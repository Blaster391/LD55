using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyingSlimePassiveDamage : SlimeAbility
{
    [SerializeField] private float m_flyingDamage = 5.0f;
    private List<Enemy> m_affectedEnemies = new List<Enemy>();

    protected override void ActiveActive()
    {

    }

    protected override void UpdatePassive(float _deltaTime)
    {
        m_affectedEnemies = m_affectedEnemies.Where(x => x != null).ToList();

        foreach (var enemy in m_affectedEnemies)
        {
            enemy.OnDamaged(m_flyingDamage * _deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.GetComponent<Enemy>() != null)
        {
            m_affectedEnemies.Add(_collision.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.GetComponent<Enemy>() != null)
        {
            m_affectedEnemies.Remove(_collision.GetComponent<Enemy>());
        }
    }
}
