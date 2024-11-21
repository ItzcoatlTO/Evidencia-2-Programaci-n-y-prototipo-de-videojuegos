using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jefe_HabilidadBehaviour : StateMachineBehaviour
{
    [SerializeField] private GameObject habilidad;
    [SerializeField] private float offsetY = 2f;
    private Jefe jefe;
    private Transform jugador;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        jefe = animator.GetComponent<Jefe>();

        if (jefe != null)
        {
            jugador = jefe.jugador;

            // Asegura que el jefe mire hacia el jugador
            jefe.MirarJugador();

            // Calcula la posición de aparición de la habilidad con un offset en Y
            if (jugador != null)
            {
                Vector2 posicionAparicion = new Vector2(jugador.position.x, jugador.position.y + offsetY);
                Instantiate(habilidad, posicionAparicion, Quaternion.identity);
            }
        }
    }
}
