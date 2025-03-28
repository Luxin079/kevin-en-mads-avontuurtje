using UnityEngine;

public class ItemDisappear : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object or any of its parents has a CharacterController
        if (HasCharacterController(other.transform))
        {
            Destroy(gameObject); // Make the item disappear
        }
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
