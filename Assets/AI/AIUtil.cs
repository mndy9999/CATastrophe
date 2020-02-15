using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class AIUtil
{
    public static Quaternion UpdateRotation(Transform origin, Vector3 target, float turnSpeed)
    {
        var moveDirection = target - origin.position;
        if (moveDirection != Vector3.zero)
            return Quaternion.Slerp(origin.rotation, Quaternion.LookRotation(moveDirection), turnSpeed * Time.deltaTime);
        else
            return origin.rotation;
    }

    /// <summary>
    /// Get random position on the walkable mesh
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="dist"></param>
    /// <param name="layermask"></param>
    /// <returns></returns>
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        //generate a random target position close to the GO 
        Vector3 randDirection = Random.insideUnitCircle * dist;
        randDirection += origin;

        //get a valid position as close as possible to the target
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }



    public static Vector3 RandomNavSphereRaycast(Vector3 origin, float dist, int layermask)
    {
        var pos = RandomNavSphere(origin, dist, layermask);

        //get direction and distance
        Vector3 dirToTarget = pos - origin;
        float distToTarget = Vector3.Distance(origin, pos);

        //if the raycast does not hit anything, return the position
        if (!Physics.Raycast(origin, dirToTarget, distToTarget))
        {
            return pos;
        }
        //otherwise calculate it again

        return RandomNavSphere(origin, dist, layermask);

    }
}
