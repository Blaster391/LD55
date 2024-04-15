using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TrailSlimeAbility : SlimeAbility
{
    [SerializeField] private TrailAffector m_TrailAffectorPrefab;
    [SerializeField] private float m_TrailDropDelay;
    [SerializeField] private float m_TrailDropLifetime;

    [SerializeField] private float m_TrailAffectCooldown;

    private float m_TimeSinceTrailDrop = 0;
    private Dictionary<TrailAffector, float> m_ActiveAffectors = new Dictionary<TrailAffector, float>();

    private float m_TimeSinceTrailAffect;

    protected override void ActiveActive() { }

    protected override void UpdatePassive(float deltaTime)
    {
        if (GameManager.Instance.IsPaused)
            return;

        m_TimeSinceTrailDrop += GameManager.Instance.GameDeltaTime;
        if(m_TimeSinceTrailDrop >= m_TrailDropDelay)
        {
            TrailAffector affector = Instantiate(m_TrailAffectorPrefab, transform.position, Quaternion.identity, GameManager.Instance.transform);
            m_ActiveAffectors.Add(affector, 0);
            m_TimeSinceTrailDrop = 0;
        }

        HashSet<Enemy> affectedEnemies = new HashSet<Enemy>();
        List<TrailAffector> affectors = new List<TrailAffector>(m_ActiveAffectors.Keys);
        foreach (TrailAffector affector in affectors)
        {
            affectedEnemies.AddRange(affector.AffectedEnemies);

            m_ActiveAffectors[affector] += GameManager.Instance.GameDeltaTime;
            if (m_ActiveAffectors[affector] >= m_TrailDropLifetime)
            {
                m_ActiveAffectors.Remove(affector);
                Destroy(affector.gameObject);
            }
        }

        m_TimeSinceTrailAffect += GameManager.Instance.GameDeltaTime;
        if (m_TimeSinceTrailAffect >= m_TrailAffectCooldown)
        {
            m_TimeSinceTrailAffect = 0;

            foreach (Enemy affectedEnemy in affectedEnemies)
            {
                AffectEnemy(affectedEnemy);
            }
        }
    }

    protected abstract void AffectEnemy(Enemy enemy);
}
