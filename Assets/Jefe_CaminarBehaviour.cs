using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jefe_CaminarBehaviour : StateMachineBehaviour
{
    private Jefe jefe;
    private Rigidbody2D rb2D;
    [SerializeField] private float velocidadMovimiento = 2f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        jefe = animator.GetComponent<Jefe>();
        rb2D = jefe.rb2D;

        if (jefe != null)
        {
            jefe.MirarJugador();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rb2D != null)
        {
            rb2D.linearVelocity = new Vector2(velocidadMovimiento, rb2D.linearVelocity.y) * animator.transform.right;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rb2D != null)
        {
            rb2D.linearVelocity = Vector2.zero;
        }
    }
}
