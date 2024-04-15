using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        Attack,
        Charge,
        Recoil
    }

    [SerializeField]
    private float m_movementForce = 1.0f;

    [SerializeField]
    private float m_health = 3.0f;

    [SerializeField]
    private float m_maxRecoveryTime = 0.5f;

    [SerializeField]
    private float m_bounceForce = 100.0f;

    [SerializeField]
    private float m_summonBounceForce = 0.0f;

    [SerializeField]
    private bool m_isBoss = false;

    [SerializeField]
    private bool m_ignoreRecoil = false;

    [SerializeField]
    private bool m_canCharge = false;
    [SerializeField]
    private float m_chargeForce = 1000.0f;
    [SerializeField]
    private float m_chargeTime = 1.0f;
    [SerializeField]
    private float m_chargeCooldown = 3.0f;
    [SerializeField]
    private float m_chargeRange = 5.0f;

    private float m_lastChargeTime = 0.0f;


    [System.Serializable]
    private struct DropData
    {
        public GameObject DropPrefab;
        public int Min;
        public int Max;
    }

    [SerializeField]
    private List<DropData> Drops;

    public event System.Action<Enemy> Died;

    private FlockManager m_flockManager = null;
    private Vector2 m_movement = Vector2.zero;
    private Rigidbody2D m_rigidbody = null;
    private Player m_player = null;
    private float m_recoveryTime = 0.0f;
    private State m_state = State.Attack;

    public State GetState()
    {
        return m_state;
    }

    public void OnDamaged(float _damage)
    {
        if(m_state == State.Attack && !m_ignoreRecoil)
        {
            m_recoveryTime = m_maxRecoveryTime * (_damage / 10.0f);
            m_state = State.Recoil;
        }
 
        m_health -= _damage;
        if (m_health <= 0.0f)
        {
            GameManager.Instance.AudioManager.SlimeKill();

            Died?.Invoke(this);

            foreach(DropData dropData in Drops)
            {
                int dropCount = Random.Range(dropData.Min, dropData.Max + 1);
                for (int i = 0; i < dropCount; i++)
                {
                    GameObject drop = Instantiate(dropData.DropPrefab, GameManager.Instance.transform);
                    drop.transform.position = transform.position;
                }
            }
    
            if(m_isBoss)
            {
                GameManager.Instance.OnBossKilled();
            }

            Destroy(gameObject);
        }
    }

    void Start()
    {
        m_flockManager = GameManager.Instance.FlockManager;
        m_player = GameManager.Instance.Player;
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_flockManager.Register(this);
    }

    private void OnDestroy()
    {
        m_flockManager.Unregister(this);
    }

    private void FixedUpdate()
    {
        if (m_player == null)
        {
            return;
        }

        float speedMod = 1.0f;
        if(GetComponent<PufferScript>() != null && !GetComponent<PufferScript>().IsGrowing())
        {
            speedMod = 1.5f;
        }

        m_rigidbody.AddForce(m_movement * m_movementForce * speedMod * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if(m_player == null)
        {
            return;
        }

        if (GameManager.Instance.IsGameWon())
        {
            Destroy(gameObject);
            return;
        }

        if (GameManager.Instance.IsPaused)
        {
            m_movement = Vector2.zero;
            return;
        }

        float distFromPlayer = Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position);

        GetComponent<Collider2D>().enabled = distFromPlayer < 20.0f;

        if (!m_isBoss && distFromPlayer > 30.0f)
        {
            Destroy(gameObject);
            return;
        }

        if(m_state == State.Recoil && m_ignoreRecoil)
        {
            m_state = State.Attack;
        }

        switch (m_state)
        {
            case State.Attack:
                UpdateAttack();
                break;
            case State.Charge:
                UpdateCharge();
                break;
            case State.Recoil:
                UpdateRecoil();
                break;
        }

    }

    private void UpdateAttack()
    {
        Vector2 target = (m_player.transform.position - transform.position);
        m_movement = target;

        if(m_canCharge && GameManager.Instance.GameTime > m_lastChargeTime + m_chargeCooldown)
        {
            if(m_movement.magnitude < m_chargeRange)
            {
                m_lastChargeTime = -1.0f;
                m_state = State.Charge;
                m_movement.Normalize();
                return;
            }
        }

        m_movement.Normalize();
    }

    private void UpdateRecoil()
    {
        m_recoveryTime -= Time.deltaTime;
        m_movement = Vector2.zero;
        if(m_recoveryTime < 0.0f)
        {
            m_state = State.Attack;
        }
    }

    private void UpdateCharge()
    {
        if(m_lastChargeTime < 0.0f)
        {
            Vector2 target = (m_player.transform.position - transform.position);
            m_movement = target;
            m_movement.Normalize();

            m_rigidbody.AddForce(m_movement * m_chargeForce, ForceMode2D.Impulse);

            m_lastChargeTime = GameManager.Instance.GameTime;
        }

        if (GameManager.Instance.GameTime > m_lastChargeTime + m_chargeTime)
        {
            m_state = State.Attack;
            return;
        }

        m_movement = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if(m_state == State.Recoil)
        {
            return;
        }

        if (_collision.collider.gameObject.GetComponent<Summon>() != null)
        {
            _collision.collider.GetComponent<Rigidbody2D>().AddForce(-_collision.GetContact(0).normal * m_summonBounceForce, ForceMode2D.Impulse);
        }

        if (_collision.collider.gameObject.GetComponent<Player>() != null)
        {
            m_rigidbody.AddForce(_collision.GetContact(0).normal * m_bounceForce, ForceMode2D.Impulse);
            _collision.collider.gameObject.GetComponent<Player>().TakeDamage();
            m_recoveryTime = m_maxRecoveryTime;
            m_state = State.Recoil;
        }
    }
}
