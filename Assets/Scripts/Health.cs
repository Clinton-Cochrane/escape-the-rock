using System.Linq.Expressions;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"took {damage} damage! Remaining HP: {currentHealth}");
        if(currentHealth <= 0) Die();
    }

    public float GetHealthPercentage()
    {
        return currentHealth/maxHealth;
    } 

    private void Die()
    {
        Debug.Log($"{gameObject.name} had died!");
        Destroy(gameObject);
    }
}
