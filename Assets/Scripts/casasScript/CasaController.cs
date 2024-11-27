using UnityEngine;

public class CasaController : MonoBehaviour
{
    public GameObject interiorCasa; // O sprite do interior da casa
    public Transform pontoSaidaCasa; // O ponto fora da casa para onde o jogador ser� teleportado
    public GameObject objetoSaidaDentroCasa; // O objeto dentro da casa que o jogador deve interagir para sair

    private bool dentroDaCasa = false;

    void Update()
    {
        // Verifica se o jogador pressionou 'E' para interagir
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dentroDaCasa)
            {
                // Verifica se o jogador est� perto do objeto de sa�da dentro da casa
                if (objetoSaidaDentroCasa.activeSelf)
                {
                    SairDaCasa(); // Jogador vai sair da casa ao interagir com o objeto dentro da casa
                }
            }
            else
            {
                EntrarNaCasa(); // Jogador vai entrar na casa
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jogador"))
        {
            dentroDaCasa = true; // Jogador entrou na �rea da porta
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jogador"))
        {
            dentroDaCasa = false; // Jogador saiu da �rea da porta
        }
    }

    // Fun��o para o jogador entrar na casa
    void EntrarNaCasa()
    {
        interiorCasa.SetActive(true); // Ativa o interior da casa
        objetoSaidaDentroCasa.SetActive(true); // Ativa o objeto de sa�da dentro da casa (para que o jogador possa interagir com ele)
    }

    // Fun��o para o jogador sair da casa
    void SairDaCasa()
    {
        interiorCasa.SetActive(false); // Desativa o interior da casa

        // Teleporta o jogador para fora da casa
        GameObject jogador = GameObject.FindGameObjectWithTag("Jogador");
        if (jogador != null && pontoSaidaCasa != null)
        {
            // Teleporta o jogador para o ponto fora da casa
            jogador.transform.position = pontoSaidaCasa.position;
        }

        // Desativa o objeto de sa�da dentro da casa
        objetoSaidaDentroCasa.SetActive(false);
    }
}
