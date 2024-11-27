using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    [Header("Components")]
    public GameObject dialogueObj;
    public Image profile;
    public Text speechText;
    public Text actorNameText;

    [Header("Settings")]
    public float typingSpeed;
    private string[] sentences;
    private int index;

    private Coroutine typingCoroutine;

    public void Speech(Sprite p, string[] txt, string actorName)
    {
        dialogueObj.SetActive(true);
        profile.sprite = p;
        sentences = txt;
        actorNameText.text = actorName;
        index = 0;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        speechText.text = "";
        foreach (char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    public void NextSentence()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            speechText.text = sentences[index];
            typingCoroutine = null;
        }
        else if (index < sentences.Length - 1)
        {
            index++;
            speechText.text = "";
            typingCoroutine = StartCoroutine(TypeSentence());
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        speechText.text = "";
        index = 0;
        dialogueObj.SetActive(false);
        typingCoroutine = null;

        // Reativar movimentação do jogador
        var playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}
