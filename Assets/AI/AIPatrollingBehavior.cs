using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrollingBehavior : StateMachineBehaviour
{
    private AIController controller;
    private AIPatrolWaypoints patrolWaypoints;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<AIController>();
        patrolWaypoints = animator.gameObject.GetComponent<AIPatrolWaypoints>();
        controller.MoveSpeed = 2.0f;
        //generate random target position and start moving towards it
        var targetPos = patrolWaypoints.GetRandomWayPoint();
        controller.NavAgent.SetDestination(targetPos);
    } 

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.RotateToTarget();

        //change state when finding a target or when close to the target pos
        animator.SetBool("mIsFollowing", controller.CurrentTarget != null);
        animator.SetBool("mIsPatrolling", controller.NavAgent.remainingDistance > 1.0f);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.MoveSpeed = 5.0f;
        controller.NavAgent.ResetPath();
    }



}
