using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 20;  // Dano do projétil
    public float speed = 10f; // Velocidade do projétil
    public float lifetime = 5f; // Tempo de vida do projétil antes de desaparecer
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Pega a referência do Rigidbody2D
        rb.velocity = transform.right * speed;  // Move o projétil na direção certa

        // Destruir o projétil após um certo tempo
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
{
    if (collider.gameObject.CompareTag("Enemy"))
    {
        // Aplica o dano ao inimigo
        Enemy enemy = collider.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage, knockbackDirection);
        }

        // Destrói o projétil após a colisão
        Destroy(gameObject);
    }
}

}   