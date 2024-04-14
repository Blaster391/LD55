using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single element of a long trail
/// Feeds back overlapping enemies to the owner and lets it use them to apply its effects
/// </summary>
public class TrailAffector : MonoBehaviour
{
    private List<Enemy> m_AffectedEnemies = new List<Enemy>();

    public IReadOnlyList<Enemy> AffectedEnemies => m_AffectedEnemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enteredEnemy = collision.gameObject.GetComponent<Enemy>();
        if (enteredEnemy != null)
        {
            m_AffectedEnemies.Add(enteredEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enteredEnemy = collision.gameObject.GetComponent<Enemy>();
        if (enteredEnemy != null)
        {
            m_AffectedEnemies.Remove(enteredEnemy);
        }
    }
}
