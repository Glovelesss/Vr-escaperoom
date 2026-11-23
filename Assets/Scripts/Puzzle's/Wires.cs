using System.Collections.Generic;
using UnityEngine;

public class Wires : MonoBehaviour
{
    LineRenderer lineRenderer;
    public List<GameObject> points;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    
    void Update()
    {
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i].transform.position);
        }
    }
}
