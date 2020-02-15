using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Met_FollowBehaviour : StateMachineBehaviour
{
    private AIController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<AIController>();
        try
        {
            controller.NavAgent.SetDestination(controller.CurrentTarget != null ? controller.CurrentTarget.position : controller.LastKnownTarget.position);
        }
        catch
        {
            Debug.Log("Lost Target");
            animator.SetBool("mIsFollowing", false);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (controller.CurrentTarget != null)
        {
            controller.NavAgent.SetDestination(controller.CurrentTarget.position);
        }
        else if(controller.LastKnownTarget != null)
        {
            controller.NavAgent.SetDestination(controller.LastKnownTarget.position);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.NavAgent.ResetPath();
    }

}
