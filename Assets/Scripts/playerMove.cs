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

    float horizontalMove = 0f;
    bool jump = false;
    bool isJumping = false;
    bool isSprinting = false; // Flag para verificar se o jogador está correndo
    bool isDashing = false; // Flag para verificar se o jogador está dando um dash
    float dashTime = 0f; // Tempo restante do dash

    float coyoteTime = 2f; //tanto de tempo permitido ao jogador pular depois de cair da plataforma
    float coyoteTimeCounter;

    float jumpBufferTimer = 0.5f; //tanto de tempo permitido ao jogador pular depois de pressionar o pulo
    float jumpBufferCounter; //OBS: a ser adicionado
    #endregion

    #region Update
    void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        HandleJumpCutoff();
        HandleDash(); //entrada do dash
        UpdateAnimations();
        ApplyCoyoteTime();
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
        //se a tecla Shift está pressionada, ativar o estado de corrida
        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // se o Control está pressionado da o dash
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            isDashing = true;
            dashTime = dashDuration; // Define o tempo do dash
        }

        // Define a velocidade de movimento com base no estado (normal, corrida rápida ou dash)
        float currentSpeed = isDashing ? dashSpeed : (isSprinting ? runSpeed * sprintMultiplier : runSpeed);

        horizontalMove = Input.GetAxisRaw("Horizontal") * currentSpeed;
    }

    void HandleJumpInput()
    {
        if (coyoteTimeCounter > 0f && Input.GetButtonDown("Jump"))
        {
            jump = true;
            isJumping = true;
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
        //a ser adicionado
    }
    #endregion
}
