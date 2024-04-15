using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    private Sprite m_openSprite = null;

    [SerializeField]
    private int m_maxCoins = 10;

    [SerializeField]
    private int m_minCoints = 3;

    [SerializeField]
    private GameObject m_coinPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.WorldSpawnManager.Register(this);
    }

    private void OnDestroy()
    {
        GameManager.Instance.WorldSpawnManager.Unregister(this);
    }

    public void Open()
    {
        GameManager.Instance.AudioManager.SlimeKill();

        GetComponent<SpriteRenderer>().sprite = m_openSprite;
        GetComponent<Collider2D>().enabled = false;

        int coinsToSpawn = Random.Range(m_minCoints, m_maxCoins + 1);
        for(int i = 0; i < coinsToSpawn; i++)
        {
            GameObject coin = Instantiate(m_coinPrefab, transform.parent);
            coin.transform.position = transform.position + new Vector3(Random.value, (Random.value - 0.5f) * 2.0f);
        }

        // Kill the component, leave the sprite
        Destroy(this);
    }
}
