using System.Collections.Generic;
using UnityEngine;

public class WorldSpawnManager : MonoBehaviour
{
    public List<Chest> ActiveChests { get; private set; } = new List<Chest>();

    [SerializeField]
    private GameObject m_worldParent = null;

    [SerializeField]
    private float m_worldWidth = 1000.0f;
    [SerializeField]
    private float m_worldHeight = 1000.0f;
    [SerializeField]
    private float m_resolution = 1.0f;

    [SerializeField]
    private List<GameObject> m_corals = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_decor = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_shells = new List<GameObject>();
    [SerializeField]
    private GameObject m_chest = null;
    [SerializeField]
    private GameObject m_funnySub = null;

    [SerializeField]
    private int m_coralCount = 100;
    [SerializeField]
    private int m_chestCount = 20;
    [SerializeField]
    private int m_decorCount = 200;
    [SerializeField]
    private int m_shellCount = 300;

    private bool[,] m_freeSpace;
    private int m_widthIndexScale = 10;
    private int m_heightIndexScale = 10;

    public bool IsSpaceClear(Vector2 _worldPos)
    {
        Vector2Int index = WorldToIndex(_worldPos);
        return m_freeSpace[index.x, index.y];
    }

    public void Register(Chest _chest)
    {
        ActiveChests.Add(_chest);
    }

    public void Unregister(Chest _chest) 
    {
        ActiveChests.Remove(_chest);
    }

    void Awake()
    {
        m_widthIndexScale = Mathf.CeilToInt(m_worldWidth / m_resolution);
        m_heightIndexScale = Mathf.CeilToInt(m_worldHeight / m_resolution);

        m_freeSpace = new bool[m_widthIndexScale, m_heightIndexScale];

        for(int i = 0; i < m_widthIndexScale; i++)
        {
            for(int j = 0; j < m_heightIndexScale; j++)
            {
                m_freeSpace[i, j] = true;
            }
        }

        // Ensure some of the spawn area is always clear
        m_freeSpace[m_widthIndexScale / 2, m_heightIndexScale / 2] = false;
        m_freeSpace[m_widthIndexScale / 2 + 1, m_heightIndexScale / 2] = false;
        m_freeSpace[m_widthIndexScale / 2 - 1, m_heightIndexScale / 2] = false;
        m_freeSpace[m_widthIndexScale / 2, m_heightIndexScale / 2 + 1] = false;
        m_freeSpace[m_widthIndexScale / 2, m_heightIndexScale / 2 - 1] = false;
        m_freeSpace[m_widthIndexScale / 2 - 1, m_heightIndexScale / 2 - 1] = false;
        m_freeSpace[m_widthIndexScale / 2 - 1 , m_heightIndexScale / 2 + 1] = false;
        m_freeSpace[m_widthIndexScale / 2 + 1, m_heightIndexScale / 2 - 1] = false;
        m_freeSpace[m_widthIndexScale / 2 + 1, m_heightIndexScale / 2 + 1] = false;

        for (int i = 0; i < m_chestCount; ++i)
        {
            Vector2 worldPos = Vector2.zero;
            worldPos.x = Random.Range(-m_worldWidth * 0.5f, m_worldWidth * 0.5f);
            worldPos.y = Random.Range(-m_worldHeight * 0.5f, m_worldHeight * 0.5f);

            Vector2Int index = WorldToIndex(worldPos);

            if (!m_freeSpace[index.x, index.y])
            {
                continue;
            }

            GameObject chest = Instantiate(m_chest, m_worldParent.transform);
            chest.transform.position = worldPos;
            m_freeSpace[index.x, index.y] = false;
        }

        {
            Vector2 worldPos = Vector2.zero;
            worldPos.x = Random.Range(-m_worldWidth * 0.5f, m_worldWidth * 0.5f);
            worldPos.y = Random.Range(-m_worldHeight * 0.5f, m_worldHeight * 0.5f);

            Vector2Int index = WorldToIndex(worldPos);

            if (m_freeSpace[index.x, index.y])
            {
                GameObject sub = Instantiate(m_funnySub, m_worldParent.transform);
                sub.transform.position = worldPos;
                m_freeSpace[index.x, index.y] = false;
            }
        }

        for (int i = 0; i < m_coralCount; ++i)
        {
            Vector2 worldPos = Vector2.zero;
            worldPos.x = Random.Range(-m_worldWidth * 0.5f, m_worldWidth * 0.5f);
            worldPos.y = Random.Range(-m_worldHeight * 0.5f, m_worldHeight * 0.5f);

            Vector2Int index = WorldToIndex(worldPos);

            if(!m_freeSpace[index.x, index.y])
            {
                continue;
            }

            GameObject coral = Instantiate(m_corals[Random.Range(0, m_corals.Count)], m_worldParent.transform);
            coral.transform.position = worldPos;
            coral.transform.localScale = new Vector3((Random.Range(0,2) == 0 ? 2 : -2), 2, 2);
            m_freeSpace[index.x, index.y] = false;
        }

        for (int i = 0; i < m_decorCount; ++i)
        {
            Vector2 worldPos = Vector2.zero;
            worldPos.x = Random.Range(-m_worldWidth * 0.5f, m_worldWidth * 0.5f);
            worldPos.y = Random.Range(-m_worldHeight * 0.5f, m_worldHeight * 0.5f);

            Vector2Int index = WorldToIndex(worldPos);

            if (!m_freeSpace[index.x, index.y])
            {
                continue;
            }

            GameObject decor = Instantiate(m_decor[Random.Range(0, m_decor.Count)], m_worldParent.transform);
            decor.transform.position = worldPos;
            decor.transform.localScale = new Vector3((Random.Range(0,2) == 0 ? 1 : -1), 1, 1);
            m_freeSpace[index.x, index.y] = false;
        }

        for (int i = 0; i < m_shellCount; ++i)
        {
            Vector2 worldPos = Vector2.zero;
            worldPos.x = Random.Range(-m_worldWidth * 0.5f, m_worldWidth * 0.5f);
            worldPos.y = Random.Range(-m_worldHeight * 0.5f, m_worldHeight * 0.5f);

            Vector2Int index = WorldToIndex(worldPos);

            if (!m_freeSpace[index.x, index.y])
            {
                continue;
            }

            GameObject shell = Instantiate(m_shells[Random.Range(0, m_shells.Count)], m_worldParent.transform);
            shell.transform.position = worldPos;
            shell.transform.Rotate(0,0, Random.Range(0,360));
            shell.transform.localScale = new Vector3(1, (Random.Range(0,2) == 0 ? 1 : -1), 1);
            m_freeSpace[index.x, index.y] = false;
        }
    }

    private Vector2Int WorldToIndex(Vector2 _worldPos)
    {
        int widthIndex = Mathf.Clamp(Mathf.RoundToInt((_worldPos.x + m_worldWidth * 0.5f) / m_resolution), 0, m_widthIndexScale - 1);
        int heightIndex = Mathf.Clamp(Mathf.RoundToInt((_worldPos.y + m_worldHeight * 0.5f) / m_resolution), 0, m_heightIndexScale - 1);

        return new Vector2Int(widthIndex, heightIndex);
    }
}
