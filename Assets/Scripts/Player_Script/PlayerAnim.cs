using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private PlayerCombat playerCombat; // Referência para PlayerCombat
    private playerMove playerMove;
    private CharacterController2D characterController;
    private Animator animator;

    private bool isJumping = false;
    private bool isLanding = false;
    private float jumpStartTime;
    private float jumpTransitionDelay = 0.2f;

    void Start()
    {
        playerMove = GetComponent<playerMove>();
        characterController = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        playerCombat = GetComponent<PlayerCombat>(); // Obtém a referência para PlayerCombat
    }

    void Update()
    {
        // Verifica se o jogador está congelado antes de definir a animação
        if (playerCombat != null && playerCombat.isFrozen) // Acessa a variável 'isFrozen' da instância de PlayerCombat
        {
            animator.SetInteger("Transition", 8); // Player_hold_gun
        }
        else if (playerMove.isDashing)
        {
            animator.SetInteger("Transition", 3); // Dash
        }
        else if (!characterController.isGrounded() && !isJumping && playerMove.isJumping)
        {
            animator.SetInteger("Transition", 5); // Start jump
            isJumping = true;
            jumpStartTime = Time.time;
        }
        else if (isJumping && !characterController.isGrounded() && (Time.time - jumpStartTime) > jumpTransitionDelay)
        {
            animator.SetInteger("Transition", 6); // Idle no ar
        }
        else if (characterController.isGrounded() && isJumping)
        {
            animator.SetInteger("Transition", 7); // Pouso
            isLanding = true;
            isJumping = false;
        }
        else if (playerMove.isSprinting && Mathf.Abs(playerMove.horizontalMove) > 0)
        {
            animator.SetInteger("Transition", 2); // Run
        }
        else if (Mathf.Abs(playerMove.horizontalMove) > 0)
        {
            animator.SetInteger("Transition", 1); // Walk
        }
        else if (characterController.isGrounded() && !isLanding)
        {
            animator.SetInteger("Transition", 0); // Idle
        }

        if (characterController.isGrounded() && isLanding)
        {
            isLanding = false;
        }
    }
}