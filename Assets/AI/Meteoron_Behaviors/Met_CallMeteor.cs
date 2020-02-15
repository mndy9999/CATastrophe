using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Met_CallMeteor : StateMachineBehaviour
{
    AIController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<AIController>();
        controller.Attack(controller.CurrentTarget != null ? controller.CurrentTarget : controller.LastKnownTarget);
        controller.ResetCanAttack(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("mIsAbility", false);
        animator.SetBool("mIsRanged", false);
        animator.SetInteger("mPunchesLeft", Random.Range(0, 3));
    }
}
