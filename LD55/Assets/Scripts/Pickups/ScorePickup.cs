public class ScorePickup : Pickup
{
    public override void OnPickup()
    {
        GameManager.Instance.AudioManager.PickupScore();
        GameManager.Instance.AddScore(1);
    }
}
