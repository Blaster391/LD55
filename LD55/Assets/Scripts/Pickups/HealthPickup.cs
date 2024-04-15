using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public override void OnPickup()
    {
        GameManager.Instance.AudioManager.PickupHealth();
        GameManager.Instance.Player.AddHealth(1);
    }
}

