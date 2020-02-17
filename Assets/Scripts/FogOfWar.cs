using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{

    public GameObject FogOfWarPlane;

    public LayerMask FogLayer;
    public float Radius = 5.0f;

    private Transform Player;
    private float radiusSqrt { get { return Radius * Radius; } }

    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colours;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }
        Ray r = new Ray(Player.transform.position, Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 1000, FogLayer))
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = FogOfWarPlane.transform.TransformPoint(vertices[i]);
                float dist = Vector3.SqrMagnitude(v - hit.point);
                if (dist < radiusSqrt)
                {
                    float alpha = 0;
                    colours[i].a = alpha;
                }
            }
            UpdateColour();
        }
    }

    private void Initialize()
    {
 
        mesh = FogOfWarPlane.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colours = new Color[vertices.Length];
        for(int i = 0; i < vertices.Length; i++)
        {
            colours[i] = Color.black;
        }

        UpdateColour();

    }

    private void UpdateColour()
    {
        mesh.colors = colours;
    }


}
