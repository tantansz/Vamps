using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaVida : MonoBehaviour
{
    public BarraDeVida barra;
    private float vida = 100;
    

    // Start is called before the first frame update
    void Start()
    {
        //estou usando a função para ajustar a vida no inicio conforme eu alterar o valor da vida, e a barra sempre fica completa.
        vida = 100.0f;
        barra.ColocarVidaMaxima(vida);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // A condição é que toda vez que receber um input do "F" ele perde 10 de vida, alterar futuramente essa tecla "F" para quando ele tomar um dano de um objeto.
        {
            vida -= 10.0f;
            barra.AlterarVida(vida);
        }

        if (Input.GetKeyDown(KeyCode.R)) //Quando eu aperta R eu recupero vida, para uma possivel mecanica de usar um item de vida ou um objeto que recupera vida, alterando apenas o parametro que vai receber.
        {
            vida += 10.0f;
            barra.AlterarVida(vida);
        }
    }
}
