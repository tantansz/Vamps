using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Transform player;  // Referência ao player para o monstro perseguir
    
    public float speed = 2f;  // Velocidade de movimento do monstro
    public float chaseRange = 5f;  // Distância máxima em que o monstro começa a perseguir o player
    public float returnSpeed = 1f;  // Velocidade de retorno ao ponto de patrulha quando o player escapar
    public float jumpForce = 5f;  // Força de pulo do monstro
    
    public LayerMask groundLayer;  // Camada para detectar o chão e os obstáculos
    public Transform groundCheck;  // Ponto de verificação para detecção do chão
    public float groundCheckRadius = 0.1f;  // Raio de verificação do chão (para determinar se o monstro está no chão)
    public Transform obstacleCheck;  // Ponto de verificação para detecção de obstáculos à frente
    public float obstacleCheckDistance = 0.5f;  // Distância para o monstro detectar obstáculos
    
    public int damage = 10; // Definindo a quantidade de dano
    public float damageInterval = 1f; // Intervalo de dano do monstro
    private float nextDamageTime = 0f;  // Tempo para aplicar o próximo dano

    private Rigidbody2D rb;  // Referência ao componente Rigidbody2D do monstro para controlar seu movimento
    private bool isChasing = false;  // Flag para verificar se o monstro está perseguindo o player
    private bool isGrounded = false;  // Flag para verificar se o monstro está no chão

    
    public Transform pointA;  // Ponto de patrulha A
    public Transform pointB;  // Ponto de patrulha B
    private bool movingToA = true;  // Flag para indicar se o monstro está indo para o ponto A

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Pega a referência ao Rigidbody2D do monstro
    }

    void Update()
    {
        // Verifica se o monstro está no chão
        isGrounded = IsGrounded();

        // Calcula a distância entre o monstro e o player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Se o player estiver dentro da distância de perseguição, o monstro começa a perseguir
        if (distanceToPlayer < chaseRange)
        {
            ChasePlayer();
        }
        // Se o monstro estava perseguindo, mas o player escapou, ele retorna ao ponto de patrulha mais próximo
        else if (isChasing)
        {
            ReturnToPatrol();
        }
        // Se o monstro não está perseguindo, ele patrulha entre os pontos A e B
        else
        {
            Patrol(); 
        }
    }




///////////////////////////PATRULHA//////////////////////////////////////////
    void Patrol()
    {
        // Verifica se o monstro deve se mover para o ponto A ou B
        Transform targetPoint = movingToA ? pointA : pointB;
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        // Move o monstro na direção do ponto de patrulha
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Verifica se o monstro chegou no ponto de patrulha
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            movingToA = !movingToA;  // Alterna o destino entre A e B
        }

        // Se o monstro está no chão e há um obstáculo à frente, ele pula
        if (isGrounded && IsObstacleAhead())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  // Colocando uma força para o monstro pular
        }
    }
///////////////////////////////////////////////////////////////////////////






/////////////////////////PERSEGUIÇÃO E RETORNO CASO NÃO VEJA MAIS O PLAYER///////////////////////////
    void ChasePlayer()
    {
        isChasing = true;  // Define que o monstro está perseguindo o player

        // Calcula a direção do monstro em direção ao player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move o monstro em direção ao player na direção calculada
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Se o monstro está no chão e há um obstáculo à frente, ele pula
        if (isGrounded && IsObstacleAhead())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  // Colocando uma força para o monstro pular
        }
    }

    void ReturnToPatrol()
    {
        // Calcula o ponto de patrulha mais próximo (A ou B)
        Transform closestPoint = Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position) ? pointA : pointB;

        // Move o monstro de volta para o ponto de patrulha mais próximo
        Vector2 direction = (closestPoint.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * returnSpeed, rb.velocity.y);

        // Se o monstro está no chão e há um obstáculo à frente, ele pula
        if (isGrounded && IsObstacleAhead())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  // Colocando força denovo para o monstro pular
        }

        // Se o monstro está próximo ao ponto de patrulha, ele retoma a patrulha normal
        if (Vector2.Distance(transform.position, closestPoint.position) < 0.2f)
        {
            isChasing = false;  // Define que o monstro não está mais perseguindo o player
            movingToA = closestPoint == pointB;  // Decide para qual ponto deve continuar a patrulha
        }
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////





//////////////////////////VERIFICADORES///////////////////////////////////////
    // Verifica se o monstro está no chão, usando um círculo invisível no ponto groundCheck
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Verifica se há um obstáculo à frente do monstro
    bool IsObstacleAhead()
    {
        return Physics2D.Raycast(obstacleCheck.position, transform.right, obstacleCheckDistance, groundLayer);
    }
///////////////////////////////////////////////////////////////////////////

///////////////////////////DANO NO PLAYER///////////////////////////////
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Se o monstro continuar colidindo com o player, causar dano contínuo
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            ControlaVida controlaVida = collision.gameObject.GetComponent<ControlaVida>();
            if (controlaVida != null)
            {
                controlaVida.TomarDano(damage);  
                nextDamageTime = Time.time + damageInterval;  // Define o tempo para o próximo dano
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se o monstro colidir com o player, causar dano
        if (collision.gameObject.CompareTag("Player"))
        {
            ControlaVida controlaVida = collision.gameObject.GetComponent<ControlaVida>();
            if (controlaVida != null)
            {
                controlaVida.TomarDano(damage);  
            }
        }
    }
///////////////////////////////////////////////////////////////////////////
}
