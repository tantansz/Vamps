using System.Collections; // Necessário para usar IEnumerator

using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    public Image barraCheia; // Referência para a imagem cheia
    public Image barraVazia; // Opcional, para controle visual

    public void ConfigurarVidaMaxima(float vidaMaxima)
    {
        AtualizarBarra(vidaMaxima, vidaMaxima); // Inicializa como cheia
    }

    public void AtualizarBarra(float vidaAtual, float vidaMaxima)
    {
        float porcentagem = vidaAtual / vidaMaxima; // Calcula a porcentagem
        barraCheia.fillAmount = porcentagem; // Atualiza o preenchimento
    }

    public IEnumerator FeedbackDano()
    {
        // Pequeno efeito para destacar dano recebido
        barraCheia.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        barraCheia.color = Color.white;
    }
}
