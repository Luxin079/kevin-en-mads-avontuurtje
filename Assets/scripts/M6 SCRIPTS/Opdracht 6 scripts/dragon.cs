using UnityEngine;

public class Dragon : Enemy
{
    public float health = 100f;
    public float speed = 2f;

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} kreeg {damage} damage HP: {health}");
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Attack(GameObject target)
    {
        Debug.Log($"{gameObject.name} valt aan!");
    }

    public virtual void Die()
    {
        Debug.Log($"{gameObject.name} diddys kidnapbus vertrok zonder hem");
        Destroy(gameObject);
    }
}