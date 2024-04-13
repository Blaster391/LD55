using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public bool IsTargettingPlayer { get; private set; } = true;
    public Vector2 TargetPosition { get; private set; } = Vector2.zero;

    private Player m_player = null;

    void Start()
    {
        m_player = GameManager.Instance.Player;
    }

    void Update()
    {
        if(m_player == null)
        {
            return;
        }

        if(Input.GetButtonDown("FlockTarget"))
        {
            IsTargettingPlayer = false;
            TargetPosition = GameHelper.MouseToWorldPosition();
        }
        else if(Input.GetButtonDown("FlockRecall"))
        {
            IsTargettingPlayer = true;
        }

        if(IsTargettingPlayer)
        {
            TargetPosition = m_player.transform.position;
        }
    }
}
