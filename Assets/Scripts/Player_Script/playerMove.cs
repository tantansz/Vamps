using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
   #region Variaveis
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;
    public float sprintMultiplier = 1.5f; // Multiplicador de velocidade para correr ao pressionar Shift
    public float jumpCutMultiplier = 0.5f;

    public float dashSpeed = 80f; // Velocidade do dash
    public float dashDuration = 0.2f; // Duração do dash em segundos
    public float dashCooldown = 10f; // Cooldown para limitar o dash, para não fica um dash infinito

    public float horizontalMove;
    bool jump = false;
    public bool isJumping = false;
    public bool isSprinting = false; // Flag para verificar se o jogador está correndo
    public bool isDashing = false; // Flag para verificar se o jogador está dando um dash
    float dashTime = 0f; // Tempo restante do dash

    float coyoteTime = 0.1f; // Tanto de tempo permitido ao jogador pular depois de cair da plataforma
    float coyoteTimeCounter;

    float jumpBufferTimer = 0.05f; // Tanto de tempo permitido ao jogador pular depois de pressionar o pulo
    float jumpBufferCounter;

    bool hasAirDashed = false; // Flag para verificar se o dash no ar já foi utilizado
    float dashCooldownTimer = 0f; // Timer para controlar o cooldown do dash no chão
    #endregion

    #region Update
    void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        HandleJumpCutoff();
        HandleDash(); // Entrada do dash
        UpdateAnimations();
        ApplyCoyoteTime();
        ApplyJumpBuffer();

        // Atualiza o cooldown do dash no chão, decrementando a cada frame
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }
    #endregion

    #region FixedUpdate
    void FixedUpdate()
    {
        ApplyMovement();
    }
    #endregion

    #region Input Handling
    void HandleMovementInput()
    {
        // Se a tecla Shift está pressionada, ativar o estado de corrida
        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Se Control está pressionado, inicia o dash com base nas condições
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            // Dash no ar (somente uma vez)
            if (!controller.isGrounded() && !hasAirDashed && !isDashing)
            {
                StartDash(); 
                hasAirDashed = true; // Marca o dash no ar como usado
            }
            // Dash no chão com cooldown
            else if (controller.isGrounded() && dashCooldownTimer <= 0f && !isDashing)
            {
                StartDash(); 
                dashCooldownTimer = dashCooldown; // Reinicia o cooldown do dash no chão
            }
        }

        // Define a velocidade de movimento com base no estado (normal, corrida rápida ou dash)
        float currentSpeed = isDashing ? dashSpeed : (isSprinting ? runSpeed * sprintMultiplier : runSpeed);
        horizontalMove = Input.GetAxisRaw("Horizontal") * currentSpeed;
    }

    void HandleJumpInput()
    {
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            jump = true;
            isJumping = true;

            jumpBufferCounter = 0f;
        }
    }

    void HandleJumpCutoff()
    {
        if (Input.GetButtonUp("Jump") && isJumping)
        {
            controller.ReduceJumpHeight(jumpCutMultiplier);
            isJumping = false;

            coyoteTimeCounter = 0f;
        }
    }

    void HandleDash()
    {
        // Se o jogador está dando um dash, decrementa o tempo do dash
        if (isDashing)
        {
            dashTime -= Time.deltaTime; // Decrementa o tempo restante do dash

            // Quando o tempo do dash termina, para o dash
            if (dashTime <= 0f)
            {
                isDashing = false; // Reseta o estado de dash
            }
        }

        // Reseta a habilidade de dash no ar quando o jogador toca o chão
        if (controller.isGrounded())
        {
            hasAirDashed = false; // Permite que o jogador faça um novo dash no ar ao pular novamente
        }
    }

    void StartDash()
    {
        isDashing = true; // Ativa o estado de dash
        dashTime = dashDuration; // Define o tempo de duração do dash
    }
    #endregion

    #region Animation
    void UpdateAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }
    #endregion

    #region Movement
    void ApplyMovement()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    void ApplyCoyoteTime()
    {
        if (controller.isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    
    void ApplyJumpBuffer()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTimer;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
    #endregion
}
