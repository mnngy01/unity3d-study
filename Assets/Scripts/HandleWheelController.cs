using UnityEngine;

public class HandleWheelController : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private float rotateSpeed = 180f;
    [SerializeField] private RailController railController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private RailPuzzleController puzzleController;

    private Transform player;
    private bool isUsing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // 너무 멀어지면 자동 해제
        if (Vector3.Distance(player.position, transform.position) > interactRange)
        {
            isUsing = false;
            return;
        }

        // E키로 상호작용: 잡기 / 놓기
        if (Input.GetKeyDown(KeyCode.E))
        {
            isUsing = !isUsing;

            if (isUsing)
            {
                Debug.Log("Handle Grab");
                
                // todo
                playerController.SetMovementEnabled(false);
                playerController.SetJumpEnabled(false);
                // Animator.SetBool("UseHandle", true);
            }
            else
            {
                Debug.Log("Handle Release");

                // todo
                playerController.SetMovementEnabled(true);
                playerController.SetJumpEnabled(true);
                // Animator.SetBool("UseHandle", false);
            }
        }

        if (!isUsing)
            return;
        
        // D : 시계 방향 회전
        if (isUsing && Input.GetKey(KeyCode.D))
        {
            transform.Rotate(
                0f,
                rotateSpeed * Time.deltaTime,
                0f,
                Space.Self
            );

            railController.RotateRail(-1f);
        }

        // D를 떼면 퍼즐 풀었는지 검사
        if (isUsing && Input.GetKeyUp(KeyCode.D))
        {
            railController.SnapRotation();
            puzzleController.CheckPuzzle();
        }
        
    }
}
