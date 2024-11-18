using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

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
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
       
        animator.SetTrigger("Attack");

        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        
        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;

            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, knockbackDirection);
        }
    }

    
}