using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickup : Pickup
{
    public override void OnPickup()
    {
        GameManager.Instance.AudioManager.PickupScore();
        GameManager.Instance.AddScore(1);
    }
}
