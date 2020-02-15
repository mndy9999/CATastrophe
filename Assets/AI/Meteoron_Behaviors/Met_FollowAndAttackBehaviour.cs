using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Met_FollowAndAttackBehaviour : StateMachineBehaviour
{
    AIController controller;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<AIController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.RotateToTarget();
        if (controller.CheckTargetInRange(animator))
        {
            animator.SetBool("mTargetInRange", true);
            if (controller.CurrentTarget == null)
            {
                controller.ResetLastKnownTarget();
                animator.SetBool("mIsFollowing", false);
            }
        }
        else
        {
            animator.SetBool("mTargetInRange", false);
        }
    }
}
