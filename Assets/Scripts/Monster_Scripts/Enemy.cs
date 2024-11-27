using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Animator animator; //referencia de anima��o
    public float knockbackForce = 5f; //For�a de knockback aplicada ao monstro
    private Rigidbody2D rb;
    [SerializeField] private AudioClip[] damageSoundClips;

    public float stunDuration = 0.5f;
    public bool isStunned = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>(); //pega o Rigidbody2D pra conseguir aplicar o knockback
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;        //Rezuz a vida do monstro
        Debug.Log("Enemy took damage!");

        //animator.SetTrigger("TakeDamage");

        SFXManager.instance.PlayRandomSFXClip(damageSoundClips, transform, 1f);

        //Vector2 knockbackDirection = (transform.position - attackerPosition.position).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); //Aplica o knockback 
        Debug.Log("KnockbackOn");

        StartCoroutine(ApplyStun());

        if (!isStunned)
        {
            StartCoroutine(Stun());
        }

        if (currentHealth <= 0)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = true;  // Torna o Rigidbody2D kinem�tico, ou seja, ele n�o � mais afetado pela f�sica
            }
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;  // Desativa o collider
            }
            Die();
        }
    } //attackerPosition

    private IEnumerator ApplyStun()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    IEnumerator Stun()
    {
        isStunned = true;

        rb.velocity = Vector2.zero;
        rb.isKinematic = true; //Evita a movimenta��o

        yield return new WaitForSeconds(stunDuration);

        rb.isKinematic = true;
        isStunned = false;
    }

    void Die()
    {
        //animator.SetTrigger("Die");
        Debug.Log("Enemy died!");
        Destroy(gameObject);//Retirar esse destroy depois, coloquei como teste aqui.
        //Invoke("DestroyEnemy", 2f);
    }

    void DestroyOnAnimationEnd()
    {
        Destroy(gameObject);
    }
}///Tantan, comentei todas as animações para poder verificar se era isso que estava impedindo de usar o script no bat
///ent precisa descomentar depois, mas aparentimente esta funcionando o dano no morcego agora.
