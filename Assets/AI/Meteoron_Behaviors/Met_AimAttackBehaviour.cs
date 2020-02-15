using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Met_AimAttackBehaviour : StateMachineBehaviour
{
    AIController controller;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetInteger("mPunchesLeft") < 0)
        {
            animator.SetBool("mIsRanged", true);
        }
    }


}
