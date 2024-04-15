using UnityEngine;

public class MagmaTrailAbility : TrailSlimeAbility
{
    [SerializeField] private float m_MagmaDamage;

    protected override void AffectEnemy(Enemy enemy)
    {
        enemy.OnDamaged(m_MagmaDamage);
    }
}
