using UnityEngine;

public class RainTrigger : MonoBehaviour
{
    public ParticleSystem rainEffect; // Sistema de partículas de chuva
    public ParticleSystem rainEffect2; // Sistema de partículas adicional
    public ParticleSystem rainEffect3; // Outro sistema de partículas
    public AudioSource rainSound; // Som da chuva
    public GameObject rainFilter; // Filtro visual de chuva (opcional)
    public AudioSource backGroundSound;

    public LayerMask playerLayer; // Layer do jogador
    public float radius; // Raio de detecção
    public float fadeDuration = 2f;
    public CanvasGroup canvasGroup;

    private bool rainActivated = false; // Controle para ativar a chuva uma única vez

    private void FixedUpdate()
    {
        DetectPlayer();
    }

    public void Start()
    {
        rainEffect.Stop();
        rainEffect2.Stop();
        rainEffect3.Stop();
    }

    private void DetectPlayer()
    {
        // Detecta o jogador dentro do raio
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, playerLayer);

        // Ativa a chuva apenas uma vez quando o jogador entra no raio
        if (hit != null && !rainActivated)
        {
            ActivateRain();
        }
    }

    private void ActivateRain()
    {
        rainActivated = true;

        // Ativa os sistemas de partículas de chuva
        if (rainEffect != null)
        {
            rainEffect.Play();
            Debug.Log("RainEffect ativado!");
        }
        else
        {
            Debug.LogWarning("RainEffect não atribuído no Inspector!");
        }

        if (rainEffect2 != null)
        {
            rainEffect2.Play();
            Debug.Log("RainEffect2 ativado!");
        }
        else
        {
            Debug.LogWarning("RainEffect2 não atribuído no Inspector!");
        }

        if (rainEffect3 != null)
        {
            rainEffect3.Play();
            Debug.Log("RainEffect3 ativado!");
        }
        else
        {
            Debug.LogWarning("RainEffect3 não atribuído no Inspector!");
        }

        // Ativa o som de chuva
        if (rainSound != null)
        {
            backGroundSound.Stop();
            rainSound.Play();
            Debug.Log("RainSound ativado!");
        }
        else
        {
            Debug.LogWarning("RainSound não atribuído no Inspector!");
        }

        // Ativa o filtro visual
        if (rainFilter != null && canvasGroup != null)
        {
            rainFilter.SetActive(true);
            Debug.Log("RainFilter ativado!");

            canvasGroup.alpha = 0;

            StartCoroutine(FadeIn());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    private void OnDrawGizmosSelected()
    {
        // Mostra o raio de detecção no editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
