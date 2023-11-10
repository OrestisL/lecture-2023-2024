using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshAgentVision : ManagedClass
{
    public LayerMask visibleLayers;
    public LayerMask obstacleLayers;
    public float viewRadius;
    public float viewDegrees;

    [SerializeField] private bool draw;

    public override void OnFixedUpdate()
    {
        Collider[] visibleColliders = Physics.OverlapSphere(transform.position, viewRadius, visibleLayers);
        if (visibleColliders.Length > 0)
        {
            for (int i = 0; i < visibleColliders.Length; i++)
            {
                Vector3 currentColliderPos = visibleColliders[i].transform.position;
                if (Physics.Linecast(transform.position, currentColliderPos, obstacleLayers))
                {
                    //there's an obstacle between the agent and the collider
                    if (draw)
                    {
                        Debug.DrawLine(transform.position, currentColliderPos, Color.red);
                    }
                    continue;
                }
                else
                {
                    //there are not obstacles between the agent and the collider
                    //check if visible object is in front
                    if (Mathf.Abs(Vector3.Angle(transform.forward, currentColliderPos - transform.position)) <= viewDegrees)
                    {
                        //in agent's field of view
                        //do something
                        if (draw)
                        {
                            Debug.DrawLine(transform.position, currentColliderPos, Color.green);
                        }
                    }
                    else
                    {
                        if (draw)
                        {
                            Debug.DrawLine(transform.position, currentColliderPos, Color.red);
                        }
                    }
                }

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
