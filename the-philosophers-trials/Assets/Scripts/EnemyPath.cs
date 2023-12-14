using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

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
    [SerializeField] private AudioClip pathfindingSound;
    private AudioSource audioSource;
    private float detectionRange;
    private int waypointIndex = 0;
    private Pathfinding pathfinding;
    private const float AttackRange = 1f; // Define the range for attacking
    private bool followingPath;

    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
        pathfinding = new Pathfinding(gridVisualizer); // Create a new instance of Pathfinding
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        GameLost();
        UpdateDetectionRange();

        if (Physics.OverlapSphere(transform.position, detectionRange, playerLayer).Length > 0)
        {
            Vector3 playerPosition = playerTransform.position;
            if (!followingPath || Vector3.Distance(transform.position, playerPosition) > pathUpdateThreshold)
            {
                StopAllCoroutines();
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

    private void MoveToWaypoints()
    {
        if (transform.position != waypoints[waypointIndex].transform.position)
        {
            Vector3 nextPosition = waypoints[waypointIndex].transform.position;
            OrientTowards(nextPosition); // Update orientation
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
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
        if (!followingPath) // Check if the enemy is not already following a path
        {
            followingPath = true; // Set followingPath to true here
        }
        StopAllCoroutines();
        StartCoroutine(FollowPath(gridVisualizer.path));
    }
}

IEnumerator FollowPath(System.Collections.Generic.List<Node> path)
{
    foreach (var node in path)
    {
        Vector3 nextPosition = node.worldPosition;
        while (Vector3.Distance(transform.position, nextPosition) > 0.1f)
        {
                moveSpeed = 8;
            OrientTowards(nextPosition); 
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    followingPath = false; // Reset followingPath to false here
}

    // New method to orient the enemy towards the target position
    private void OrientTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }
    }

    private void GameLost()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= 1.5)
        {
            SceneManager.LoadScene("Lose Screen");
        }
    }
}
