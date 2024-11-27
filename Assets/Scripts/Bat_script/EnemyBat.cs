using UnityEngine;

public class EnemyBat : MonoBehaviour
{
    public int maxHealth = 100; // Vida máxima
    private int currentHealth; // Vida atual

    void Start()
    {
        currentHealth = maxHealth; // Inicializa a vida
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        // Apenas reduz a vida (ignora o knockbackDirection)
        Debug.Log($"TakeDamage called on {gameObject.name}");
        currentHealth -= damage;
        Debug.Log($"EnemyBat took {damage} damage. Current health: {currentHealth}");

        // Verifica se a vida chegou a 0 para destruir o morcego
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("EnemyBat died!");
        Destroy(gameObject); // Remove o objeto da cena
    }
}
