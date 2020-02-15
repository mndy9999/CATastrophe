﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFollowAndAttack : StateMachineBehaviour
{
    AIController controller;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<AIController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("mDistanceToTarget", controller.CurrentTarget == null ? 999 : Vector3.Distance(animator.transform.position, controller.CurrentTarget.position));
        animator.SetBool("mIsFollowing", controller.CurrentTarget != null);
        controller.RotateToTarget();
    }
}
