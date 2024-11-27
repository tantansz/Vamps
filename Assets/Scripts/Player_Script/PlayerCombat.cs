using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
     public GameObject gemSprite;
    public GameObject fireballPrefab; // Prefab da bola de fogo
    public Transform projectileSpawnPoint; // Ponto de spawn do projétil
    public Animator animator;

    public bool isFrozen = false; // Controle do estado de congelamento
    private int currentState = 1; // 1: Machado, 2: Comida, 3: Arma

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    private Rigidbody2D rb; // Referência ao Rigidbody para controle de movimento
   
    

    

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleStateChange(); // Troca de estados
        HandleInput();       // Lida com entrada do jogador
        HandleAttack();      // Ataca com o machado
    }

    /// <summary>
    /// Lida com a entrada de teclas e ativa os estados correspondentes.
    /// </summary>
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleFreezeState();
        }

        if (isFrozen && Input.GetMouseButtonDown(1)) // Botão direito do mouse
        {
            ShootMagicProjectile();
        }
    }

    /// <summary>
    /// Alterna o estado congelado do jogador (ativa/desativa).
    /// </summary>
void ToggleFreezeState()
{
    isFrozen = !isFrozen;

    if (isFrozen)
    {
        // Força a animação "Player_hold_gun" sem depender do movimento
        animator.SetInteger("Transition", 8);
        animator.Play("Player_hold_gun"); // Força a animação "hold gun"
        
        gemSprite.SetActive(true); // Ativa a gema

        GetComponent<playerMove>().enabled = false; // Desativa o movimento
        rb.velocity = Vector2.zero; // Para o movimento
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // Congela o Rigidbody
    }
    else
    {
        // Sai do estado congelado
        animator.SetInteger("Transition", 0); // Volta ao estado de "Idle"
        animator.Play("Base"); // Reverte para o estado de "Idle"
        
        gemSprite.SetActive(false); // Desativa a gema

        GetComponent<playerMove>().enabled = true; // Reativa o movimento
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Libera o Rigidbody
    }
}


    /// <summary>
    /// Sai do estado congelado.
    /// </summary>
    void ExitFrozenState()
    {
        isFrozen = false;
        animator.SetInteger("Transition", 0); // Volta ao estado Idle
        GetComponent<playerMove>().enabled = true; // Reativa o movimento
    }

    /// <summary>
    /// Atira o projétil de magia na direção do mouse.
    /// </summary>
 void ShootMagicProjectile()
{
    

    if (fireballPrefab != null && projectileSpawnPoint != null)
    {
        GameObject fireball = Instantiate(fireballPrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Calcula a direção do mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2)(mousePosition - projectileSpawnPoint.position)).normalized;

        // Adiciona movimento ao projétil
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
        if (fireballRb != null)
        {
            fireballRb.velocity = direction * 10f; // Velocidade do projétil
        }

        // Ajusta a rotação do sprite para a direção do movimento
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Ignora colisões com o jogador
        Physics2D.IgnoreCollision(fireball.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
    else
    {
        Debug.LogWarning("FireballPrefab ou ProjectileSpawnPoint não está configurado!");
    }
}


    /// <summary>
    /// Lida com a troca de estados entre machado, comida e arma.
    /// </summary>
    void HandleStateChange()
{
    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
    {
        // Sai do estado congelado
        if (isFrozen)
        {
            isFrozen = false;
            animator.SetInteger("Transition", 0); // Volta ao estado padrão
            GetComponent<playerMove>().enabled = true; // Reativa o movimento
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Libera o Rigidbody
        }

        // Troca o estado (machado ou comida)
        currentState = Input.GetKeyDown(KeyCode.Alpha1) ? 1 : 2;
    }
}


    /// <summary>
    /// Lida com o ataque do jogador no estado do machado.
    /// </summary>
    void HandleAttack()
    {
        if (currentState != 1) return; // Bloqueia ataque para outros estados

        // Controle de tempo do ataque
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0)) // Clique esquerdo
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    /// <summary>
    /// Realiza o ataque corpo a corpo com o machado.
    /// </summary>
    void Attack()
    {
        animator.SetTrigger("Attack"); // Executa animação de ataque

        // Detecta inimigos no raio de ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, knockbackDirection);
        }
    }

    /// <summary>
    /// Desenha o raio de ataque no editor para visualização.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

        public bool IsFrozen() { return isFrozen; } // Getter para acessar o valor

}
