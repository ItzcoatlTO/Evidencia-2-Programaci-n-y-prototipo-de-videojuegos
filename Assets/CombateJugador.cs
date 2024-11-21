using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateJugador : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float vidaMaxima = 100f; // Vida máxima del jugador
    private float vidaActual;
    [SerializeField] private BarraDeVida barraDeVida; // Referencia a la barra de vida del jugador

    [Header("Ataque")]
    [SerializeField] private float dañoAtaque = 10f; // Daño que inflige el jugador
    [SerializeField] private float rangoAtaque = 1.5f; // Rango de ataque del jugador
    [SerializeField] private Transform puntoAtaque; // Punto desde donde se detecta el ataque
    [SerializeField] private float tiempoEntreAtaques = 1f; // Tiempo entre ataques
    private float tiempoSiguienteAtaque;

    private Animator animator;

    private void Start()
    {
        vidaActual = vidaMaxima;

        // Inicializa la barra de vida del jugador
        if (barraDeVida != null)
        {
            barraDeVida.InicializarBarraDeVida(vidaMaxima);
        }

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && tiempoSiguienteAtaque <= 0) // "Fire1" equivale a clic izquierdo o botón de ataque
        {
            Atacar();
            tiempoSiguienteAtaque = tiempoEntreAtaques;
        }
    }

    private void Atacar()
    {
        Debug.Log("El jugador está atacando.");
        animator.Play("Golpe"); // Reproduce la animación "Golpe"

        // Detectar enemigos dentro del rango de ataque
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(puntoAtaque.position, rangoAtaque);

        foreach (Collider2D enemigo in enemigos)
        {
            if (enemigo.CompareTag("Enemigo"))
            {
                enemigo.GetComponent<Jefe>().TomarDaño(dañoAtaque); // Aplica daño al enemigo
            }
        }
    }

    public void TomarDaño(float daño)
    {
        // Reduce la vida del jugador
        vidaActual = Mathf.Clamp(vidaActual - daño, 0, vidaMaxima);

        // Actualiza la barra de vida del jugador
        if (barraDeVida != null)
        {
            barraDeVida.CambiarVidaActual(vidaActual);
        }

        // Si la vida llega a 0, activa la muerte
        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        Debug.Log("El jugador ha muerto.");
        animator.Play("Muerte"); // Reproduce la animación de muerte
        // Aquí puedes agregar lógica adicional para reiniciar el nivel o detener el juego
    }

    private void OnDrawGizmosSelected()
    {
        // Visualiza el rango de ataque en la vista del editor
        if (puntoAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoAtaque.position, rangoAtaque);
        }
    }
}
