using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private LayerMask player;

    private int waypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Set enemy position to start at 1st waypoint
        transform.position = waypoints[waypoint].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.OverlapSphere(transform.position, 6f, player).Length > 0)
        {
            Collider[] collider = Physics.OverlapSphere(transform.position, 6f, player);
            transform.position = Vector3.MoveTowards(transform.position, collider[0].transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            if (transform.position != waypoints[waypoint].transform.position)
            {
                // Move enemy from current position to next waypoint
                transform.position = Vector3.MoveTowards(transform.transform.position, waypoints[waypoint].transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                // Increment waypointindex only when the enemy reaches the target waypoint
                waypoint = (waypoint + 1) % waypoints.Length;
            }
        }
    }
}
