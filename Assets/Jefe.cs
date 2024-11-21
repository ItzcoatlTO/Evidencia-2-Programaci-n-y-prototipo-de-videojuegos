using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar la escena

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
    [SerializeField] private Transform controladorAtaque; // Punto desde donde se detecta el ataque
    [SerializeField] private float radioAtaque = 1.5f; // Radio del ataque
    [SerializeField] private float daño = 10f; // Daño del ataque
    [SerializeField] private float tiempoEntreAtaques = 2f; // Tiempo entre ataques
    private float tiempoSiguienteAtaque;

    private bool mirandoDerecha = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // Inicializa la vida del jefe
        vidaActual = vidaMaxima;

        // Inicializa la barra de vida si está asignada
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

        // Si está dentro del rango de ataque y puede atacar
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
        animator.Play("Attack"); // Reproduce la animación "Attack"
    }

    // Método llamado por el Animation Event en la animación "Attack"
    public void Ataque()
    {
        Debug.Log("El jefe está atacando.");

        // Detectar al jugador dentro del rango de ataque
        Collider2D[] jugadores = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque);

        foreach (Collider2D jugador in jugadores)
        {
            if (jugador.CompareTag("Player"))
            {
                jugador.GetComponent<CombateJugador>().TomarDaño(daño); // Aplica daño al jugador
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
        animator.Play("Death"); // Reproduce la animación de muerte
        rb2D.linearVelocity = Vector2.zero;
        rb2D.bodyType = RigidbodyType2D.Static;

        // Espera un momento y luego reinicia la escena
        StartCoroutine(ReiniciarEscena());
    }

    private IEnumerator ReiniciarEscena()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos antes de reiniciar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena actual
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el jugador toca al jefe, evitar solapamiento
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
