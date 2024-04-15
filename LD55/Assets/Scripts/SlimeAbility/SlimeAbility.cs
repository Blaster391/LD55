using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlimeAbility : MonoBehaviour
{
    [SerializeField] private float m_ActiveCooldown;

    private FlockManager m_FlockManager;

    private float m_CurrentActiveCooldown = 0;

    private void Awake()
    {
        m_FlockManager = GetComponentInParent<FlockManager>();
        if(m_FlockManager != null )
        {
            m_FlockManager.ActivateSlimeActives += TryActivateActive;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;

        m_CurrentActiveCooldown -= GameManager.Instance.GameDeltaTime;
        m_CurrentActiveCooldown = Mathf.Max(m_CurrentActiveCooldown, 0.0f);

        UpdatePassive(GameManager.Instance.GameDeltaTime);
    }

    private void TryActivateActive()
    {
        if (GameManager.Instance.IsPaused)
            return;

        if (m_CurrentActiveCooldown > 0)
            return;

        ActiveActive();
        m_CurrentActiveCooldown = m_ActiveCooldown;
    }

    protected abstract void UpdatePassive(float deltaTime);

    protected abstract void ActiveActive();
}
