using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private playerMove playerMove;
    private CharacterController2D characterController;
    private Animator animator;

    void Start()
    {
        playerMove = GetComponent<playerMove>();
        characterController = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Mathf.Abs(playerMove._horizontalMove) > 0) // Se o personagem está se movendo horizontalmente
        {
            animator.SetInteger("Transition", 1); // Definindo o estado de "walk" (andar)
        }
        else
        {
            animator.SetInteger("Transition", 0); // Definindo o estado de "idle" (parado)
        }

        if (!characterController.isGrounded())
        {
            animator.SetInteger("Transition", 3); // Definindo o estado de "jump" (pulo)
        }
    }
}
