using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 1f;
    public int attackDamage = 10;
    public LayerMask playerLayer;

    public float attackDelay = 1f;

    protected bool isAttacking = false;

    protected virtual void Start ()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update ()
    {
        //da pra adicionar coisa se a gente quiser
    }

    public virtual void PerformAttack()
    {
        if (isAttacking)
            return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelay);

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach(Collider2D player in hitPlayers)
        {
            ControlaVida playerHealth = player.GetComponent<ControlaVida> ();
            if(playerHealth != null)
            {
                playerHealth.TomarDano(attackDamage);
            }
        }
        isAttacking = false;
    }
}
