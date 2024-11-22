using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Jefe : MonoBehaviour
{
    private Animator animator;
    public Rigidbody2D rb2D;
    public Transform jugador;

    [Header("Vida")]
    [SerializeField] private float vidaMaxima = 100f;
    private float vidaActual;
    [SerializeField] private BarraDeVida barraDeVida;

    [Header("Ataque")]
    [SerializeField] private Transform controladorAtaque; 
    [SerializeField] private float radioAtaque = 1.5f; 
    [SerializeField] private float daño = 10f; 
    [SerializeField] private float tiempoEntreAtaques = 2f; 
    private float tiempoSiguienteAtaque;

    private bool mirandoDerecha = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        vidaActual = vidaMaxima;
        if (barraDeVida != null)
        {
            barraDeVida.InicializarBarraDeVida(vidaMaxima);
        }
    }

    private void Update()
    {
        MirarJugador();

        float distanciaJugador = Vector2.Distance(transform.position, jugador.position);

        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }
        if (distanciaJugador <= radioAtaque && tiempoSiguienteAtaque <= 0)
        {
            Atacar();
            tiempoSiguienteAtaque = tiempoEntreAtaques;
        }
    }

    public void MirarJugador()
    {
        bool jugadorALaDerecha = jugador.position.x > transform.position.x;

        if (jugadorALaDerecha && !mirandoDerecha || !jugadorALaDerecha && mirandoDerecha)
        {
            mirandoDerecha = !mirandoDerecha;
            transform.eulerAngles = new Vector3(0, mirandoDerecha ? 0 : 180, 0);
        }
    }

    public void Atacar()
    {
        Debug.Log("El jefe inicia el ataque.");
        animator.Play("Attack");
    }
    public void Ataque()
    {
        Debug.Log("El jefe está atacando.");
        Collider2D[] jugadores = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque);

        foreach (Collider2D jugador in jugadores)
        {
            if (jugador.CompareTag("Player"))
            {
                jugador.GetComponent<CombateJugador>().TomarDaño(daño);
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
        else
        {
            Debug.Log("El jefe recibió daño.");
        }
    }

    private void Muerte()
    {
        Debug.Log("El jefe ha muerto.");
        animator.Play("Death");
        rb2D.linearVelocity = Vector2.zero;
        rb2D.bodyType = RigidbodyType2D.Static;
        StartCoroutine(ReiniciarEscena());
    }

    private IEnumerator ReiniciarEscena()
    {
        yield return new WaitForSeconds(2f); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
    }
}
