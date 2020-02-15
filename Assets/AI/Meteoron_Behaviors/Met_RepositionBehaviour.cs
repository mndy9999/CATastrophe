using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Met_RepositionBehaviour : StateMachineBehaviour
{
    private AIController controller;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<AIController>();
        var distanceToTarget = Vector3.Distance(animator.transform.position, controller.CurrentTarget.position);
        var direction = Random.Range(0, 2) > 0 ? animator.transform.right : -animator.transform.right;
        direction *= 3f;

        Vector3 runTo = controller.transform.position + direction;
        if (distanceToTarget < controller.rangedRange)
        {
            runTo += -controller.transform.forward * (controller.rangedRange - distanceToTarget);
        }

        NavMeshHit hit;
        NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetAreaFromName("Default"));

        // And get it to head towards the found NavMesh position
        controller.NavAgent.SetDestination(hit.position);

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("mIsRepositioned", controller.NavAgent.remainingDistance <= controller.NavAgent.stoppingDistance);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.NavAgent.ResetPath();
    }
}
