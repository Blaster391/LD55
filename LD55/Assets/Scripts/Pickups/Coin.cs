using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup
{
    public override void OnPickup()
    {
        GameManager.Instance.RunResources.AddTokens(1);
    }
}
