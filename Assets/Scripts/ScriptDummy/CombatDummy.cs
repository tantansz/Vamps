using UnityEngine;

public class CombatDummy : MonoBehaviour
{
    [Header("Atributos")]
    public int maxHealth = 100; // Vida m�xima do dummy
    private int currentHealth; // Vida atual

    [Header("Componentes")]
    public Animator animator; // Refer�ncia ao Animator do dummy

    private void Start()
    {
        currentHealth = maxHealth; // Inicializa a vida do dummy
    }

    // M�todo chamado pelo PlayerCombat
    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (currentHealth <= 0) return; // Ignora se j� est� destru�do

        currentHealth -= damage; // Reduz a vida
        Debug.Log($"Dummy tomou dano! Vida atual: {currentHealth}"); // Apenas para depura��o
        animator.SetTrigger("TakeDamage"); // Ativa a anima��o de dano

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Dummy destru�do!"); // Apenas para depura��o
        Destroy(gameObject); // Destroi o objeto
    }

    private void OnDrawGizmosSelected()
    {
        // Apenas para visualizar o Collider na cena (opcional)
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center, GetComponent<Collider2D>().bounds.size);
    }
}
