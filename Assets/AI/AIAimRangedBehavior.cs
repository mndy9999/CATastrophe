using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAimAttack : StateMachineBehaviour
{
    AIController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<AIController>();
        controller.ResetCanAttack(animator);

        if (controller.CurrentTarget != null)
        {
            var dir = controller.CurrentTarget.transform.position - animator.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(animator.transform.position,dir,  out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                    animator.SetBool("mTargetInLine", true);
            }
        }
    }
}
