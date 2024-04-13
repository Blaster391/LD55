using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [SerializeField]
    private Summon m_summonPrefab = null;

    public bool IsTargettingPlayer { get; private set; } = true;
    public Vector2 TargetPosition { get; private set; } = Vector2.zero;
    public List<Summon> Flock { get; private set; } = new List<Summon>();
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
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

    public void Register(Enemy _enemy)
    {
        Enemies.Add(_enemy);
    }

    public void Unregister(Enemy _enemy)
    {
        Enemies.Remove(_enemy);
    }


    void Start()
    {
        m_player = GameManager.Instance.Player;
        GameManager.Instance.RunResources.SlimeAdded += OnSlimeAdded;
    }

    void Update()
    {
        if(m_player == null)
        {
            return;
        }

        if (GameManager.Instance.IsPaused())
        {
            return;
        }

        if (Input.GetButtonDown("FlockTarget"))
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

        FlockCenter = m_player.transform.position;
        if(Flock.Count > 0)
        {
            foreach (Summon s in Flock)
            {
                Vector2 sPosition = s.transform.position;
                FlockCenter += sPosition;
            }
            FlockCenter /= Flock.Count;
        }

        Enemies = Enemies.OrderBy(x => Vector2.Distance(x.transform.position, m_player.transform.position)).ToList();
    }

    private void OnSlimeAdded(SlimeAsset _slimeAsset)
    {
        Summon newSummon = Instantiate<Summon>(m_summonPrefab, GameManager.Instance.transform);
        newSummon.gameObject.name = $"{_slimeAsset.Name}{Flock.Count}";
        newSummon.Setup(_slimeAsset);
        newSummon.transform.position = m_player.transform.position + new Vector3(Random.value, Random.value, 0.0f) * 2.0f;

    }
}
