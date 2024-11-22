using UnityEngine;

public class ArakBehaviour : MonoBehaviour
{
    public float patrolMin; // Limite mínimo da patrulha
    public float patrolMax; // Limite máximo da patrulha
    public float patrolSpeed = 2f; // Velocidade durante a patrulha
    public float chaseSpeed = 4f; // Velocidade durante a perseguição
    public float detectionRange = 5f; // Alcance de detecção do jogador
    public Transform player; // Referência ao jogador

    private bool movingRight = true; // Direção da patrulha
    private bool isChasing = false; // Estado atual
    private Rigidbody2D rb; // Referência ao Rigidbody
    private float initialY; // Posição inicial no eixo Y

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Movimento controlado exclusivamente pelo script

        initialY = transform.position.y; // Salva a posição inicial no eixo Y
    }

    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= detectionRange)
        {
            // Jogador dentro do alcance: iniciar perseguição
            isChasing = true;
        }
        else
        {
            // Jogador fora do alcance: voltar para patrulha
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Garantir que o Arak permaneça no chão
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
    }

    void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * patrolSpeed * Time.deltaTime;

            if (transform.position.x >= patrolMax)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.position += Vector3.left * patrolSpeed * Time.deltaTime;

            if (transform.position.x <= patrolMin)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void ChasePlayer()
    {
        // Somente mover no eixo X, ignorando o eixo Y
        float directionX = (player.position.x - transform.position.x);
        directionX = directionX > 0 ? 1 : -1; // Normaliza o valor para direita ou esquerda

        transform.position += new Vector3(directionX * chaseSpeed * Time.deltaTime, 0, 0);

        // Flip baseado na direção do movimento
        if (directionX > 0 && !movingRight)
        {
            movingRight = true;
            Flip();
        }
        else if (directionX < 0 && movingRight)
        {
            movingRight = false;
            Flip();
        }
    }

    void Flip()
    {
        // Inverte a escala no eixo X
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Corrige a posição para impedir o empurrão
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null && !playerRb.isKinematic)
            {
                playerRb.velocity = Vector2.zero; // Zera o movimento do player na colisão
            }

            rb.velocity = Vector2.zero; // Garante que o Arak também não se mova pela física
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar o range de detecção no Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualizar os limites de patrulha no Editor
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(patrolMin, transform.position.y, transform.position.z),
                        new Vector3(patrolMax, transform.position.y, transform.position.z));
    }
}
