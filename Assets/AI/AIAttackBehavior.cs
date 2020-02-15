using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackBehavior : StateMachineBehaviour
{
    AIController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<AIController>();
        controller.Attack(controller.CurrentTarget != null ? controller.CurrentTarget : controller.LastKnownTarget);
        controller.ResetCanAttack(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("mTargetInLine", false);
    }
}
