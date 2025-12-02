using UnityEngine.UIElements;
using UnityEngine;


    public class BaseEnemy : Monobehaviour
{
    [SerializeField] private float speed = 9;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private int health = 100;
    protected virtual void TakeDamage(int damage)

    {
        health -= damage;
    }
    protected virtual float CalculateRange(float modifier)
    {
        return range;
    }
}

public class Enemy : BaseEnemy
{
    GameObject player;
    private void Attack()
    {
        // Attack logic
    }

    private void OnTriggerEnter(Collision other)
    {
        if (other.CompareTag("Trap"))
        {
            TakeDamage(50);
        }
    }

    void Update()
    {
        if (Distance(gameObject.transform, position, player.transform.position) > CalculateRange(speed / 5)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Attack logic
    }

}
