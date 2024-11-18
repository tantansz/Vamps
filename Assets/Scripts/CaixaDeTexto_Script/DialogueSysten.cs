using UnityEngine;
using TMPro;  // Importar o TextMeshPro para usar TextMeshProUGUI
using UnityEngine.UI;
using System.Collections; // Adicionando esta linha para corrigir o erro do IEnumerator

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox;  // Caixa de di�logo
    public TextMeshProUGUI characterName;  // Nome do personagem (TextMeshPro)
    public TextMeshProUGUI dialogueText;  // Texto do di�logo (TextMeshPro)
    public Image characterIcon;  // �cone do personagem

    public string nameToShow;  // Nome do personagem
    [TextArea(3, 5)] public string[] dialogues;  // Array de sess�es de texto
    public Sprite iconToShow;  // �cone do personagem

    private int currentDialogueIndex = 0;  // �ndice da sess�o atual
    private bool isDialogueActive = false;  // Se o di�logo est� ativo
    private bool isTyping = false;  // Se o texto est� sendo digitado
    private bool skipTyping = false;  // Para pular a anima��o de digita��o

    private PlayerMovement playerMovement;  // Refer�ncia ao script de movimento do jogador

    public float typingSpeed = 0.05f;  // Velocidade de digita��o

    void Start()
    {
        // Inicializa a caixa de di�logo como invis�vel
        dialogueBox.SetActive(false);  // Caixa de di�logo invis�vel inicialmente

        // Procura o script de movimento do jogador
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDialogueActive)
        {
            Debug.Log("Jogador entrou no trigger!");  // Mensagem de depura��o
            StartDialogue();  // Come�a o di�logo quando o jogador entra no trigger
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.J))
        {
            if (!isTyping)
            {
                AdvanceDialogue();  // Avan�a para o pr�ximo di�logo quando pressionar J
            }
            else
            {
                skipTyping = true;  // Pula a digita��o do texto
            }
        }
    }

    private void StartDialogue()
    {
        // Configura o di�logo inicial
        isDialogueActive = true;
        dialogueBox.SetActive(true);  // Mostra a caixa de di�logo
        characterName.text = nameToShow;  // Exibe o nome do personagem
        characterIcon.sprite = iconToShow;  // Exibe o �cone do personagem

        // Exibe o primeiro di�logo
        currentDialogueIndex = 0;
        StartCoroutine(TypeText(dialogues[currentDialogueIndex]));  // Exibe o texto com efeito de digita��o

        // Desativa o movimento do jogador
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    private void AdvanceDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Length)
        {
            // Exibe o pr�ximo di�logo
            StartCoroutine(TypeText(dialogues[currentDialogueIndex]));  // Exibe o pr�ximo texto com digita��o
        }
        else
        {
            // Finaliza o di�logo
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBox.SetActive(false);  // Esconde a caixa de di�logo

        // Reativa o movimento do jogador
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";  // Limpa o texto anterior

        foreach (char c in text)
        {
            if (skipTyping)
            {
                dialogueText.text = text;  // Pula a digita��o e mostra todo o texto de uma vez
                break;
            }

            dialogueText.text += c;  // Adiciona uma letra por vez
            yield return new WaitForSeconds(typingSpeed);  // Atraso entre as letras
        }

        isTyping = false;
        skipTyping = false;  // Reseta o skip
    }
}
