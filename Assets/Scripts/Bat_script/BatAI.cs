using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : MonoBehaviour
{
    public Transform player;  // Referência ao player para o morcego perseguir
    public Animator animator; // Para as animações

    public float speed = 2f;  // Velocidade de movimento do morcego
    public float chaseRange = 5f;  // Distância máxima para perseguir o player
    public float returnSpeed = 1f;  // Velocidade de retorno ao ponto de patrulha quando o player escapar

    public float patrolMinX = -5f;  // Limite mínimo da patrulha
    public float patrolMaxX = 5f;   // Limite máximo da patrulha
    public float patrolMinY = 2f;   // Limite mínimo no eixo Y para simular voo
    public float patrolMaxY = 5f;   // Limite máximo no eixo Y para simular voo
    private bool movingToRight = true; // Direção inicial na patrulha
    private bool movingUp = true; // Controle do movimento vertical

    private Rigidbody2D rb;  // Referência ao Rigidbody2D do morcego
    private bool isChasing = false;  // Flag para verificar se o morcego está perseguindo o player
    private Vector3 initialScale; // Escala inicial para o Flip

    private bool isDead = false; // Verifica se o morcego está morto

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Impede que o morcego gire
        initialScale = transform.localScale; // Armazena a escala inicial
    }

    void Update()
    {
        if (isDead) return; // Impede que o morcego execute ações após a morte

        // Calcula a distância entre o morcego e o player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Se o player estiver dentro da distância de perseguição, o morcego começa a perseguir
        if (distanceToPlayer < chaseRange)
        {
            ChasePlayer();
        }
        // Se o morcego estava perseguindo, mas o player escapou, ele retorna à patrulha
        else if (isChasing)
        {
            ReturnToPatrol();
        }
        // Se o morcego não está perseguindo, ele patrulha entre os limites definidos
        else
        {
            Patrol();
        }

        // Corrige a velocidade para evitar que o morcego seja empurrado descontroladamente
        LimitVelocity();
    }

    ///////////////////////////PATRULHA//////////////////////////////////////////
    void Patrol()
    {
        Vector2 velocity = rb.velocity;

        // Movimento horizontal
        if (movingToRight)
        {
            velocity.x = speed;

            // Inverte direção ao alcançar o limite
            if (transform.position.x >= patrolMaxX)
            {
                movingToRight = false;
                Flip(false); // Vira para a esquerda
            }
        }
        else
        {
            velocity.x = -speed;

            // Inverte direção ao alcançar o limite
            if (transform.position.x <= patrolMinX)
            {
                movingToRight = true;
                Flip(true); // Vira para a direita
            }
        }

        // Movimento vertical
        if (movingUp)
        {
            velocity.y = speed;

            // Inverte direção ao alcançar o limite superior
            if (transform.position.y >= patrolMaxY)
            {
                movingUp = false;
            }
        }
        else
        {
            velocity.y = -speed;

            // Inverte direção ao alcançar o limite inferior
            if (transform.position.y <= patrolMinY)
            {
                movingUp = true;
            }
        }

        // Aplica a velocidade final ao Rigidbody
        rb.velocity = velocity;

        // Define a animação de idle para a patrulha
        if (animator != null)
        {
            animator.SetTrigger("Idle");
        }
    }
    ///////////////////////////////////////////////////////////////////////////

    /////////////////////////PERSEGUIÇÃO E RETORNO CASO NÃO VEJA MAIS O PLAYER///////////////////////////
    void ChasePlayer()
    {
        isChasing = true;

        // Move o morcego em direção ao player
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;

        // Ajusta a direção do morcego para o lado correto
        if ((direction.x > 0 && !movingToRight) || (direction.x < 0 && movingToRight))
        {
            Flip(direction.x > 0);
        }

        // Define a animação de corrida para a perseguição
        if (animator != null)
        {
            animator.SetTrigger("Run");
        }
    }

    void ReturnToPatrol()
    {
        // Calcula o ponto central da área de patrulha
        Vector3 patrolCenter = new Vector3((patrolMinX + patrolMaxX) / 2f, (patrolMinY + patrolMaxY) / 2f, transform.position.z);

        // Move o morcego suavemente em direção ao centro da patrulha
        Vector3 direction = (patrolCenter - transform.position).normalized;
        rb.velocity = direction * returnSpeed;

        // Ajusta a direção do morcego para o lado correto
        if ((direction.x > 0 && !movingToRight) || (direction.x < 0 && movingToRight))
        {
            Flip(direction.x > 0);
        }

        // Se o morcego estiver próximo o suficiente do centro da patrulha, ele retoma a patrulha normal
        if (Vector3.Distance(transform.position, patrolCenter) < 0.2f)
        {
            isChasing = false; // Sai do estado de perseguição

            // Reinicia o movimento de patrulha
            movingToRight = patrolCenter.x < (patrolMinX + patrolMaxX) / 2f;
            movingUp = patrolCenter.y < (patrolMinY + patrolMaxY) / 2f;

            // Reseta a velocidade para evitar resquícios da perseguição
            rb.velocity = Vector2.zero;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////FLIP PARA AJUSTAR DIREÇÃO////////////////////////////////
    void Flip(bool isMovingRight)
    {
        movingToRight = isMovingRight; // Define a direção horizontal

        // Inverte a escala no eixo X
        Vector3 localScale = transform.localScale;
        localScale.x = movingToRight ? Mathf.Abs(initialScale.x) : -Mathf.Abs(initialScale.x);
        transform.localScale = localScale;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////LIMITAR VELOCIDADE////////////////////////////////
    void LimitVelocity()
    {
        // Limita a velocidade do morcego para evitar movimentos descontrolados
        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////TOMAR DANO////////////////////////////////////////
    public void TakeDamage()
    {
        if (isDead) return; // Não toma dano se já estiver morto

        // Define a animação de tomar dano
        if (animator != null)
        {
            animator.SetTrigger("TakeDamage");
        }

        Debug.Log("Morcego tomou dano!");
    }
    ///////////////////////////////////////////////////////////////////////////

    /////////////////////////MORTE/////////////////////////////////////////////
    public void Die()
    {
        if (isDead) return; // Já está morto, não executa novamente

        isDead = true; // Marca o morcego como morto

        // Define a animação de morte
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Debug.Log("Morcego morreu!");

        // Destroi o objeto após um tempo para garantir que a animação termine
        Destroy(gameObject, 2f);
    }
    ///////////////////////////////////////////////////////////////////////////

    /////////////////////////ATAQUE/////////////////////////////////////////////
    public void Attack()
    {
        // Define a animação de ataque
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Debug.Log("Morcego atacou!");
    }
    ///////////////////////////////////////////////////////////////////////////
}
