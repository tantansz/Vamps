using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : MonoBehaviour
{
    public Transform player;  
    public Animator animator; 

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; 
    }

    void Update()
    {
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
            }
        }
        else
        {
            velocity.x = -speed;

            // Inverte direção ao alcançar o limite
            if (transform.position.x <= patrolMinX)
            {
                movingToRight = true;
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
    }
    ///////////////////////////////////////////////////////////////////////////

    /////////////////////////PERSEGUIÇÃO E RETORNO CASO NÃO VEJA MAIS O PLAYER///////////////////////////
    void ChasePlayer()
    {
        isChasing = true;

        // Move o morcego em direção ao player
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void ReturnToPatrol()
    {
        // Calcula o ponto central da área de patrulha
        Vector3 patrolCenter = new Vector3((patrolMinX + patrolMaxX) / 2f, (patrolMinY + patrolMaxY) / 2f, transform.position.z);

        // Move o morcego suavemente em direção ao centro da patrulha
        Vector3 direction = (patrolCenter - transform.position).normalized;
        rb.velocity = direction * returnSpeed;

        // Se o morcego estiver próximo o suficiente do centro da patrulha, ele retoma a patrulha normal
        if (Vector3.Distance(transform.position, patrolCenter) < 0.2f)
        {
            isChasing = false; // Sai do estado de perseguição

            // Reinicia o movimento de patrulha
            movingToRight = patrolCenter.x < (patrolMinX + patrolMaxX) / 2f;
            movingUp = patrolCenter.y < (patrolMinY + patrolMaxY) / 2f;
        }
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
}
