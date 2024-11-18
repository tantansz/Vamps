using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArakiAI : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public GameObject rockPrefab; // Prefab da pedra
    public Animator animator; // Animator para as animações do Araki

    public float patrolMinX = 90f; // Limite mínimo da patrulha
    public float patrolMaxX = 100f; // Limite máximo da patrulha
    public float patrolSpeed = 2f; // Velocidade de patrulha
    public float chaseSpeed = 3f; // Velocidade de perseguição

    public float detectionRange = 10f; // Alcance de detecção do jogador
    public float attackCooldown = 2f; // Tempo entre ataques
    public float attackRange = 10f; // Alcance para atacar o jogador

    private bool movingToRight = true; // Controle da direção da patrulha
    private bool isChasing = false; // Flag para indicar se está perseguindo
    private float lastAttackTime = 0f; // Tempo do último ataque
    private Vector3 lastPlayerPosition; // Última posição do jogador no momento do ataque

    private Rigidbody2D rb; // Referência ao Rigidbody2D do Araki

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Bloqueia a rotação
    }

    void Update()
    {
        // Calcula a distância até o jogador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Lógica de perseguição e ataque
        if (distanceToPlayer < detectionRange)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            // Patrulha entre os limites
            Patrol();
        }
    }

    /////////////////////////PATRULHA///////////////////////////////////////////
    void Patrol()
    {
        // Move para a direita ou esquerda baseado na direção
        if (movingToRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(patrolMaxX, transform.position.y, transform.position.z), patrolSpeed * Time.deltaTime);

            // Chegou ao limite máximo, inverte a direção
            if (transform.position.x >= patrolMaxX)
            {
                movingToRight = false;
                Flip(false);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(patrolMinX, transform.position.y, transform.position.z), patrolSpeed * Time.deltaTime);

            // Chegou ao limite mínimo, inverte a direção
            if (transform.position.x <= patrolMinX)
            {
                movingToRight = true;
                Flip(true);
            }
        }
    }

    /////////////////////////PERSEGUIÇÃO E ATAQUE///////////////////////////////
    void ChasePlayer(float distanceToPlayer)
    {
        isChasing = true;

        if (distanceToPlayer > attackRange)
        {
            // Persegue o jogador
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * chaseSpeed * Time.deltaTime;

            // Atualiza a direção do Araki
            Flip(direction.x > 0);
        }
        else
        {
            // Está no alcance de ataque
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        // Verifica o cooldown de ataque
        if (Time.time - lastAttackTime < attackCooldown) return;

        // Armazena a última posição do jogador no momento do ataque
        lastPlayerPosition = player.position;

        // Dispara a animação de ataque
        animator.SetTrigger("Attack");

        // Instancia a pedra e direciona-a para a última posição do jogador
        Invoke("ThrowRock", 0.5f); // Lança a pedra com um pequeno delay para sincronizar com a animação

        lastAttackTime = Time.time; // Atualiza o tempo do último ataque
    }

    void ThrowRock()
    {
        GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
        rock.GetComponent<Rock>().Initialize(lastPlayerPosition);
    }

    void Flip(bool isMovingRight)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = isMovingRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }
}
