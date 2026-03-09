using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public string collectableName;

    public abstract void OnCollect();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect();
        }
    }
}