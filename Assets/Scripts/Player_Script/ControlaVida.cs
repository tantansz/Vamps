using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaVida : MonoBehaviour
{
    public BarraDeVida barra;
    private float vida = 100;

    void Start()
    {
        // Ajusta a vida no início conforme o valor definido e atualiza a barra de vida
        barra.ColocarVidaMaxima(vida);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) //toma dano apertando F
        {
            TomarDano(10.0f);
        }

        if (Input.GetKeyDown(KeyCode.R)) // recupera a vida apertando R
        {
            RecuperarVida(10.0f);  
        }
    }

    public void TomarDano(float dano)
    {
        vida -= dano;  
        barra.AlterarVida(vida); 

        if (vida <= 0)
        {
            Morrer();  // Chama a função Morrer se a vida chegar a 0
        }
    }

    public void RecuperarVida(float quantidade)
    {
        vida += quantidade;  
        if (vida > barra.slider.maxValue)
        {
            vida = barra.slider.maxValue;  // Garante que a vida não exceda o máximo da barra slider
        }
        barra.AlterarVida(vida);  
    }

    void Morrer()
    {
        Debug.Log("Player morreu!");  // Mensagem para aparecer no terminal avisando que morreu 
        Destroy(gameObject);  // Destroi o player
    }
}