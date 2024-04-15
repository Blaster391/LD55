using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaFlamethrowerAbility : SlimeAbility
{
    [SerializeField] private float m_FlamethrowerCooldown;
    [SerializeField] private float m_FlamethrowerDuration;

    [SerializeField] private float m_FlamethrowerTickCooldown;
    [SerializeField] private SlimeProjectile m_FlamethrowerProjectile;
    [SerializeField] private float m_ProjectileSpeed;

    private float m_TimeSinceFlamethrower = 0;
    private bool m_FlamethrowerActive = false;

    private float m_TimeSinceFlamethrowerTick = 0;

    protected override void ActiveActive()
    {
    }

    protected override void UpdatePassive(float deltaTime)
    {
        if(GameManager.Instance.Player == null)
        {
            return;
        }

        if(m_FlamethrowerActive)
        {
            m_TimeSinceFlamethrowerTick += deltaTime;
            if (m_TimeSinceFlamethrowerTick > m_FlamethrowerTickCooldown)
            {
                m_TimeSinceFlamethrowerTick = 0;

                Summon summon = GetComponentInParent<Summon>();
                Vector3 target = GameManager.Instance.Player.transform.position;
                Vector2 direction = (transform.position - target).normalized;
                if (summon.GetAttackTarget() != null)
                {
                    target = summon.GetAttackTarget().transform.position;
                    direction = (target - transform.position).normalized;
                }

                SpawnFlames(direction);
            }

            m_TimeSinceFlamethrower += deltaTime;
            if(m_TimeSinceFlamethrower >= m_FlamethrowerDuration)
            {
                m_TimeSinceFlamethrower = 0;
                m_FlamethrowerActive = false;
            }
        }
        else
        {
            m_TimeSinceFlamethrower += deltaTime;
            if (m_TimeSinceFlamethrower >= m_FlamethrowerCooldown)
            {
                m_TimeSinceFlamethrower = 0;

                // Start flamin
                m_FlamethrowerActive = true;
                m_TimeSinceFlamethrowerTick = m_FlamethrowerTickCooldown;
            }
        }
    }

    private void SpawnFlames(Vector2 mainDirection)
    {
        Vector2 leftDirection = Quaternion.AngleAxis(-10, Vector3.forward) * mainDirection;
        Vector2 rightDirection = Quaternion.AngleAxis(10, Vector3.forward) * mainDirection;

        SpawnFlame(mainDirection);
        SpawnFlame(leftDirection);
        SpawnFlame(rightDirection);
    }

    private void SpawnFlame(Vector2 direction)
    {
        SlimeProjectile projectile = Instantiate(m_FlamethrowerProjectile, transform.position, Quaternion.identity, GameManager.Instance.transform);
        Rigidbody2D projectileRigidbody = projectile.gameObject.GetComponent<Rigidbody2D>();
        projectileRigidbody.AddForce(direction * m_ProjectileSpeed, ForceMode2D.Impulse);
    }
}
