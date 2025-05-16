using UnityEngine;
using System.Collections;

public class ItemDisappear : MonoBehaviour
{
    private ParticleSystem pickupEffect;

    private void Start()
    {
        pickupEffect = GetComponentInChildren<ParticleSystem>(true); // Get the child particle system
        if (pickupEffect != null)
        {
            pickupEffect.Stop(); // Ensure it's stopped initially
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasCharacterController(other.transform))
        {
            StartCoroutine(ShowEffectAndDisable());
        }
    }

    private IEnumerator ShowEffectAndDisable()
    {
        if (pickupEffect != null)
        {
            pickupEffect.Play(); // Play particle effect
            yield return new WaitForSeconds(1f); // Wait for 1 second
            pickupEffect.Stop(); // Stop particle effect
        }
        gameObject.SetActive(false); // Disable the item instead of destroying it
    }

    private bool HasCharacterController(Transform obj)
    {
        while (obj != null)
        {
            if (obj.GetComponent<CharacterController>() != null)
            {
                return true;
            }
            obj = obj.parent; // Move up the hierarchy
        }
        return false;
    }
}
