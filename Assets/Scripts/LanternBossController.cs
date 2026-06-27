using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.AI;
using NUnit.Framework;

public class LanternBossController : MonoBehaviour
{
    [Header("배회 범위")]
    public Vector3 centerPosition;      // 배회 중심
    public float rangeX = 10f;
    public float rangeY = 2f;
    public float rangeZ = 10f;

    [Header("이동 설정")]
    public float moveSpeed = 5f;        // 평소 속도
    public float investigateSpeed = 10f; // 소리 추적 속도
    public float waitTime = 2f;

    private Vector3 targetPosition;     // 현재 목표 위치
    private float currentSpeed;         // 현재 이동 속도
    private bool isWaiting = false;

    void Start()
    {
        // 처음 목적지 생성
        PickRandomDestination();

        // 처음 이동 속도 
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        // 대기 중이면 이동 X
        if (isWaiting)
            return;
        
        MoveToDestination();
    }

    // 랜덤 목적지 선택
    void PickRandomDestination()
    {
        targetPosition = new Vector3(
            Random.Range(centerPosition.x - rangeX, centerPosition.x + rangeX),
            Random.Range(centerPosition.y - rangeY, centerPosition.y + rangeY),
            Random.Range(centerPosition.z - rangeZ, centerPosition.z + rangeZ)
        );
    }

    // 목적지로 이동
    void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            currentSpeed * Time.deltaTime
        );

        // 이동 방향을 바라보게 회전
        Vector3 direction = targetPosition - transform.position;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                5f * Time.deltaTime
            );
        }

        // 목적지 도착
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    // 목적지 도착 후 잠시 대기
    IEnumerator WaitAndMove()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);

        currentSpeed = moveSpeed;   // 이동속도 설정

        PickRandomDestination();    // 목적지 설정

        isWaiting = false;
    }

    // Scene 에서 배회 범위 확인용
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(
            centerPosition,
            new Vector3(rangeX * 2, rangeY * 2, rangeZ * 2)
        );
    }

    // 소리를 들었을 때 호출
    public void HearSound(Vector3 soundPosition)
    {
        StopAllCoroutines();

        isWaiting = false;
        
        currentSpeed = investigateSpeed;

        // 소리가 들린 곳으로 타겟위치 변경
        targetPosition = new Vector3(
            soundPosition.x,
            transform.position.y,
            soundPosition.z
        );
    }
}