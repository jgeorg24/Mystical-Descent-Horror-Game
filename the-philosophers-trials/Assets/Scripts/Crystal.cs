using UnityEngine;

public class Crystal : MonoBehaviour
{
    private static int crystalCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Make sure your player GameObject has the tag "Player"
        {
            crystalCount++;
            Debug.Log("Crystals collected: " + crystalCount);

            // Deactivate or destroy the crystal
            gameObject.SetActive(false); // Deactivate
            // Destroy(gameObject); // Alternatively, destroy the crystal
        }
    }
}
