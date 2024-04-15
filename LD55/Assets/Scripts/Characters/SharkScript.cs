using UnityEngine;

public class SharkScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.collider.gameObject.layer == 7)
        {
            GameManager.Instance.AudioManager.SlimeKill();
            Destroy(_collision.collider.gameObject);
        }
    }
}
