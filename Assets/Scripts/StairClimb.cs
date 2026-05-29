using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class StairClimb : MonoBehaviour
{
    public float moveSpeed = 4f;    // 이동 속도
    public float jumpForce = 20f;    // 점프 변수
    public float rotationSpeed = 10f; // 회전 속도
    public float maxStepHeight = 0.5f;  // 오를 수 있는 최대 계단 높이
    public float stepCheckDistance = 0.4f;   // 계단 감지 전방 거리
    public float stepUpForce = 8f;  // 계단 오를 때 위쪽으로 작용하는 힘
    public float stepSmoothing = 15f;    // 계단 오르기 스무싱 속도
    public float maxSlopeAngle = 45f;   // 지면으로 인정하는 최대 경사각
    public float groundCheckDistance = 0.15f;    // 지면 감지 레이 길이
    public LayerMask groundLayer = ~0;  // 지면으로 인정할 레이어

    private Rigidbody rb;
    private CapsuleCollider col;

    private bool isGrounded;
    private bool isClimbingStep;
    private float targetStepY;  // 계단 오를 때 목표 y 위치
    private Vector3 moveInput;

    // 계단 감지용 하단/상단 레이 시작 위치 오프셋
    private float lowerRayOffset;   // 바닥에서 살짝 위
    private float upperRayOffset;   // maxStepHeight 높이


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        // 회전은 스크립트가 직접 제어
        rb.freezeRotation = true;
        
        lowerRayOffset = 0.05f;
        upperRayOffset = maxStepHeight + 0.05f;
    }

    void Start()
    {
        
    }

    void Update()
    {
        GatherInput();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        HandleStepClimb();
        ApplyMovement();
        ApplyRotation();
    }

    // 입력 수집
    void GatherInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Vector3 camForward = Camera.main.transform.forward; camForward.y = 0;
        Vector3 camRight   = Camera.main.transform.right;   camRight.y   = 0;
        
        moveInput = (camForward * v + camRight * h).normalized;
    }

    // 점프
    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // 지면 감지
    void CheckGround()
    {
        // 캡슐 하단 중심에서 아래로 짧은 레이
        Vector3 origin = transform.position + Vector3.up * (col.radius * 0.9f);
        isGrounded = Physics.SphereCast(
            origin,
            col.radius * 0.8f,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance + col.radius * 0.1f,
            groundLayer
        ) && Vector3.Angle(hit.normal, Vector3.up) <= maxSlopeAngle;
    }

    // 계단 감지 & 오르기
    void HandleStepClimb()
    {
        if (!isGrounded || moveInput.sqrMagnitude < 0.01f)
        {
            isClimbingStep = false;
            return;
        }

        Vector3 dir = moveInput;
    
        // 1. 하단 레이: maxStepHeight 아래에 벽이 있는가?
        Vector3 lowerOrigin = transform.position + Vector3.up * lowerRayOffset;
        bool lowerHit = Physics.Raycast(
            lowerOrigin, dir, out RaycastHit lowerInfo,
            stepCheckDistance, groundLayer
        );

        if (!lowerHit)
        {
            isClimbingStep = false;
            return;
        }

        // 2. 상단 레이: maxStepHeight 위에서 같은 방향 - 장애물이 없으면 오를 수 있는 계단
        Vector3 upperOrigin = transform.position + Vector3.up * upperRayOffset;
        bool upperHit = Physics.Raycast(
            upperOrigin, dir, stepCheckDistance, groundLayer
        );

        if (upperHit)
        {
            // 너무 높은 벽 : 계단이 아님 
            isClimbingStep = false;
            return;
        }

        // 3. 계단 윗면 높이 계산: 상단 레이 원점에서 아래로 레이캐스트
        Vector3 stepTopOrigin = transform.position
                            + dir * (stepCheckDistance * 0.8f)
                            + Vector3.up * upperRayOffset;
    
        if (Physics.Raycast(stepTopOrigin, Vector3.down, out RaycastHit stepTop, upperRayOffset + 0.1f, groundLayer))
        {
            float newY = stepTop.point.y + (col.height * 0.5f) + col.center.y;

            if (newY > transform.position.y + 0.01f)    // 실제로 올라가야 할 때만
            {
                targetStepY = newY;
                isClimbingStep = true;
            }
        }
    }

    // 이동 적용
    void ApplyMovement()
    {
        // 수평 속도 계산
        Vector3 desiredVelocity = moveInput * moveSpeed;

        if (isClimbingStep)
        {
            // 계단 오르는 중: Y위치를 부드럽게 올리기
            float newY = Mathf.Lerp(transform.position.y, targetStepY, Time.fixedDeltaTime * stepSmoothing);
            Vector3 newPos = new Vector3(transform.position.x + desiredVelocity.x * Time.fixedDeltaTime, newY, transform.position.z + desiredVelocity.z * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        } else {
            // 일반 이동: 현재 Y 속도(중력) 유지
            Vector3 currentVel = rb.linearVelocity;
            rb.linearVelocity = new Vector3(desiredVelocity.x, currentVel.y, desiredVelocity.z);
        }
    }

    // 회전 적용
    void ApplyRotation()
    {
        if (moveInput.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(moveInput, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed));
    }

    // 디버그 기즈모
    void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<CapsuleCollider>();
        if (col == null) return;

        Vector3 fwd = transform.forward;

        // 하단 레이 (노란색)
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.05f, fwd * stepCheckDistance);

        // 상단 레이 (초록색)
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up * (maxStepHeight + 0.05f), fwd * stepCheckDistance);

        // 지면 감지 구 (파란색)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * (col.radius * 0.9f - groundCheckDistance), col.radius * 0.8f);
    }
}



