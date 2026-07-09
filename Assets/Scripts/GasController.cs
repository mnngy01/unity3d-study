using UnityEngine;

public class GasController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ParticleSystem gasParticle;
    [SerializeField] private Collider gasCollider;

    private bool isBlocked = false;

    // 가스 상태 변경
    public void SetBlocked(bool blocked)
    {
        // 상태가 같으면 아무것도 안 함
        if (isBlocked == blocked)
            return;

        isBlocked = blocked;    // BlockZone에서 상태 전달

        if (isBlocked)  // 가스 구멍 막으면
        {
            gasParticle.Stop();
            gasCollider.enabled = false;    // 플레이어 리타이어 감지 off
        }
        else    // 가스 구멍 안 막으면
        {
            gasParticle.Play();
            gasCollider.enabled = true;     // 플레이어 리타이어 감지 on
        }
    }
}
