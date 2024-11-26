using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public Sprite profile;
    public string[] speechTxt;
    public string actorName;

    public LayerMask playerLayer;
    public float radious;
    public bool isMandatory;

    private DialogueControl dc;
    private bool onRadious;
    private bool dialogueStarted;

    private void Start()
    {
        dc = FindObjectOfType<DialogueControl>();
    }

    private void FixedUpdate()
    {
        Interact();
    }

    private void Update()
    {
        if (!isMandatory && onRadious && Input.GetKeyDown(KeyCode.Space))
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        dialogueStarted = true;
        dc.Speech(profile, speechTxt, actorName);

        // Desativar movimentação do jogador
        var playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    public void Interact()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radious, playerLayer);

        if (hit != null)
        {
            onRadious = true;

            // Se for obrigatório, iniciar diálogo automaticamente
            if (isMandatory && !dialogueStarted)
            {
                StartDialogue();
            }
        }
        else
        {
            onRadious = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radious);
    }
}
