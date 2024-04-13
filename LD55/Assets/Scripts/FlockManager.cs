using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public bool IsTargettingPlayer { get; private set; } = true;
    public Vector2 TargetPosition { get; private set; } = Vector2.zero;
    public List<Summon> Flock { get; private set; } = new List<Summon>();

    public Vector2 FlockCenter { get; private set; } = Vector2.zero;

    private Player m_player = null;


    public void Register(Summon _summon)
    {
        Flock.Add(_summon);
    }

    public void Unregister(Summon _summon)
    {
        Flock.Remove(_summon);
    }

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

        FlockCenter = Vector2.zero;
        if (IsTargettingPlayer)
        {
            TargetPosition = m_player.transform.position;
            FlockCenter = TargetPosition;
        }
        else if(Flock.Count > 0)
        {
            foreach (Summon s in Flock)
            {
                Vector2 sPosition = s.transform.position;
                FlockCenter += sPosition;
            }
            FlockCenter /= Flock.Count;
        }
    }
}
