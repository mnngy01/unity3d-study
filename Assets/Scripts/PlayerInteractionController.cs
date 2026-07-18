using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public float interactRange = 2f;    // 상호작용 거리
    public float pushSpeed = 2.5f;      // 밀기 속도
    public float grabOffset = 0.8f;     // 상자 - 플레이어 거리

    private PlayerController playerController;  // PlayerController 스크립트 호출
    private bool isPushing = false;     // 밀기 상태
    private Rigidbody targetBox;        // 잡은 상자
    private int grabSide = -1;           // -1: 왼쪽 | 1: 오른쪽

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // E키 : 잡기 / 놓기
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isPushing)
                TryGrab();
            else
                Release();
        }

        // 잡은 상태에서 이동
        if (isPushing && targetBox != null)
        {
            // 플레이어를 상자 옆에 고정
            transform.position = new Vector3(
                targetBox.position.x + grabSide * (grabOffset + 1.5f),
                transform.position.y,
                transform.position.z
            );

            float h = Input.GetAxisRaw("Horizontal");

            if (h != 0)
            {
                Vector3 move = Vector3.right * h * pushSpeed;   // x축으로만 이동

                // 플레이어와 상자 같이 이동
                transform.position += move * Time.deltaTime;
                targetBox.MovePosition(targetBox.position + move * Time.deltaTime);
            }
        }
    }

    
    // 상자 잡기
    void TryGrab()
    {
        Vector3 origin = transform.position + Vector3.up * 1f;

        Debug.DrawRay(origin, transform.forward * interactRange, Color.red, 2f);

        if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Pushable"))
            {
                targetBox = hit.collider.GetComponent<Rigidbody>();

                isPushing = true;
                playerController.isPushing = true;

                // 플레이어가 상자의 어느 쪽에 있는지
                grabSide = (transform.position.x < targetBox.position.x) ? -1 : 1;

                // 플레이어를 상자 옆으로 이동
                transform.position = new Vector3(
                    targetBox.position.x + grabSide * (grabOffset + 1.5f),
                    transform.position.y,
                    transform.position.z
                );
            }
        }
    }

    // 상자 놓기
    void Release()
    {
        playerController.isPushing = false;
        isPushing = false;
        targetBox = null;
    }
}
