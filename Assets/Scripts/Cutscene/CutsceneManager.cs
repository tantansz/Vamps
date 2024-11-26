using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public DialogueControl dialogueControl; // Controle de di�logos
    public GameObject player; // Refer�ncia ao jogador para desativar o movimento
    public AudioSource backgroundMusic;
    public AudioClip cutsceneMusic;
    public float transitionDelay = 2f; // Tempo antes de carregar a pr�xima cena

    [System.Serializable]
    public class CutsceneDialogue
    {
        public Sprite profile;
        [TextArea(3, 5)] public string[] speechTxt;
        public string actorName;
    }

    public CutsceneDialogue[] cutsceneDialogues; // Lista de di�logos da cutscene
    private int currentDialogueIndex = 0;

    private bool isDialogueActive = false; // Indica se o di�logo est� ativo

    public void StartCutscene()
    {
        // Desativa o movimento do jogador
        if (player != null)
        {
            var playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
                playerMovement.enabled = false;
        }

        // Trocar a m�sica de fundo
        ChangeBackgroundMusic();

        // Iniciar o primeiro di�logo
        PlayDialogue();
    }

    private void PlayDialogue()
    {
        if (currentDialogueIndex < cutsceneDialogues.Length)
        {
            isDialogueActive = true;

            // Configurar o di�logo atual
            var dialogue = cutsceneDialogues[currentDialogueIndex];
            dialogueControl.isCutsceneDialogue = true; // Indica que � um di�logo de cutscene
            dialogueControl.Speech(dialogue.profile, dialogue.speechTxt, dialogue.actorName);
        }
        else
        {
            EndCutscene();
        }
    }

    public void NextDialogue()
    {
        currentDialogueIndex++;
        PlayDialogue();
    }

    private void EndCutscene()
    {
        // Reativa o movimento do jogador
        if (player != null)
        {
            var playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
                playerMovement.enabled = true;
        }

        // Troca para a pr�xima cena
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene("Treino Plataforma"); // Ajuste para o nome da sua pr�xima cena
    }

    private void ChangeBackgroundMusic()
    {
        if (backgroundMusic != null && cutsceneMusic != null)
        {
            backgroundMusic.Stop();
            backgroundMusic.clip = cutsceneMusic;
            backgroundMusic.Play();
        }
    }
}
