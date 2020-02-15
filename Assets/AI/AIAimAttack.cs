using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAimRangedBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var rand = Random.Range(0, 2);
        animator.SetBool("mMelee", rand > 0);
    }

}
