using System.Collections;
using UnityEngine;

public class BatAI : MonoBehaviour
{
    public Transform player;  // Referência ao player
    public Transform groundCheck;  // Objeto abaixo do morcego para verificar o chão
    public float groundCheckRadius = 0.2f; // Raio para verificar o chão
    public LayerMask groundLayer; // Camada do chão para detectar colisão

    public float patrolSpeed = 2f;  // Velocidade de patrulha
    public float diveSpeed = 8f;  // Velocidade do rasante
    public float chaseRange = 5f;  // Distância para detectar o player
    public float diveCooldown = 2f;  // Tempo entre rasantes

    public float patrolMinX = -5f;  // Limite mínimo no eixo X
    public float patrolMaxX = 5f;   // Limite máximo no eixo X
    public float patrolMaxY = 5f;   // Altura máxima de patrulha

    public int damageToPlayer = 20; // Dano que o morcego causa ao player

    private Rigidbody2D rb;  // Referência ao Rigidbody2D
    private bool isDiving = false;  // Flag para verificar se está realizando o rasante
    private bool isReturning = false;  // Flag para verificar se está retornando à patrulha
    private bool isOnCooldown = false;  // Flag para controlar o cooldown do rasante
    private bool movingToRight = true; // Direção horizontal inicial
    private Vector3 diveDirection;  // Direção do rasante

    private Vector3 initialScale; // Escala inicial para o Flip

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Impede que o morcego gire
        initialScale = transform.localScale; // Salva a escala inicial
    }

    void Update()
    {
        if (isDiving || isReturning || isOnCooldown) return; // Se está ocupado, não executa outras ações

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Se o player estiver no alcance, inicia o rasante
        if (distanceToPlayer < chaseRange)
        {
            StartDive();
        }
        else
        {
            Patrol(); // Caso contrário, continua patrulhando
        }
    }

    ///////////////////////////RASANTE//////////////////////////////////////////
    void StartDive()
    {
        isDiving = true; // Ativa o estado de rasante
        diveDirection = (player.position - transform.position).normalized; // Calcula a direção em direção ao player

        // Ajusta a direção do morcego para o player
        if ((player.position.x > transform.position.x && !movingToRight) || (player.position.x < transform.position.x && movingToRight))
        {
            Flip(player.position.x > transform.position.x);
        }

        StartCoroutine(DiveCoroutine());
    }

    IEnumerator DiveCoroutine()
    {
        while (isDiving)
        {
            rb.velocity = diveDirection * diveSpeed; // Move o morcego na direção do rasante

            // Verifica se o GroundCheck está tocando o chão
            if (IsNearGround())
            {
                isDiving = false; // Finaliza o rasante
                rb.velocity = Vector2.zero; // Para o movimento
                StartReturn(); // Inicia o retorno ao ponto superior
            }

            yield return null;
        }
    }

    ///////////////////////////DANO AO PLAYER///////////////////////////////////
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDiving) return; // Apenas aplica dano durante o rasante

        // Verifica se o objeto colidido é o player
        if (collision.collider.CompareTag("Player"))
        {
            // Acessa o script de vida do player e aplica dano
            ControlaVida playerHealth = collision.collider.GetComponent<ControlaVida>();
            if (playerHealth != null)
            {
                playerHealth.TomarDano(damageToPlayer); // Aplica o dano ao player
                Debug.Log("Morcego causou dano ao player!");
            }

            // Finaliza o rasante e inicia o retorno
            isDiving = false;
            rb.velocity = Vector2.zero;
            StartReturn();
        }
    }
    ///////////////////////////////////////////////////////////////////////////

    ///////////////////////////RETORNO AO PONTO SUPERIOR/////////////////////////
    void StartReturn()
    {
        isReturning = true; // Ativa o estado de retorno
        Vector3 returnPoint = new Vector3(transform.position.x, patrolMaxY, transform.position.z); // Define o ponto superior de retorno
        StartCoroutine(ReturnCoroutine(returnPoint));
    }

    IEnumerator ReturnCoroutine(Vector3 returnPoint)
    {
        while (isReturning)
        {
            // Move o morcego para o ponto superior de patrulha
            Vector3 direction = (returnPoint - transform.position).normalized;
            rb.velocity = direction * patrolSpeed;

            // Se atingir o ponto superior, termina o retorno
            if (Vector3.Distance(transform.position, returnPoint) < 0.2f)
            {
                isReturning = false; // Finaliza o retorno
                rb.velocity = Vector2.zero; // Para o movimento
                isOnCooldown = true; // Ativa o cooldown
                yield return new WaitForSeconds(diveCooldown); // Aguarda o cooldown
                isOnCooldown = false; // Sai do cooldown
            }

            yield return null;
        }
    }
    ///////////////////////////////////////////////////////////////////////////

    ///////////////////////////PATRULHA//////////////////////////////////////////
    void Patrol()
    {
        Vector2 velocity = rb.velocity;

        // Movimentação horizontal
        if (movingToRight)
        {
            velocity.x = patrolSpeed;
            if (transform.position.x >= patrolMaxX)
            {
                movingToRight = false;
                Flip(false); // Vira para a esquerda
            }
        }
        else
        {
            velocity.x = -patrolSpeed;
            if (transform.position.x <= patrolMinX)
            {
                movingToRight = true;
                Flip(true); // Vira para a direita
            }
        }

        // Movimentação vertical fixa
        velocity.y = Mathf.Sin(Time.time * patrolSpeed) * patrolMaxY; // Movimento de patrulha

        rb.velocity = velocity;
    }
    ///////////////////////////////////////////////////////////////////////////

    ///////////////////////////DETECÇÃO DO CHÃO (GROUND CHECK)///////////////////
    bool IsNearGround()
    {
        // Verifica se o GroundCheck está colidindo com o chão
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    ///////////////////////////////////////////////////////////////////////////

    ///////////////////////////FLIP///////////////////////////////////////////
    void Flip(bool isMovingRight)
    {
        movingToRight = isMovingRight;
        Vector3 localScale = transform.localScale;
        localScale.x = isMovingRight ? Mathf.Abs(initialScale.x) : -Mathf.Abs(initialScale.x);
        transform.localScale = localScale;
    }
    ///////////////////////////////////////////////////////////////////////////

    ///////////////////////////DEBUG PARA GROUND CHECK/////////////////////////
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    ///////////////////////////////////////////////////////////////////////////
}
