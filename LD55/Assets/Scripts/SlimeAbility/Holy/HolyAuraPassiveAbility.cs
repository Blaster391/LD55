using System.Collections.Generic;
using UnityEngine;

public class HolyAuraPassiveAbility : SlimeAbility
{
    [SerializeField] private float m_AuraDamage;
    [SerializeField] private float m_AuraDamageTickDuration;
    [SerializeField] private float m_AuraDamageTickForce;

    private float m_TimeSinceTick = 0;

    private List<Enemy> m_AffectedEnemies = new List<Enemy>();

    protected override void ActiveActive() { }

    protected override void UpdatePassive(float deltaTime)
    {
        m_TimeSinceTick += deltaTime;
        if(m_TimeSinceTick >= m_AuraDamageTickDuration)
        {
            m_TimeSinceTick -= m_AuraDamageTickDuration;

            List<Enemy> enemiesToHit = new List<Enemy>(m_AffectedEnemies); // In case something dies and the collection is modified
            foreach(Enemy enemy in enemiesToHit)
            {
                enemy.OnDamaged(m_AuraDamage);

                Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
                enemyRigidbody.AddForce((enemy.transform.position - transform.position).normalized * m_AuraDamageTickForce, ForceMode2D.Impulse);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enteredEnemy = collision.gameObject.GetComponent<Enemy>();
        if (enteredEnemy != null)
        {
            m_AffectedEnemies.Add(enteredEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enteredEnemy = collision.gameObject.GetComponent<Enemy>();
        if (enteredEnemy != null)
        {
            m_AffectedEnemies.Remove(enteredEnemy);
        }
    }
}
