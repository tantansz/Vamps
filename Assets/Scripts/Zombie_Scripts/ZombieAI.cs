using System.Collections;
using UnityEngine;

public class ZombieAI : MonsterAttack
{
    public Transform player;
    public float chaseRange = 3f;
    public float attackZombieRange = 1f;  // Dist�ncia para o ataque (por exemplo, a dist�ncia de colis�o do ataque)
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    private bool isZombieAttacking = false; // Flag para garantir que o zumbi s� ataque uma vez de cada vez

    protected override void Start()
    {
        base.Start();  // Chama o Start do base (MonsterAttack)
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        // Checa a dist�ncia para o jogador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Verifica se o jogador est� no alcance de persegui��o
        if (distanceToPlayer <= chaseRange && !isKnockedBack && !isZombieAttacking)
        {
            // Se o jogador estiver dentro do range de ataque, ele vai atacar
            if (distanceToPlayer <= attackZombieRange)
            {
                rb.velocity = Vector2.zero;  // Para de se mover enquanto ataca
                PerformAttack();
            }
            else
            {
                // Se o jogador n�o estiver no alcance de ataque, o zumbi persegue
                ChasePlayer();
            }
        }
        // Caso o jogador saia do alcance do zumbi
        else if (distanceToPlayer > chaseRange && !isKnockedBack && !isZombieAttacking)
        {
            // Caso o zumbi n�o esteja atacando nem sendo empurrado, ele pode se mover para perseguir o jogador
            ChasePlayer();
        }
    }

    // Fun��o para lidar com knockback
    public void Knockback(Vector2 direction)
    {
        if (isKnockedBack)
            return;

        isKnockedBack = true;
        rb.AddForce(direction * 10f, ForceMode2D.Impulse);  // Aplica o knockback

        // Cancela o ataque se o monstro for atingido
        StopAllCoroutines();
        isZombieAttacking = false;

        // Reseta a condi��o de knockback ap�s algum tempo
        Invoke("ResetKnockback", 0.5f);
    }

    private void ResetKnockback()
    {
        isKnockedBack = false;
    }

    // M�todo para perseguir o jogador
    private void ChasePlayer()
    {
        if (isZombieAttacking)
            return;  // N�o faz movimento se estiver atacando

        // Zumbi persegue o jogador (movimento)
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * 2f, rb.velocity.y);  // Movimenta o zumbi na dire��o do jogador
    }

    // Sobrescreve o m�todo de ataque para garantir que o zumbi pare enquanto ataca
    public override void PerformAttack()
    {
        if (isZombieAttacking)
            return; // N�o ataca se j� estiver atacando

        isZombieAttacking = true; // Marca que o zumbi est� atacando
        animator.SetTrigger("Attack"); // Aciona a anima��o de ataque

        // Cancela o movimento do zumbi enquanto ataca
        rb.velocity = Vector2.zero;

        StartCoroutine(AttackRoutine()); // Inicia a rotina do ataque
    }

    private IEnumerator AttackRoutine()
    {
        // Atraso para simular o tempo que o ataque leva para acontecer
        yield return new WaitForSeconds(attackDelay);

        // Detecta o jogador e aplica dano
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackZombieRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            ControlaVida playerHealth = player.GetComponent<ControlaVida>();
            if (playerHealth != null)
            {
                playerHealth.TomarDano(attackDamage); // Aplica dano ao jogador
            }
        }

        // Ap�s o ataque, reseta a flag e permite novos movimentos
        isZombieAttacking = false;
    }
}
