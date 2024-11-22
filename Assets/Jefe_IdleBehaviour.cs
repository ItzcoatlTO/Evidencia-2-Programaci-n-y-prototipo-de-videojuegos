using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jefe_IdleBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("numeroAleatorio", Random.Range(0, 2));
    }
}
