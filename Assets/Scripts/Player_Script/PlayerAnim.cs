using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private playerMove playerMove;
    private CharacterController2D characterController;
    private Animator animator;

    private bool isJumping = false; // Controle para verificar se o pulo iniciou
    private bool isLanding = false; // Controle para verificar se está no pouso
    private float jumpStartTime; // Tempo inicial do pulo
    private float jumpTransitionDelay = 0.2f; // Duração do estado de "pulo inicial"

    void Start()
    {
        playerMove = GetComponent<playerMove>();
        characterController = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Dash
        if (playerMove.isDashing)
        {
            animator.SetInteger("Transition", 3); // Dash
        }
        // Start Jump
        else if (!characterController.isGrounded() && !isJumping && playerMove.isJumping)
        {
            animator.SetInteger("Transition", 5); // Start jump
            isJumping = true;
            jumpStartTime = Time.time; // Marca o tempo em que o pulo iniciou
        }
        // Airborne (Idle no ar) após o tempo de "start jump"
        else if (isJumping && !characterController.isGrounded() && (Time.time - jumpStartTime) > jumpTransitionDelay)
        {
            animator.SetInteger("Transition", 6); // Idle no ar
        }
        // Landing
        else if (characterController.isGrounded() && isJumping)
        {
            animator.SetInteger("Transition", 7); // Pouso
            isLanding = true;
            isJumping = false;
        }
        // Caminhada e Corrida
        else if (playerMove.isSprinting && Mathf.Abs(playerMove.horizontalMove) > 0)
        {
            animator.SetInteger("Transition", 2); // Run
        }
        else if (Mathf.Abs(playerMove.horizontalMove) > 0)
        {
            animator.SetInteger("Transition", 1); // Walk
        }
        // Idle
        else if (characterController.isGrounded() && !isLanding)
        {
            animator.SetInteger("Transition", 0); // Idle
        }

        // Reseta o estado de pouso após a animação de pouso
        if (characterController.isGrounded() && isLanding)
        {
            isLanding = false;
        }
    }
}
