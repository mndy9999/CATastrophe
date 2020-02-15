using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    public int meleeRange;
    public int rangedRange;

    private float mMoveSpeed;
    public float MoveSpeed {
        get
        {
            return mMoveSpeed;
        }
        set
        {
            mMoveSpeed = value;
            NavAgent.speed = value;
        }
    }
    public float TurnSpeed = 9;

    private AIFieldOfView fieldOfView;
    private EnemyAttackController enemyAttackController;
    public NavMeshAgent NavAgent;
    public Vector3 SpawnLocation;

    public Transform CurrentTarget
    {
        get
        {
            return fieldOfView.PlayerTarget;
        }
    }
    public Transform LastKnownTarget
    {
        get
        {
            return fieldOfView.LastKnownPlayerPos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<AIFieldOfView>();
        enemyAttackController = GetComponent<EnemyAttackController>();
        SpawnLocation = transform.position;
        MoveSpeed = 2.0f;
        NavAgent.angularSpeed = TurnSpeed;
    }

    public void Attack(Transform target)
    {
        enemyAttackController.Attack(target);
    }

    public void RotateToTarget()
    {
        var destination = CurrentTarget == null ? NavAgent.destination : CurrentTarget.position;
        transform.rotation = AIUtil.UpdateRotation(transform, destination, TurnSpeed);
    }

    public void ResetCanAttack(Animator animator)
    {
        StartCoroutine(ResetAttack(animator));
    }

    IEnumerator ResetAttack(Animator animator)
    {
        animator.SetBool("mCanAttack", false);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("mCanAttack", true);
    }

    private int GetCurrentStoppingDistance(Animator animator)
    {
        return animator.GetBool("mIsRanged") ? rangedRange : meleeRange;
    }

    public bool CheckTargetInRange(Animator animator)
    {
        var dist = GetCurrentStoppingDistance(animator);
        try
        {
            return Vector3.Distance(transform.position, CurrentTarget != null ? CurrentTarget.position : LastKnownTarget.position) <= dist;
        }
        catch
        {
            return false;
        }
    }

    public void ResetLastKnownTarget()
    {
        fieldOfView.LastKnownPlayerPos = null;
    }

}
