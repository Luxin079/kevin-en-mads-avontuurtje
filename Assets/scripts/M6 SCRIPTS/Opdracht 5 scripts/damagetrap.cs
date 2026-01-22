using UnityEngine;

public class DamageTrap : Collectable
{
    public int damage = 20;

    public override void OnCollect()
    {
        Debug.Log("Trap triggered! Damage: " + damage);
        Destroy(gameObject);
    }
}