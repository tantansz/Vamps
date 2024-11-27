using UnityEngine;

public class ArakBehaviour : MonoBehaviour
{
    public float patrolMin; // Limite mínimo da patrulha
    public float patrolMax; // Limite máximo da patrulha
    public float patrolSpeed = 2f; // Velocidade durante a patrulha
    public float chaseSpeed = 4f; // Velocidade durante a perseguição
    public float detectionRange = 5f; // Alcance de detecção do jogador
    public Transform player; // Referência ao jogador
    public GameObject rockPrefab; // Prefab básico da pedra
    public float throwForce = 5f; // Força inicial do lançamento
    public float cooldownTime = 2f; // Tempo de recarga entre os ataques

    private bool movingRight = true; // Direção da patrulha
    private bool isChasing = false; // Estado atual
    private float lastAttackTime; // Tempo do último ataque
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

            if (Time.time >= lastAttackTime + cooldownTime)
            {
                LaunchRock(player.position);
                lastAttackTime = Time.time; // Atualiza o tempo do último ataque
            }
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

    void LaunchRock(Vector3 targetPosition)
    {
       // Cria a pedra como um prefab na posição do Arak
    GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
    Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();

    if (rockRb != null)
    {
        // Calcula a distância horizontal e vertical entre o Arak e o jogador
        Vector2 distance = targetPosition - transform.position;

        // Tempo ajustado dinamicamente baseado na distância horizontal
        float timeToTarget = Mathf.Max(Mathf.Abs(distance.x) / 5.0f, 0.5f); // Garante um tempo mínimo

        // Calcula as forças horizontal e vertical
        float horizontalForce = distance.x / timeToTarget; // Força proporcional à distância no eixo X
        float verticalForce = (distance.y / timeToTarget) + (0.5f * Mathf.Abs(Physics2D.gravity.y) * timeToTarget); // Força no eixo Y

        // Logs para depuração
        Debug.Log($"Distance X: {distance.x}, Distance Y: {distance.y}");
        Debug.Log($"Horizontal Force: {horizontalForce}");
        Debug.Log($"Vertical Force: {verticalForce}");

        // Aplica a força na pedra como velocidade inicial
        rockRb.velocity = new Vector2(horizontalForce, verticalForce);
    }
    else
    {
        Debug.LogError("Rigidbody2D is missing on the instantiated Rock prefab.");
    }

    }
    

    void Flip()
    {
        // Inverte a escala no eixo X
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
