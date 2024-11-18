using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox; // Caixa de di�logo
    public Text characterName;     // Nome do personagem
    public Text dialogueText;      // Texto do di�logo
    public Image characterIcon;    // �cone do personagem

    public string nameToShow;      // Nome do personagem
    [TextArea(3, 5)] public string[] dialogues; // Array de sess�es de texto
    public Sprite iconToShow;      // �cone do personagem

    private int currentDialogueIndex = 0; // �ndice da sess�o atual
    private bool isDialogueActive = false; // Se o di�logo est� ativo

    private PlayerMovement playerMovement; // Refer�ncia ao script de movimento do jogador

    void Start()
    {
        // Inicializa a caixa de di�logo como invis�vel
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
        // Configura o di�logo inicial
        isDialogueActive = true;
        dialogueBox.SetActive(true);

        characterName.text = nameToShow;
        characterIcon.sprite = iconToShow;

        // Exibe o primeiro di�logo
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
            // Exibe o pr�ximo di�logo
            dialogueText.text = dialogues[currentDialogueIndex];
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
        dialogueBox.SetActive(false);

        // Reativa o movimento do jogador
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}
