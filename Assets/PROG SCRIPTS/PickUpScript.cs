using UnityEngine;
using System;

public class Pickup : MonoBehaviour
{
    public static event Action OnPickupCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickupCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}