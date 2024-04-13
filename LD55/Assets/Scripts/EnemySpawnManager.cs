using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    float timer = 0.0f;

    [SerializeField]
    private List<GameObject> m_enemies = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 4.0f)
        {
            timer = 0.0f;

            GameObject enemy = Instantiate(m_enemies[Random.Range(0, m_enemies.Count)], GameManager.Instance.transform);
            enemy.transform.position = new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), 0.0f);
        }
    }
}
