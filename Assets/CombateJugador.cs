using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateJugador : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float vidaMaxima = 100f; 
    private float vidaActual;
    [SerializeField] private BarraDeVida barraDeVida; 

    [Header("Ataque")]
    [SerializeField] private float dañoAtaque = 10f; 
    [SerializeField] private float rangoAtaque = 1.5f; 
    [SerializeField] private Transform puntoAtaque; 
    [SerializeField] private float tiempoEntreAtaques = 1f; 
    private float tiempoSiguienteAtaque;

    private Animator animator;

    private void Start()
    {
        vidaActual = vidaMaxima;

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

        if (Input.GetButtonDown("Fire1") && tiempoSiguienteAtaque <= 0)
        {
            Atacar();
            tiempoSiguienteAtaque = tiempoEntreAtaques;
        }
    }

    private void Atacar()
    {
        Debug.Log("El jugador está atacando.");
        animator.Play("Golpe"); 
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(puntoAtaque.position, rangoAtaque);

        foreach (Collider2D enemigo in enemigos)
        {
            if (enemigo.CompareTag("Enemigo"))
            {
                enemigo.GetComponent<Jefe>().TomarDaño(dañoAtaque); 
            }
        }
    }

    public void TomarDaño(float daño)
    {
        vidaActual = Mathf.Clamp(vidaActual - daño, 0, vidaMaxima);
        if (barraDeVida != null)
        {
            barraDeVida.CambiarVidaActual(vidaActual);
        }
        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        Debug.Log("El jugador ha muerto.");
        animator.Play("Muerte"); 
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoAtaque.position, rangoAtaque);
        }
    }
}
