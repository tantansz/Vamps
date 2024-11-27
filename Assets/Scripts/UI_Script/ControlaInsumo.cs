using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlaInsumo : MonoBehaviour
{
    public Image machado; // Referência para a imagem do machado
    public Image comida;  // Referência para a imagem da comida
    public Image arma;    // Referência para a imagem da arma

    void Start()
    {
        // Certifique-se de que apenas o machado está ativo inicialmente
        AtualizarInsumo("machado");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Tecla 2 para comida
        {
            AtualizarInsumo("comida");
        }
        else if (Input.GetKeyDown(KeyCode.J)) // Tecla J para arma
        {
            AtualizarInsumo("arma");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) // Tecla I para estado idle (machado)
        {
            AtualizarInsumo("machado");
        }
    }

    void AtualizarInsumo(string insumo)
    {
        // Desativa todos os ícones
        machado.gameObject.SetActive(false);
        comida.gameObject.SetActive(false);
        arma.gameObject.SetActive(false);

        // Ativa o ícone correspondente
        switch (insumo)
        {
            case "machado":
                machado.gameObject.SetActive(true);
                break;
            case "comida":
                comida.gameObject.SetActive(true);
                break;
            case "arma":
                arma.gameObject.SetActive(true);
                break;
        }
    }
}
