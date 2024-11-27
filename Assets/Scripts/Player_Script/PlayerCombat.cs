using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private AudioClip[] attackSoundClips;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                SFXManager.instance.PlayRandomSFXClip(attackSoundClips, transform, 1f);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // Ativa a animação de ataque
        animator.SetTrigger("Attack");

        // Detecta inimigos no raio de ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Itera pelos inimigos atingidos
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Objeto detectado: {enemy.name}"); // Mensagem de debug para identificar o objeto atingido

            // Verifica se o inimigo possui o componente Enemy
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                // Calcula a direção do knockback e aplica o dano
                Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                enemyComponent.TakeDamage(attackDamage, knockbackDirection);
            }
            else
            {
                // Exibe um aviso caso o objeto não tenha o componente Enemy
                Debug.LogWarning($"O objeto {enemy.name} não possui o componente 'Enemy'. Certifique-se de que está configurado corretamente.");
            }
        }
    }

    // Método para desenhar o raio de ataque no editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
