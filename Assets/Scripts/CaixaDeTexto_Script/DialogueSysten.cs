using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox; // Caixa de diálogo
    public Text characterName;     // Nome do personagem
    public Text dialogueText;      // Texto do diálogo
    public Image characterIcon;    // Ícone do personagem

    public string nameToShow;      // Nome do personagem
    [TextArea(3, 5)] public string[] dialogues; // Array de sessões de texto
    public Sprite iconToShow;      // Ícone do personagem

    private int currentDialogueIndex = 0; // Índice da sessão atual
    private bool isDialogueActive = false; // Se o diálogo está ativo

    private PlayerMovement playerMovement; // Referência ao script de movimento do jogador

    void Start()
    {
        // Inicializa a caixa de diálogo como invisível
        dialogueBox.SetActive(false);

        // Procura o script de movimento do jogador
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDialogueActive)
        {
            StartDialogue();
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.J))
        {
            AdvanceDialogue();
        }
    }

    private void StartDialogue()
    {
        // Configura o diálogo inicial
        isDialogueActive = true;
        dialogueBox.SetActive(true);

        characterName.text = nameToShow;
        characterIcon.sprite = iconToShow;

        // Exibe o primeiro diálogo
        currentDialogueIndex = 0;
        dialogueText.text = dialogues[currentDialogueIndex];

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
            dialogueText.text = dialogues[currentDialogueIndex];
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
        dialogueBox.SetActive(false);

        // Reativa o movimento do jogador
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}
