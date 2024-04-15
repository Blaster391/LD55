public class HealthPickup : Pickup
{
    public override void OnPickup()
    {
        GameManager.Instance.AudioManager.PickupHealth();
        GameManager.Instance.Player.AddHealth(1);
    }
}

