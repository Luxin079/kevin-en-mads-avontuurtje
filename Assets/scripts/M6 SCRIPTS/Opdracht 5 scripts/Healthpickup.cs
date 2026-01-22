using UnityEngine;

public class HealthPickup : Collectable
{
    public int healthAmount = 25;

    public override void OnCollect()
    {
        Debug.Log("Picked up health: +" + healthAmount);
        Destroy(gameObject);
    }
}