using UnityEngine;

public class ControlaVida : MonoBehaviour
{
    public BarraDeVida barraDeVida;
    public float vidaMaxima = 100;
    private float vidaAtual;

    void Start()
    {
        vidaAtual = vidaMaxima;
        barraDeVida.ConfigurarVidaMaxima(vidaMaxima);
    }

    public void TomarDano(float dano)
    {
        vidaAtual -= dano;
        if (vidaAtual < 0) vidaAtual = 0;

        barraDeVida.AtualizarBarra(vidaAtual, vidaMaxima);
        StartCoroutine(barraDeVida.FeedbackDano());

        if (vidaAtual <= 0)
            Morrer();
    }

    public void RecuperarVida(float quantidade)
    {
        vidaAtual += quantidade;
        if (vidaAtual > vidaMaxima) vidaAtual = vidaMaxima;

        barraDeVida.AtualizarBarra(vidaAtual, vidaMaxima);
    }

    private void Morrer()
    {
        Debug.Log("Player morreu!");
        Destroy(gameObject); // Substitua por sua lógica de morte
    }
}
