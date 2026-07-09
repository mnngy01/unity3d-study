using UnityEngine;

public class GasDamage : MonoBehaviour
{
    // gas collider 에 충돌하면 플레이어 리타이어
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        Debug.Log("Player Retire");
    }
}
