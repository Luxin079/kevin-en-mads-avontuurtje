using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public List<Collectable> collectables;

    void Start()
    {
        Debug.Log("Total collectables in scene: " + collectables.Count);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Collectable collectable = collision.gameObject.GetComponent<Collectable>();
        if (collectable != null)
        {
            collectable.OnCollect();
            collectables.Remove(collectable);
            Debug.Log("Collectables remaining: " + collectables.Count);
        }
    }
}