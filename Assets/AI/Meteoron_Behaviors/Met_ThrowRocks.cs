using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Met_ThrowRocks : StateMachineBehaviour
{
    AIController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<AIController>();
        controller.Attack(controller.CurrentTarget != null ? controller.CurrentTarget : controller.LastKnownTarget);
        controller.ResetCanAttack(animator);

        animator.SetBool("mIsAbility", true);
        animator.SetInteger("mNextAbility", Random.Range(0, 2));
    }
}
