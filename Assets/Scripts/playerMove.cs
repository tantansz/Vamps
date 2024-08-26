using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    #region Variaveis
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;
    public float jumpCutMultiplier = 0.5f;

    float horizontalMove = 0f;
    bool jump = false;
    bool isJumping = false;
    
    #endregion

    #region Update
                    //onde a gente vai chamar as funções do personagem.

    void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        HandleJumpCutoff();
        UpdateAnimations();
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
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
    }

    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
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
    #endregion
}