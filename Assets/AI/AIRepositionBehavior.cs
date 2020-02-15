using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRepositionBehavior : StateMachineBehaviour
{

    private AIController controller;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<AIController>();
        var distanceToTarget = animator.GetFloat("mDistanceToTarget");
        var direction = Random.Range(0, 2) > 0 ? animator.transform.right : -animator.transform.right;
        direction *= 3f;
        Vector3 runTo = controller.transform.position + (-controller.transform.forward * (controller.rangedRange - distanceToTarget) + direction);

        NavMeshHit hit;
        NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetAreaFromName("Default"));

        // And get it to head towards the found NavMesh position
        controller.NavAgent.SetDestination(hit.position);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("mIsRepositioned", controller.NavAgent.remainingDistance <= 1.0f);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.NavAgent.ResetPath();
    }


}
