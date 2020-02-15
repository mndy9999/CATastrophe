using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Met_PunchBehaviour : StateMachineBehaviour
{
    AIController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<AIController>();
        controller.Attack(controller.CurrentTarget != null ? controller.CurrentTarget : controller.LastKnownTarget);
        controller.ResetCanAttack(animator);

        var left = animator.GetInteger("mPunchesLeft") - 1;
        animator.SetInteger("mPunchesLeft", left);
    }
}
