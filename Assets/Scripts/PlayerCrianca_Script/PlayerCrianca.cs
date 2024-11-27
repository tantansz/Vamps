using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float jumpCutMultiplier = 0.5f; // Reduz a for�a do pulo ao soltar o bot�o
    public LayerMask groundLayer; // Camada do ch�o

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isJumping;

    public Transform groundCheck; // Ponto para verificar o ch�o
    public float groundCheckRadius = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movimento horizontal
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Virar o sprite
        if (moveInput > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = false;
        }

        // Verificar se est� no ch�o
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // In�cio do pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
        }

        // Soltar o bot�o de pulo
        if (Input.GetButtonUp("Jump") && isJumping)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
            }
            isJumping = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualizar o groundCheck no Editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
