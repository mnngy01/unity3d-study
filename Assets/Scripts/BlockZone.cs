using System;
using UnityEngine;

public class BlockZone : MonoBehaviour
{
    [SerializeField] public GasController gasController;     // 연결할 가스

    // 상자가 들어오면 가스를 막음
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            gasController.SetBlocked(true);
        }
    }

    // 상자가 나가면 다시 가스 분출
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            gasController.SetBlocked(false);
        }
    }
}
