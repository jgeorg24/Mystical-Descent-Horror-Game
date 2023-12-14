using UnityEngine;

public class StoneCollection : MonoBehaviour
{
    private static int crystalCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            crystalCount++;
            Debug.Log("Crystals collected: " + crystalCount);

            // Destroy the crystal object
            Destroy(gameObject);
        }
    }
}
