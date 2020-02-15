using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFieldOfView : MonoBehaviour
{
    public float ViewRadius;
    [Range(0, 360)]
    public float ViewAngle;

    private float baseViewAngle;
    private float baseViewRadius;

    public LayerMask targetMask;
    public LayerMask environmentMask;

    public Transform LastKnownPlayerPos;
    private Transform mPlayerTarget;
    public Transform PlayerTarget
    {
        get
        {
            return mPlayerTarget;
        }
        set
        {
            if (mPlayerTarget != null)
            {
                LastKnownPlayerPos = mPlayerTarget;
            }
            mPlayerTarget = value;
            ViewAngle = mPlayerTarget == null ? baseViewAngle : 360;
            ViewRadius = mPlayerTarget == null ? baseViewRadius : 20;
        }
    }
      
    public bool IsAttacked
    {
        set
        {
            ViewAngle = 360;
            ViewRadius = 20;
        }
    }

    private void Start()
    {
        baseViewAngle = ViewAngle;
        baseViewRadius = ViewRadius;
    }

    private void Update()
    {
        FindVisibleTargets();
    }

    void FindVisibleTargets()
    {
        Transform newPos = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);
        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < ViewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, environmentMask)){
                    newPos = target;
                }
            }
        }
        PlayerTarget = newPos;
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
