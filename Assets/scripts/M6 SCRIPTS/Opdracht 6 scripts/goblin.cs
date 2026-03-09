using UnityEngine;

public class Goblin : Enemy
{
    public float health = 100f;
    public float speed = 2f;

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} kreeg geen rice {damage} damage HP: {health}");
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
        Debug.Log($"{gameObject.name} diddy wou je niet meer in zijn huis hebben");
        Destroy(gameObject);
    }
}