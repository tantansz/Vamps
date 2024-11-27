using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    public Slider slider;


    public void ColocarVidaMaxima(float vida) //esta função serve para vida sempre comecar no maximo, por exemplo se eu alterar o maximo dela para 80 a barra de vida nao fica parecendo que ta faltando vida, ela se ajusta conforme eu altero.
    {
        slider.maxValue = vida;
        slider.value = vida;
    }

    public void AlterarVida (float vida) // está função altera o valor do slider(que é a barra de vida), conforme ele recebe um valor no parametro vida.
    {
        slider.value = vida; 
    }

}
