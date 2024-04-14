using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    public enum State
    {
        Follow,
        Attack,
        AttackChest,
        Recoil
    }

    [SerializeField]
    private float m_movementForce = 1.0f;

    [SerializeField]
    private float m_targetPriority = 1.0f;

    [SerializeField]
    private float m_cohesion = 0.5f;

    [SerializeField]
    private float m_minFlockSeparationDistance = 0.5f;

    [SerializeField]
    private float m_minPlayerTargetDistance = 2.0f;

    [SerializeField]
    private float m_maxPlayerTargetDistance = 3.0f;

    [SerializeField]
    private float m_minTargetSeparationDistance = 1.0f;

    [SerializeField]
    private float m_maxTargetSeparationDistance = 2.0f;

    [SerializeField]
    private float m_separation = 0.5f;

    [SerializeField]
    private float m_follow = 0.75f;

    [SerializeField]
    private float m_damage = 1.0f;

    [SerializeField]
    private float m_aggroRange = 10.0f;

    [SerializeField]
    private float m_chestAggroRange = 7.0f;

    [SerializeField]
    private float m_bounceForce = 20.0f;

    [SerializeField]
    private float m_pushForce = 20.0f;

    [SerializeField]
    private float m_maxRecoilTime = 1.0f;

    private float m_playerTargetDistance = 2.0f;
    private float m_targetTargetDistance = 1.0f;

    private FlockManager m_flockManager = null;
    private WorldSpawnManager m_spawnManager = null;
    private State m_state = State.Follow;
    private Vector2 m_movement = Vector2.zero;
    private Rigidbody2D m_rigidbody = null;
    private Enemy m_targetEnemy = null;
    private Chest m_targetChest = null;
    private float m_recoilTime = 0.0f;

    public State GetState()
    {
        return m_state;
    }
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        m_flockManager = GameManager.Instance.FlockManager;
        m_spawnManager = GameManager.Instance.WorldSpawnManager;
        m_flockManager.Register(this);

        m_playerTargetDistance = Random.Range(m_minPlayerTargetDistance, m_maxPlayerTargetDistance);
        m_targetTargetDistance = Random.Range(m_minTargetSeparationDistance, m_maxTargetSeparationDistance);
    }

    private void OnDestroy()
    {
        m_flockManager.Unregister(this);
    }

    private void FixedUpdate()
    {
        m_rigidbody.AddForce(m_movement * m_movementForce * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused)
        {
            m_movement = Vector2.zero;
            return;
        }

        m_movement = Vector2.zero;

        switch (m_state)
        {
            case State.Follow:
                UpdateFollow();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.AttackChest:
                UpdateAttackChest();
                break;
            case State.Recoil:
                UpdateRecoil();
                break;
        }

        if (m_movement.magnitude > 1.0f)
        {
            m_movement.Normalize();
        }
    }

    private void UpdateFollow()
    {
        Vector2 targetPosition = m_flockManager.TargetPosition;

        Vector2 targetDirection = targetPosition - m_rigidbody.position;
        if ((m_flockManager.IsTargettingPlayer && targetDirection.magnitude < m_playerTargetDistance) || (!m_flockManager.IsTargettingPlayer && targetDirection.magnitude < m_targetTargetDistance))
        {
            targetDirection = -targetDirection;
        }
        else if((m_flockManager.IsTargettingPlayer && targetDirection.magnitude < m_playerTargetDistance * 1.1f)|| (!m_flockManager.IsTargettingPlayer && targetDirection.magnitude < m_targetTargetDistance * 1.1f))
        {
            targetDirection = Vector2.zero;
        }

        if(targetDirection.magnitude > 0.0f)
        {
            targetDirection.Normalize();
        }

        Vector2 separationDirection = Vector2.zero;
        int separationTargets = 0;

        foreach(Summon member in m_flockManager.Flock)
        {
            if(member == this)
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, member.transform.position);
            if (distance < m_minFlockSeparationDistance)
            {
                Vector2 direction = transform.position - member.transform.position;
                direction.Normalize();

                separationDirection += direction;
                ++separationTargets;
            }
        }
        if(separationTargets > 0)
        {
            separationDirection.Normalize();
        }

        Vector2 centerDirection = m_flockManager.FlockCenter - m_rigidbody.position;
        centerDirection.Normalize();

        m_movement += targetDirection * m_targetPriority;

        if (!m_flockManager.IsTargettingPlayer)
        {
            m_movement += centerDirection * m_cohesion;
        }

        m_movement += separationDirection * m_separation;

        if(GameManager.Instance.Player != null)
        {
            if (m_flockManager.IsTargettingPlayer)
            {
                m_movement += GameManager.Instance.Player.MovementInput * m_follow;
            }

            Vector2 playerPosition = GameManager.Instance.Player.transform.position;
            Vector2 directionToPlayer = (playerPosition - m_rigidbody.position);
            if ((directionToPlayer.magnitude < m_playerTargetDistance * 0.9f && Vector2.Distance(playerPosition, targetPosition) < Vector2.Distance(m_rigidbody.position, targetPosition)))
            {
                Vector2 normalToPlayer = new Vector2(-directionToPlayer.y, directionToPlayer.x);

                m_movement += normalToPlayer;
                m_movement -= targetDirection * m_targetPriority * 0.75f;
            }
        }

        foreach(Enemy enemy in m_flockManager.Enemies)
        {
            if(Vector2.Distance(transform.position, enemy.transform.position) < m_aggroRange)
            {
                m_targetEnemy = enemy;
                m_state = State.Attack;
                return;
            }
        }

        foreach(Chest chest in m_spawnManager.ActiveChests)
        {
            if (Vector2.Distance(transform.position, chest.transform.position) < m_chestAggroRange)
            {
                m_targetChest = chest;
                m_state = State.AttackChest;
                return;
            }
        }
    }

    private void UpdateAttack()
    {
        if(m_targetEnemy == null)
        {
            m_state = State.Follow;
            return;
        }

        if (Vector2.Distance(transform.position, m_targetEnemy.transform.position) > m_aggroRange * 1.25f)
        {
            m_targetEnemy = null;
            m_state = State.Follow;
            return;
        }

        Vector2 targetDirection = m_targetEnemy.transform.position - transform.position;
        m_movement = targetDirection;
    }

    private void UpdateAttackChest()
    {
        if (m_targetChest == null)
        {
            m_state = State.Follow;
            return;
        }

        if (Vector2.Distance(transform.position, m_targetChest.transform.position) > m_chestAggroRange * 1.25f)
        {
            m_targetChest = null;
            m_state = State.Follow;
            return;
        }

        Vector2 targetDirection = m_targetChest.transform.position - transform.position;
        m_movement = targetDirection;
    }

    private void UpdateRecoil()
    {
        m_movement = Vector2.zero;
        m_recoilTime += Time.deltaTime;

        if (m_recoilTime < m_maxRecoilTime)
        {
            return;
        }

        m_recoilTime = 0.0f;

        if (m_targetEnemy == null)
        {
            m_state = State.Follow;
        }
        else
        {
            m_state = State.Attack;
        }
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        m_rigidbody.AddForce(_collision.GetContact(0).normal * m_bounceForce, ForceMode2D.Impulse);

        if (_collision.collider.gameObject.GetComponent<Enemy>() != null)
        {
            _collision.rigidbody.AddForce(-_collision.GetContact(0).normal * m_pushForce, ForceMode2D.Impulse);
            _collision.collider.GetComponent<Enemy>().OnDamaged(m_damage);
        }

        if (_collision.collider.gameObject.GetComponent<Chest>() != null)
        {
            _collision.collider.gameObject.GetComponent<Chest>().Open();
        }

        if (_collision.collider.gameObject.GetComponent<Summon>() == null)
        {
            m_state = State.Recoil;
        }
    }

    public void Setup(SlimeAsset _slimeAsset)
    {
        m_movementForce = _slimeAsset.Speed;
        m_rigidbody.mass = _slimeAsset.Mass;
        m_damage = Random.Range(_slimeAsset.DamageMin, _slimeAsset.DamageMax);
        GetComponent<CircleCollider2D>().radius = _slimeAsset.Radius;
        GetComponent<SpriteHandler>().SetSpriteList(_slimeAsset.SpriteList);
    }
}
