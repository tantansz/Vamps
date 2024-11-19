using UnityEngine;
using TMPro;  // Importar o TextMeshPro para usar TextMeshProUGUI
using UnityEngine.UI;
using System.Collections; // Adicionando esta linha para corrigir o erro do IEnumerator

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox;  // Caixa de diálogo
    public TextMeshProUGUI characterName;  // Nome do personagem (TextMeshPro)
    public TextMeshProUGUI dialogueText;  // Texto do diálogo (TextMeshPro)
    public Image characterIcon;  // Ícone do personagem

    public string nameToShow;  // Nome do personagem
    [TextArea(3, 5)] public string[] dialogues;  // Array de sessões de texto
    public Sprite iconToShow;  // Ícone do personagem

    private int currentDialogueIndex = 0;  // Índice da sessão atual
    private bool isDialogueActive = false;  // Se o diálogo está ativo
    private bool isTyping = false;  // Se o texto está sendo digitado
    private bool skipTyping = false;  // Para pular a animação de digitação

    private PlayerMovement playerMovement;  // Referência ao script de movimento do jogador

    public float typingSpeed = 0.05f;  // Velocidade de digitação

    void Start()
    {
        // Inicializa a caixa de diálogo como invisível
        dialogueBox.SetActive(false);  // Caixa de diálogo invisível inicialmente

        // Procura o script de movimento do jogador
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDialogueActive)
        {
            Debug.Log("Jogador entrou no trigger!");  // Mensagem de depuração
            StartDialogue();  // Começa o diálogo quando o jogador entra no trigger
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.J))
        {
            if (!isTyping)
            {
                AdvanceDialogue();  // Avança para o próximo diálogo quando pressionar J
            }
            else
            {
                skipTyping = true;  // Pula a digitação do texto
            }
        }
    }

    private void StartDialogue()
    {
        // Configura o diálogo inicial
        isDialogueActive = true;
        dialogueBox.SetActive(true);  // Mostra a caixa de diálogo
        characterName.text = nameToShow;  // Exibe o nome do personagem
        characterIcon.sprite = iconToShow;  // Exibe o ícone do personagem

        // Exibe o primeiro diálogo
        currentDialogueIndex = 0;
        StartCoroutine(TypeText(dialogues[currentDialogueIndex]));  // Exibe o texto com efeito de digitação

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
            // Exibe o próximo diálogo
            StartCoroutine(TypeText(dialogues[currentDialogueIndex]));  // Exibe o próximo texto com digitação
        }
        else
        {
            // Finaliza o diálogo
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBox.SetActive(false);  // Esconde a caixa de diálogo

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
                dialogueText.text = text;  // Pula a digitação e mostra todo o texto de uma vez
                break;
            }

            dialogueText.text += c;  // Adiciona uma letra por vez
            yield return new WaitForSeconds(typingSpeed);  // Atraso entre as letras
        }

        isTyping = false;
        skipTyping = false;  // Reseta o skip
    }
}
