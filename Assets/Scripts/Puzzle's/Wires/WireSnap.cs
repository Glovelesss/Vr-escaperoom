using UnityEngine;

public class WireSnap : MonoBehaviour
{
    [SerializeField] private GameObject middle;
    [SerializeField] private GameObject end;

    [SerializeField] private float snapDistance = 0.5f;
    void Start()
    {
        
    }
    void Update()
    {
        if (middle != null && end != null)
        {
            float distance = Vector3.Distance(middle.transform.position, end.transform.position);
            if (distance < snapDistance)
            {
                middle.transform.position = end.transform.position;
            }
        }
    }
}
