using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_targetReticule = null;

    private FlockManager m_flockManager = null;
    private Player m_player = null;

    private void Start()
    {
        m_player = GameManager.Instance.Player;
        m_flockManager = GameManager.Instance.FlockManager;
    }

    private void Update()
    {
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        m_targetReticule.SetActive(!m_flockManager.IsTargettingPlayer);
        m_targetReticule.transform.position = m_flockManager.TargetPosition;
    }
}
