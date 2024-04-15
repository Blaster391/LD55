public class Coin : Pickup
{
    public override void OnPickup()
    {
        GameManager.Instance.AudioManager.PickupCoin();
        GameManager.Instance.RunResources.AddTokens(1);
    }
}
