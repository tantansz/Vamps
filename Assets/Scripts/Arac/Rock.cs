using UnityEngine;

public class Rock : MonoBehaviour
{
    public float speed = 5f; // Velocidade da pedra
    private Vector3 targetPosition; // Posição alvo da pedra

    public int damage = 10; // Dano que a pedra causa ao jogador

    public void Initialize(Vector3 target)
    {
        targetPosition = target;
    }

    void Update()
    {
        // Move a pedra em direção ao destino
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Destroi a pedra quando atinge o destino
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se colidiu com o jogador
        if (collision.CompareTag("Player"))
        {
            // Busca o script de controle de vida no jogador
            ControlaVida playerLife = collision.GetComponent<ControlaVida>();

            if (playerLife != null)
            {
                // Aplica dano ao jogador
                playerLife.TomarDano(damage);
            }

            // Destroi a pedra
            Destroy(gameObject);
        }
    }
}
