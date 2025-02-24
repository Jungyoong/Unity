using System.Collections.Generic;
using UnityEngine;

public class NodePath : MonoBehaviour
{
    public NodePath[] nodePaths;
    public LayerMask nodePathLayer;
    public bool coverNode;

    void Start()
    {
        Collider thisCollider = GetComponent<Collider>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f, nodePathLayer);
        List<NodePath> linkedPaths = new();

        foreach (Collider collider in hitColliders)
        {
            if (collider.transform.position.z > transform.position.z && collider != thisCollider)
            {
                NodePath nodePath = collider.GetComponent<NodePath>();
                linkedPaths.Add(nodePath);
            }
        }

        nodePaths = linkedPaths.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
