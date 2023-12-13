using UnityEngine;
using System.Collections;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GridVisualizer gridVisualizer;
    [SerializeField] private Movement playerMovement; // Reference to the player's movement script
    [SerializeField] private float baseDetectionRange = 6f;
    [SerializeField] private float pathUpdateThreshold = 0.1f; // Define a threshold for path update

    private float detectionRange;
    private int waypointIndex = 0;
    private Pathfinding pathfinding;
    private const float AttackRange = 0.5f; // Define the range for attacking
    private bool followingPath;

    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
        pathfinding = new Pathfinding(gridVisualizer); // Create a new instance of Pathfinding
        StartCoroutine(PrintPlayerState());
    }

    void Update()
    {
        UpdateDetectionRange();

        if (Physics.OverlapSphere(transform.position, detectionRange, playerLayer).Length > 0)
        {
            Vector3 playerPosition = playerTransform.position;
            if (!followingPath || Vector3.Distance(transform.position, playerPosition) > pathUpdateThreshold)
            {
                StopAllCoroutines(); // Stop the current path-following coroutine
                FindPathToPlayer(playerPosition);
            }
        }
        else if (!followingPath)
        {
            MoveToWaypoints();
        }
    }

    private void UpdateDetectionRange()
    {
        if (playerMovement.IsRunning)
        {
            detectionRange = baseDetectionRange * 2; // Double the range
        }
        else if (playerMovement.IsCrouching)
        {
            detectionRange = baseDetectionRange / 2; // Half the range
        }
        else
        {
            detectionRange = baseDetectionRange;
        }
    }

    IEnumerator PrintPlayerState()
    {
        while (true)
        {
            string state = playerMovement.IsRunning ? "Running" : playerMovement.IsCrouching ? "Crouching" : "Walking";
            Debug.Log("Player State: " + state);
            yield return new WaitForSeconds(2f);
        }
    }

    private void MoveToWaypoints()
    {
        if (transform.position != waypoints[waypointIndex].transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }

    private void FindPathToPlayer(Vector3 playerPosition)
    {
        pathfinding.FindPath(transform.position, playerPosition);
        if (gridVisualizer.path != null && gridVisualizer.path.Count > 0)
        {
            StopAllCoroutines();
            StartCoroutine(FollowPath(gridVisualizer.path));
        }
    }

IEnumerator FollowPath(System.Collections.Generic.List<Node> path)
    {
        followingPath = true;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Node node = path[i];
            // Move towards each node in the path except the last one
            while (Vector3.Distance(transform.position, node.worldPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, node.worldPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // Handle the last node (player's position) differently
        Node lastNode = path[path.Count - 1];
        while (Vector3.Distance(transform.position, lastNode.worldPosition) > AttackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, lastNode.worldPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Trigger attack
        AttackPlayer();

        followingPath = false;
    }

    private void AttackPlayer()
    {
        Debug.Log("Attacked player");
        // Here you can add more logic for what happens when the player is attacked.
    }
}
