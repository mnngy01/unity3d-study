using UnityEngine;

public class BoxHandle : MonoBehaviour
{
    public OpenBox box;
    private bool playerNearby = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPickup pickup = FindFirstObjectByType<PlayerPickup>();

            // 열쇠를 들고 있을 때만 상자 열기
            if (pickup != null && pickup.HasKey())
            {
                box.Open();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
