using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void InicializarBarraDeVida(float vidaMaxima)
    {
        slider.maxValue = vidaMaxima;
        slider.value = vidaMaxima;
    }

    public void CambiarVidaActual(float vidaActual)
    {
        slider.value = Mathf.Clamp(vidaActual, 0, slider.maxValue); // Asegura que la barra no exceda los límites
    }
}
