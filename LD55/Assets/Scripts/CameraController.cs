using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Player == null)
        {
            return;
        }

        transform.position = GameManager.Instance.Player.transform.position + new Vector3(0, 0, -10);
    }
}
