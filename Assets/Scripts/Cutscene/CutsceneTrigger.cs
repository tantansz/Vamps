using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public LayerMask playerLayer; // Camada do jogador
    public float triggerRadius = 2f; // Raio de ativação
    private bool cutsceneActivated = false;

    private void FixedUpdate()
    {
        CheckPlayerInRange();
    }

    public void CheckPlayerInRange()
    {
        if (cutsceneActivated) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, triggerRadius, playerLayer);

        if (hit != null)
        {
            // Iniciar cutscene
            var cutsceneManager = GetComponent<CutsceneManager>();
            if (cutsceneManager != null)
            {
                cutsceneManager.StartCutscene();
                cutsceneActivated = true; // Garante que só ativa uma vez
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
