using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    [Header("Components")]
    public GameObject dialogueObj;
    public Image profile;
    public Text speechText;
    public Text actorNameText;
    public CutsceneManager cutsceneManager; // Refer�ncia ao CutsceneManager

    [Header("Settings")]
    public float typingSpeed;
    private string[] sentences;
    private int index;

    private Coroutine typingCoroutine;

    public bool isCutsceneDialogue = false; // Indica se o di�logo faz parte de uma cutscene

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

        // Avise ao CutsceneManager apenas se for um di�logo de cutscene
        if (isCutsceneDialogue && cutsceneManager != null)
        {
            cutsceneManager.NextDialogue();
        }
        var playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}
