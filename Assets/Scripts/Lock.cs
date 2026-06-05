using UnityEngine;

public class Lock : MonoBehaviour
{
    public GameObject passwordPanel;
    private bool playerNearby = false;

    void Start()
    {
        
    }

    void Update()
    {
        // 자물쇠 근처에서 E 키
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            passwordPanel.SetActive(true);

            // 플레이어 이동 잠금
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
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
