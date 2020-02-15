using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrolWaypoints : MonoBehaviour
{
    private CustomGrid grid;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        GetStartPlane();
    }

    // Update is called once per frame
    void Update()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void GetStartPlane()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 10.0f);
        grid = hit.collider.gameObject.GetComponent<CustomGrid>();
    }

    public Vector3 GetRandomWayPoint()
    {
        var randX = Random.Range(0, grid.gridSizeX);
        var randY = Random.Range(0, grid.gridSizeY);

        if (grid.NodeFromXYCoords(randX, randY).walkable)
        {
            Vector3 nextPos = transform.position;
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(grid.NodeFromXYCoords(randX, randY).worldPosition, out myNavHit, 100, -1))
            {
                nextPos = myNavHit.position;
            }
            return nextPos;
        }
        else
        {
            return GetRandomWayPoint();
        }
    }



}
